using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Bson;
using Mandrill;
using ConsoleApplication2._0;

namespace ConsoleApplication1
{
    // ------------------------------------------------------------------------------
    // Funcion: proveer toda la funcionalidad de generación y envio de reportes.
    // Propiedades: 
    //          reportsDetails  -> almacena el detalle de envio de un reporte semanal.
    //                             almacena informacion referente a el estado de cada reporte perteneciente al reporte semanal.
    //          reportState     -> almacena el estado general de envio de un reporte semanal.
    //          idRep           -> identificador de un reporte semanal.
    //                             se utiliza cuando se esta realizando un reenvio.
    //          newRep          -> representa si se esta realizando un nuevo envio o un reenvio.
    //                             newRep = true    -> nuevo envio.
    //                             newRep = false   -> reenvio.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    class Program
    {
        List<ReportDetails> reportsDetails = new List<ReportDetails>();
        ReportState reportState;
        int idRep;
        bool newRep = true;


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: ejecuta el metodo que provee toda la funcionalidad.
        // Parametros: 
        //          args    -> si se ejecuta desde la aplicacion web el reenvio de reportes en caso de que haya ocurrido un error
        //                      args contiene los siguientes parametros:
        //                      allReports  -> indica si se reenviaran todos los reportes o solamente un porcentaje del total.
        //                      idRep       -> representa el reporte semanal que se reenviara.
        //----------------------------------------------------------------------------------------------------------------
        static void Main(string[] args)
        {
            Console.WriteLine("---------------------------CreateAndSendingReports-------------------------");
            Console.WriteLine("starting...................................................................");

            string[] argsReport = {};
            foreach(string arg in args)
            {
                argsReport = arg.Split(',');
            }

            Program anInstanceofClass = new Program();
            if (argsReport.Count() > 0)
            {
                anInstanceofClass.core(argsReport[0], argsReport[1]);                
            }
            else
            {
                anInstanceofClass.core();
            }
            Console.Clear();
            Console.WriteLine(":::::FINALIZANDO:::::");
            Thread.Sleep(3000);
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: ejecuta toda la funcionalidad.
        // Parametros: 
        //          allReports  ->  indica si se reenviaran todos los reportes o solamente un porcentaje del total.
        //          idRep       ->  representa el reporte semanal que se reenviara.
        //----------------------------------------------------------------------------------------------------------------
        public void core(string allReports = "all", string idRep = "-1")
        {
            // Instanciando objeto que almacenara la informacion del envio.
            reportState = new ReportState(0, 0.0m, 0);

            // Nuevo reporte
            if (idRep == "-1")
            {
                // Almacenando en la base de datos la información inicial del envio.
                storeStateIni();
            }
            // Reenvio
            else
            {
                // Asignando variables utilizadas en reenvio.
                this.idRep = Convert.ToInt32(idRep);
                this.newRep = false;
            }

            Anviz_Data_BaseEntities dc = new Anviz_Data_BaseEntities();

            DateTime lastmonday;
            WeeklyReportState weeklyReportState;
            List<WeeklyReportDetail> weeklyReportDetails;

            var client = new MongoClient();
            var db = client.GetDatabase("structures");

            var collection = db.GetCollection<InfoDept>("responsables");
            var test = from d in collection.AsQueryable<InfoDept>()
                       select d.Id_Dept;
            var all = from e in dc.Userinfoes select new GetPeople { userid = e.Userid, name = e.Name, email = e.Address };

            // Realizando pruebas de conexion a las bases de datos.
            Console.WriteLine("Conecting with Mongodb................................");
            Thread.Sleep(2300);
            try
            {
                if (test.Count() <= 1)
                {
                    Console.WriteLine("Error!!!: cannot conect with MongoDb  .................");
                    Thread.Sleep(5000);


                    // Error al conectar con mongo.
                    // Ningun reporte generado.
                    reportState.TypeError = 1;
                    // storeStateReport();
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error!!!: cannot conect with MongoDb  " + e + ".................");
                Thread.Sleep(5000);


                // Error al conectar con mongo.
                // Ningun reporte generado.
                reportState.TypeError = 1;
                // storeStateReport();
                return;
            }

            Console.WriteLine("status conection::: success with MongoDb................................");
            Console.WriteLine("Conecting with Mssql................................");
            Thread.Sleep(2300);
            try
            {
                var whynot = all.Count() <= 1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error!!!: cannot conect with Mssql!!!!  ................................");
                Console.WriteLine(e);
                Thread.Sleep(5000);


                // Error al conectar con sqlserver.
                // Ningun reporte generado.
                reportState.TypeError = 1;
                // storeStateReport();
                return;
            }
            // Conexion a bases de datos exitosa.
            Console.WriteLine("status conection::: success with Mssql................................");

            // Listas utilizadas en reenvio de reportes.
            List<string> special = new List<string>();
            List<string> departments = new List<string>();
            List<string> teams = new List<string>();
            List<string> employees = new List<string>();

            // Enviando todos los reportes.
            if (allReports == "all")
            {
                DateTime now = DateTime.Now;
                var oneweekAffter = now.AddDays(7);
                DateTime day = DateTime.Today;
                int offset = day.DayOfWeek - DayOfWeek.Monday;
                lastmonday = Convert.ToDateTime(day.AddDays(-offset - 7));
            }
                // Enviando solo un porcentaje del total de reportes.
            else
            {
                weeklyReportState = (from rep in dc.SP_AllWeeklyReportsState()
                                     where rep.Id == Convert.ToInt32(idRep)
                                     select new WeeklyReportState { Id = rep.Id, StateReport = rep.StateReport, PercentReports = rep.PercentReports, StartDate = Convert.ToDateTime(rep.StartDate), EndDate = Convert.ToDateTime(rep.EndDate), TypeError = rep.TypeError }).ToList()[0];
                weeklyReportDetails = (from repD in dc.SP_WeeklyReportState(Convert.ToInt32(idRep))
                                       select new WeeklyReportDetail { Id = repD.Id, TypeReport = repD.TypeReport, IdReceptor = repD.IdReceptor, StateReport = repD.StateReport, TypeError = repD.TypeError, IdWeeklyReportState = repD.IdWeeklyReportState }).ToList();
                lastmonday = weeklyReportState.StartDate;

                // Cargando los identificadores de los responsables de recibir los resportes que se reenviaran.
                special = (from wrd in weeklyReportDetails
                          where wrd.TypeReport == 0 && wrd.StateReport == 0
                          select Convert.ToString(wrd.IdReceptor)).ToList();
                departments = (from wrd in weeklyReportDetails
                              where wrd.TypeReport == 1 && wrd.StateReport == 0 
                              select Convert.ToString(wrd.IdReceptor)).ToList();
                teams = (from wrd in weeklyReportDetails
                         where wrd.TypeReport == 2 && wrd.StateReport == 0 
                        select Convert.ToString(wrd.IdReceptor)).ToList();
                employees = (from wrd in weeklyReportDetails
                             where wrd.TypeReport == 3 && wrd.StateReport == 0 
                            select Convert.ToString(wrd.IdReceptor)).ToList();
            }

            string monday = lastmonday.ToString("MM-dd-yyyy");
            string path = "";


            // Asignando fecha de inicio y finalización del reporte semanal.
            reportState.StartDate = lastmonday;
            reportState.EndDate = lastmonday.AddDays(6);

            // Creando directorios.
            string folder = @"C:\\Pdf\\Asistance_Report_" + monday;
            path = @"C:\\Pdf\\Asistance_Report_" + monday + "\\Depts\\";
            bool folderExists = Directory.Exists((folder));
            if (!folderExists)
                Directory.CreateDirectory((folder));
            folderExists = Directory.Exists((folder));
            folderExists = Directory.Exists((path));
            if (!folderExists)
                Directory.CreateDirectory((path));
            folderExists = Directory.Exists((path));

            // Creando reportes de departamentos.
            CreatePdf(test, collection, monday, true, path);

            foreach (var items in test)
            {
                var managers = collection
                      .Find(a => a.Id_Dept == items)
                      .ToListAsync()
                       .Result;
                foreach (var i in managers)
                {
                    string[] sub_managers = i.Name;
                    List<GetReport> get = new List<GetReport>(i.GetReport);
                    path = @"C:\\Pdf\\Asistance_Report_" + monday + "\\SubDepts";
                    foreach (var sub in get)
                    {
                        Console.WriteLine("CreateReportforSubDept" + sub.subdept + "...........................................");
                        // Creando reportes de equipos. 
                        CreatePdf(sub.subdept, sub.deptID, monday, true, path);

                        //----------------------------------------------------------------------------------------------------------------
                        // Enviando reportes de equipo.
                        //----------------------------------------------------------------------------------------------------------------
                        // Enviar todos los reportes.
                        if (allReports == "all")
                        {
                            // Enviando el tipo de reporte (2)
                            // Enviando el id del equipo
                            sendEmail(monday, sub.subName, sub.subdept, sub.subEmail, "\\SubDepts\\", 2, Convert.ToInt32(sub.deptID));
                        }
                        // Enviar un porcentaje de reportes.
                        else
                        {
                            foreach (string id in teams)
                            {
                                if (sub.deptID == id)
                                {
                                    // Enviando el tipo de reporte (2)
                                    // Enviando el id del equipo
                                    sendEmail(monday, sub.subName, sub.subdept, sub.subEmail, "\\SubDepts\\", 2, Convert.ToInt32(sub.deptID));
                                }
                            }
                        }
                    }

                    for (int y = 0; y < sub_managers.Length; y++)
                    {
                        //----------------------------------------------------------------------------------------------------------------
                        // Enviando reportes de departamentos.
                        //----------------------------------------------------------------------------------------------------------------
                        // Enviar todos los reportes.
                        if (allReports == "all")
                        {
                            // Enviando el tipo de reporte (1).
                            // Enviando el id del departamento.
                            sendEmail(monday, i.Name[y], "All" + i.Nombre_Dept, i.correo[y], "\\Depts\\", 1, Convert.ToInt32(i.Id_Dept));
                        }
                        // Enviar un porcentaje de reportes.
                        else
                        {
                            foreach (string id in departments)
                            {
                                if (i.Id_Dept == id)
                                {
                                    // Enviando el tipo de reporte (1).
                                    // Enviando el id del departamento.
                                    sendEmail(monday, i.Name[y], "All" + i.Nombre_Dept, i.correo[y], "\\Depts\\", 1, Convert.ToInt32(i.Id_Dept));
                                }
                            }
                        }
                    }
                }
            }

            //----------------------------------------------------------------------------------------------------------------
            // Enviando reportes LNO-HN.
            //----------------------------------------------------------------------------------------------------------------
            // sendEmail(monday, "Flor de Liz Reyes", "LNO-HN", "flor.reyes@laureate.net", "\\Depts\\", 0, 0);
            // sendEmail(monday, "Gina Almendares", "LNO-HN", "gina.almendares@laureate.net", "\\Depts\\", 0, 1);
            // Enviar todos los reportes.
            if (allReports == "all")
            {
                List<GetPeople> usersEspecial = (from specialEmp in dc.SP_SpecialReports() select new GetPeople { email = specialEmp.Address, name = specialEmp.Name, userid = specialEmp.Userid }).ToList();

                foreach (GetPeople spec in usersEspecial)
                {
                    // Enviando el tipo de reporte (0).
                    // Enviando el id del receptor.
                    sendEmail(monday, spec.name, "LNO-HN", spec.email, "\\Depts\\", 0, Convert.ToInt32(spec.userid));
                }
            }
            // Enviar un porcentaje de reportes.
            else
            {
                List<GetPeople> usersEspecial = (from specialEmp in dc.SP_SpecialReports() select new GetPeople { email = specialEmp.Address, name = specialEmp.Name, userid = specialEmp.Userid }).ToList();
                foreach (GetPeople spec in usersEspecial)
                {
                    foreach (string id in special)
                    {
                        if (spec.userid == id)
                        {
                            // Enviando el tipo de reporte (0).
                            // Enviando el id del receptor.
                            sendEmail(monday, spec.name, "LNO-HN", spec.email, "\\Depts\\", 0, Convert.ToInt32(spec.userid));
                        }
                    }
                    
                }
            }

            path = @"C:\\Pdf\\Asistance_Report_" + monday + "\\Employes\\";
            foreach (var items in all)
            {
                // Creando reportes de empleados 
                CreatePdf(items.name, items.userid, monday, false, path);

                //----------------------------------------------------------------------------------------------------------------
                // Enviando reportes de empleados.
                //----------------------------------------------------------------------------------------------------------------
                // Enviar todos los reportes.
                if (allReports == "all")
                {
                    // Enviando el tipo de reporte (3).
                    // Enviando el id del empleado.
                    sendEmail(monday, items.name, items.name, items.email, "\\Employes\\", 3, Convert.ToInt32(items.userid));
                }
                // Enviar un porcentaje de reportes.
                else
                {
                    foreach (string id in employees)
                    {
                        if (items.userid == id)
                        {
                            // Enviando el tipo de reporte (3).
                            // Enviando el id del empleado.
                            sendEmail(monday, items.name, items.name, items.email, "\\Employes\\", 3, Convert.ToInt32(items.userid));
                        }
                    }
                }
            }

            // Insertando en la base de datos el estado de envio y los detalles finales.
            storeStateReport();
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: 
        // Parametros: 
        //          name        -> 
        //          deptId      -> 
        //          date        -> 
        //          type        -> 
        //          path        -> 
        //----------------------------------------------------------------------------------------------------------------
        public void CreatePdf(string name, string deptId, string date, bool type, string path)
        {
            Document document = new Document(PageSize.A1, 5, 5, 155, 15);
            var Id = Convert.ToInt32(deptId);
            name = name.Replace("ñ", "n");
            name = name.Replace("Ñ", "n");
            name = name.Replace("\u0001", " ");
            string folder = @"C:\\Pdf\\Asistance_Report_" + date;
            bool folderExists = Directory.Exists((folder));
            if (!folderExists)
                Directory.CreateDirectory((folder));
            folderExists = Directory.Exists((path));
            if (!folderExists)
                Directory.CreateDirectory((path));
            folderExists = Directory.Exists((path));

            path = path + "\\Attendance_Report_" + name + "_" + date + ".pdf";
            if ((!System.IO.File.Exists(path)))
            {
                using (FileStream output = new FileStream((path), FileMode.Create))
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(document, output))
                    {
                        document.Open();
                        PdfPTable table = new PdfPTable(24);
                        PdfPTable tableLegend = new PdfPTable(24);
                        table.TotalWidth = 5000f;
                        document.Add(archive(table, Id, name, date, type));
                        document.Add(legend(tableLegend));
                        document.Close();
                        output.Close();
                    }
                }
            }
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: 
        // Parametros: 
        //          test        -> 
        //          collection  -> 
        //          dates       -> 
        //          type        -> 
        //          path        -> 
        //----------------------------------------------------------------------------------------------------------------
        public void CreatePdf(IQueryable<string> test, IMongoCollection<InfoDept> collection, string dates, bool type, string path)
        {
            DateTime now = DateTime.Now;
            var oneweekAffter = now.AddDays(7);

            Document document = new Document(PageSize.A1, 5, 5, 155, 15);
            int Id = 0;

            string filepath = path + "\\" + "Attendance_Report_LNO-HN" + "_" + dates + ".pdf";

            if ((!System.IO.File.Exists(filepath)))
            {
                using (FileStream output = new FileStream((filepath), FileMode.Create))
                {
                    using (PdfWriter writer = PdfWriter.GetInstance(document, output))
                    {
                        document.Open();

                        foreach (var items in test)
                        {
                            var managers = collection
                            .Find(a => a.Id_Dept == items)
                            .ToListAsync()
                             .Result;

                            foreach (var i in managers)
                            {
                                List<GetReport> get = new List<GetReport>(i.GetReport);
                                foreach (var sub in get)
                                {
                                    Id = Convert.ToInt32(sub.deptID);
                                    PdfPTable table = new PdfPTable(24);
                                    table.TotalWidth = 5000f;
                                    if (oneweekAffter.Month != now.Month)
                                    {
                                        document.Add(ArchiveSpin4(table, Id, sub.subdept, dates, type));
                                        // document.Add(archive(table, Id, sub.subdept, dates, type));
                                    }
                                    else
                                    {
                                        document.Add(archive(table, Id, sub.subdept, dates, type));
                                    }
                                }
                            }
                        }
                        if (oneweekAffter.Month == now.Month)
                        {
                            PdfPTable tableLegend = new PdfPTable(24);
                            document.Add(legend(tableLegend));
                        }
                        document.Close();
                        output.Close();
                    }
                }
            }
            Id = 0;
            foreach (var items in test)
            {
                var managers = collection
                .Find(a => a.Id_Dept == items)
                .ToListAsync()
                 .Result;

                foreach (var i in managers)
                {
                    for (var y = 0; y < i.Name.Length; y++)
                    {
                        if (oneweekAffter.Month != now.Month)
                        {
                            Console.WriteLine("ultimo lunes ................................");
                            filepath = path + "\\Attendance_Report_All_" + i.Nombre_Dept + "_" + dates + ".pdf";
                            if ((!System.IO.File.Exists(filepath)))
                            {
                                using (FileStream output = new FileStream((filepath), FileMode.Create))
                                {
                                    Document Alldocument = new Document(PageSize.A1, 5, 5, 155, 15);

                                    using (PdfWriter writer = PdfWriter.GetInstance(Alldocument, output))
                                    {
                                        Alldocument.Open();
                                        List<GetReport> get = new List<GetReport>(i.GetReport);
                                        foreach (var sub in get)
                                        {
                                            Id = Convert.ToInt32(sub.deptID);
                                            PdfPTable table = new PdfPTable(24);
                                            table.TotalWidth = 5000f;
                                            Alldocument.Add(ArchiveSpin4(table, Id, sub.subdept, dates, type));
                                            // Alldocument.Add(archive(table, Id, sub.subdept, dates, type));
                                        }
                                        Alldocument.Close();
                                        output.Close();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("no es el ultimo lunes del mes................................");
                            filepath = path + "\\Attendance_Report_All" + i.Nombre_Dept + "_" + dates + ".pdf";
                            if ((!System.IO.File.Exists(filepath)))
                            {
                                using (FileStream output = new FileStream((filepath), FileMode.Create))
                                {
                                    Document Alldocument = new Document(PageSize.A1, 5, 5, 155, 15);

                                    using (PdfWriter writer = PdfWriter.GetInstance(Alldocument, output))
                                    {
                                        Alldocument.Open();
                                        List<GetReport> get = new List<GetReport>(i.GetReport);
                                        foreach (var sub in get)
                                        {
                                            Id = Convert.ToInt32(sub.deptID);
                                            PdfPTable table = new PdfPTable(24);
                                            table.TotalWidth = 5000f;
                                            // archive
                                            Alldocument.Add(archive(table, Id, sub.subdept, dates, type));
                                        }
                                        PdfPTable tableLegend = new PdfPTable(24);
                                        Alldocument.Add(legend(tableLegend));
                                        Alldocument.Close();
                                        output.Close();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: 
        // Parametros: 
        //          table        -> 
        //----------------------------------------------------------------------------------------------------------------
        public PdfPTable legend(PdfPTable table)
        {
            /*Agregando leyenda a la tabla. */
            PdfPCell cell2 = new PdfPCell();

            cell2 = new PdfPCell(new Phrase(""));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
            cell2.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);
            cell2.Colspan = 24;
            table.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase(""));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
            cell2.BorderColor = new iTextSharp.text.BaseColor(255, 255, 255);
            cell2.Colspan = 24;
            table.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase("Color Key"));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
            cell2.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell2.Colspan = 24;
            table.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase("Remote Authentication"));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
            cell2.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell2.Colspan = 12;
            table.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase(""));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
            cell2.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell2.Colspan = 12;
            table.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase("OnSite Authentication"));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
            cell2.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell2.Colspan = 12;
            table.AddCell(cell2);

            cell2 = new PdfPCell(new Phrase(""));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
            cell2.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell2.Colspan = 12;
            table.AddCell(cell2);

            return table;
        }

        //----------------------------------------------------------------------------------------------------------------
        // Funcion: 
        // Parametros: 
        //          table       -> 
        //          deptID      -> 
        //          subdept     -> 
        //          dates       -> 
        //          type        -> 
        //----------------------------------------------------------------------------------------------------------------
        public PdfPTable archive(PdfPTable table, int deptID, string subdept, string dates, bool type)
        {
            List<InfoReport> all = new List<InfoReport>();
            List<InfoReport> listWeek1 = new List<InfoReport>();
            List<InfoReport> listWeek2 = new List<InfoReport>();
            List<InfoReport> listWeek3 = new List<InfoReport>();
            List<InfoReport> listWeek4 = new List<InfoReport>();
            List<InfoReport> potato = new List<InfoReport>();
            PdfPCell cell = new PdfPCell();
            PdfPCell cell2 = new PdfPCell();
            PdfPCell cellName = new PdfPCell();
            PdfPCell cellMonday = new PdfPCell();
            PdfPCell cellTuesday = new PdfPCell();
            PdfPCell cellWednesday = new PdfPCell();
            PdfPCell cellThursday = new PdfPCell();
            PdfPCell cellfriday = new PdfPCell();
            PdfPCell cellsaturday = new PdfPCell();
            PdfPCell cellsunday = new PdfPCell();
            Array[] list = { };
            iTextSharp.text.Font georgia = FontFactory.GetFont("georgia", 14f);
            georgia.Color = iTextSharp.text.BaseColor.BLUE;
            iTextSharp.text.Font labels = FontFactory.GetFont("georgia", 14f);
            labels.Color = iTextSharp.text.BaseColor.WHITE;
            iTextSharp.text.Font Headers = FontFactory.GetFont("georgia", 18f);
            Headers.Color = iTextSharp.text.BaseColor.BLACK;

            PdfPTable table2 = new PdfPTable(24);
            table.TotalWidth = 5000f;

            float[] widths = new float[] { 1300f, 1300, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f };
            table.SetWidths(widths);
            DateTime week2 = Convert.ToDateTime(dates).AddDays(-7);
            DateTime week3 = Convert.ToDateTime(dates).AddDays(-14);
            DateTime week4 = Convert.ToDateTime(dates).AddDays(-21);
       
            using (Anviz_Data_BaseEntities dc = new Anviz_Data_BaseEntities())
            {
                if (type == true)
                {
                    all = (from e in dc.sp_matrix_weekdays(deptID, dates)
                           select new InfoReport
                           {
                               Nombre = e.name,
                               FechaEntradaMonday = e.Hora_de_LLegada_Lunes,
                               SensorInMonday = e.Lugar_de_Marcaje_Llegada_Lunes,
                               FechaSalidaMonday = e.Hora_de_salida_Lunes,
                               SensorOutMonday = e.Lugar_de_Marcaje_Salida_Lunes,
                               HorasMonday = e.Monday,
                               FechaEntradaTuesday = e.Hora_de_LLegada_Martes,
                               SensorInTuesday = e.Lugar_de_Marcaje_Llegada_Martes,
                               FechaSalidaTuesday = e.Hora_de_salida_Martes,
                               SensorOutTuesday = e.Lugar_de_Marcaje_Salida_Martes,
                               HorasTuesday = e.Tuesday,
                               FechaEntradaWendnsday = e.Hora_de_LLegada_Miercoles,
                               SensorInWednesday = e.Lugar_de_Marcaje_Llegada_Miercoles,
                               FechaSalidaWendnsday = e.Hora_de_salida_Miercoles,
                               SensorOutWednesday = e.Lugar_de_Marcaje_Salida_Miercoles,
                               HorasWendnsday = e.Wednsday,
                               FechaEntradaThursday = e.Hora_de_LLegada_Jueves,
                               SensorInThursday = e.Lugar_de_Marcaje_Llegada_Jueves,
                               FechaSalidaThursday = e.Hora_de_salida_Jueves,
                               SensorOutThursday = e.Lugar_de_Marcaje_Salida_Jueves,
                               HorasThursday = e.Thursday,
                               FechaEntradaFriday = e.Hora_de_LLegada_Viernes,
                               SensorInFriday = e.Lugar_de_Marcaje_Llegada_Viernes,
                               FechaSalidaFriday = e.Hora_de_salida_Viernes,
                               SensorOutFriday = e.Lugar_de_Marcaje_Salida_Viernes,
                               HorasFriday = e.Friday,
                               FechaEntradaSaturday = e.Hora_de_LLegada_Sabado,
                               SensorInSaturday = e.Lugar_de_Marcaje_Llegada_Sabado,
                               FechaSalidaSaturday = e.Hora_de_salida_Sabado,
                               SensorOutSaturday = e.Lugar_de_Marcaje_Salida_Sabado,
                               HorasSaturday = e.Saturday,
                               FechaEntradaSunday = e.Hora_de_LLegada_Domingo,
                               SensorInSunday = e.Lugar_de_Marcaje_Llegada_Domingo,
                               FechaSalidaSunday = e.Hora_de_salida_Domingo,
                               SensorOutSunday = e.Lugar_de_Marcaje_Salida_Domingo,
                               HorasSunday = e.Sunday,
                           }).ToList();
                }
                else
                {
                    all = (from e in dc.GetEmployes(deptID, dates)
                           select new InfoReport
                           {
                               Nombre = e.name,
                               FechaEntradaMonday = e.Hora_de_LLegada_Lunes,
                               SensorInMonday = e.Lugar_de_Marcaje_Llegada_Lunes,
                               FechaSalidaMonday = e.Hora_de_salida_Lunes,
                               SensorOutMonday = e.Lugar_de_Marcaje_Salida_Lunes,
                               HorasMonday = e.Monday,

                               FechaEntradaTuesday = e.Hora_de_LLegada_Martes,
                               SensorInTuesday = e.Lugar_de_Marcaje_Llegada_Martes,
                               FechaSalidaTuesday = e.Hora_de_salida_Martes,
                               SensorOutTuesday = e.Lugar_de_Marcaje_Salida_Martes,
                               HorasTuesday = e.Tuesday,

                               FechaEntradaWendnsday = e.Hora_de_LLegada_Miercoles,
                               SensorInWednesday = e.Lugar_de_Marcaje_Llegada_Miercoles,
                               FechaSalidaWendnsday = e.Hora_de_salida_Miercoles,
                               SensorOutWednesday = e.Lugar_de_Marcaje_Salida_Miercoles,
                               HorasWendnsday = e.Wednsday,

                               FechaEntradaThursday = e.Hora_de_LLegada_Jueves,
                               SensorInThursday = e.Lugar_de_Marcaje_Llegada_Jueves,
                               FechaSalidaThursday = e.Hora_de_salida_Jueves,
                               SensorOutThursday = e.Lugar_de_Marcaje_Salida_Jueves,
                               HorasThursday = e.Thursday,

                               FechaEntradaFriday = e.Hora_de_LLegada_Viernes,
                               SensorInFriday = e.Lugar_de_Marcaje_Llegada_Viernes,
                               FechaSalidaFriday = e.Hora_de_salida_Viernes,
                               SensorOutFriday = e.Lugar_de_Marcaje_Salida_Viernes,
                               HorasFriday = e.Friday,

                               FechaEntradaSaturday = e.Hora_de_LLegada_Sabado,
                               SensorInSaturday = e.Lugar_de_Marcaje_Llegada_Sabado,
                               FechaSalidaSaturday = e.Hora_de_salida_Sabado,
                               SensorOutSaturday = e.Lugar_de_Marcaje_Salida_Sabado,
                               HorasSaturday = e.Saturday,

                               FechaEntradaSunday = e.Hora_de_LLegada_Domingo,
                               SensorInSunday = e.Lugar_de_Marcaje_Llegada_Domingo,
                               FechaSalidaSunday = e.Hora_de_salida_Domingo,
                               SensorOutSunday = e.Lugar_de_Marcaje_Salida_Domingo,
                               HorasSunday = e.Sunday,
                           }).ToList();
                }
            }

            DateTime date = Convert.ToDateTime(dates);

            cell2 = new PdfPCell(new Phrase(subdept, Headers));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(245, 92, 24);
            cell2.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell2.Colspan = 24;
            table.AddCell(cell2);

            PdfPCell nombre = new PdfPCell(new Phrase("Name", labels));
            nombre.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
            nombre.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            nombre.BorderWidthRight = 1;
            nombre.Rowspan = 2;
            nombre.Colspan = 2;
            nombre.HorizontalAlignment = 1;
            nombre.VerticalAlignment = 1;
            table.AddCell(nombre);

            PdfPCell june = new PdfPCell(new Phrase("Monday  " + "\n" + Convert.ToDateTime(date).ToShortDateString().ToString(), Headers));
            june.BorderWidthRight = 1;
            june.HorizontalAlignment = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.Colspan = 3;
            table.AddCell(june);
            june = new PdfPCell(new Phrase("Tuesday  " + "\n" + Convert.ToDateTime(date).AddDays(1).ToShortDateString(), Headers));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);
            june = new PdfPCell(new Phrase("Wednesday  " + "\n" + Convert.ToDateTime(date).AddDays(2).ToShortDateString(), Headers));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);
            june = new PdfPCell(new Phrase("Thursday  " + "\n" + Convert.ToDateTime(date).AddDays(3).ToShortDateString(), Headers));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Friday   " + "\n" + Convert.ToDateTime(date).AddDays(4).ToShortDateString(), Headers));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Saturday  " + "\n" + Convert.ToDateTime(date).AddDays(5).ToShortDateString(), Headers));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);
            june = new PdfPCell(new Phrase("Sunday   " + "\n" + Convert.ToDateTime(date).AddDays(6).ToShortDateString(), Headers));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Weekly Hours Worked", labels));
            june.BorderWidthRight = 1;
            june.Rowspan = 2;
            june.HorizontalAlignment = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            table.AddCell(june);

            PdfPCell In = new PdfPCell();
            PdfPCell OUT = new PdfPCell();
            PdfPCell second = new PdfPCell();

            for (var y = 0; y <= 6; y++)
            {
                In = new PdfPCell(new Phrase("In", labels));

                In.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
                In.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                In.HorizontalAlignment = 1;
                table.AddCell(In);

                OUT = new PdfPCell(new Phrase("Out", labels));
                OUT.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
                OUT.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                OUT.HorizontalAlignment = 1;
                table.AddCell(OUT);

                second = new PdfPCell(new Phrase("Worked Hours", labels));
                second.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
                second.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                second.BorderWidthRight = 1;
                second.HorizontalAlignment = 1;
                table.AddCell(second);
            }

            foreach (var item in all)
            {
                cellName.Phrase = new Phrase(item.Nombre);
                cellName.HorizontalAlignment = 1;
                cellName.Colspan = 2;
                cellName.BorderWidthRight = 1;
                table.AddCell(cellName);

                //For Monday

                if (item.SensorInMonday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (item.SensorOutMonday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellMonday = new PdfPCell(new Phrase((item.HorasMonday).ToString()));
                cellMonday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellMonday.BorderWidthRight = 1;
                cellMonday.HorizontalAlignment = 1;
                table.AddCell(cellMonday);

                //For Tuesday

                if (item.SensorInTuesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (item.SensorOutTuesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellTuesday = new PdfPCell(new Phrase((item.HorasTuesday).ToString()));
                cellTuesday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellTuesday.BorderWidthRight = 1;
                cellTuesday.HorizontalAlignment = 1;
                table.AddCell(cellTuesday);

                //For Wednesday

                if (item.SensorInWednesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (item.SensorOutWednesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellWednesday = new PdfPCell(new Phrase((item.HorasWendnsday).ToString()));
                cellWednesday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellWednesday.BorderWidthRight = 1;
                cellWednesday.HorizontalAlignment = 1;
                table.AddCell(cellWednesday);

                //For Thursday
                if (item.SensorInThursday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (item.SensorOutThursday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellThursday = new PdfPCell(new Phrase((item.HorasThursday).ToString()));
                cellThursday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellThursday.BorderWidthRight = 1;
                cellThursday.HorizontalAlignment = 1;
                table.AddCell(cellThursday);

                //For Friday

                if (item.SensorInFriday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (item.SensorOutFriday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellfriday = new PdfPCell(new Phrase((item.HorasFriday).ToString()));
                cellfriday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellfriday.BorderWidthRight = 1;
                cellfriday.HorizontalAlignment = 1;
                table.AddCell(cellfriday);

                //For Saturday

                if (item.SensorInSaturday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (item.SensorOutSaturday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellsaturday = new PdfPCell(new Phrase((item.HorasSaturday).ToString()));
                cellsaturday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellsaturday.BorderWidthRight = 1;
                cellsaturday.HorizontalAlignment = 1;
                table.AddCell(cellsaturday);

                //For Sunday

                if (item.SensorInSunday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaSunday).ToShortTimeString().ToString());

                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaEntradaSunday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                }

                if (item.SensorOutSunday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaSunday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(item.FechaSalidaSunday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }


                cellsunday = new PdfPCell(new Phrase((item.HorasSunday).ToString()));
                cellsunday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellsunday.BorderWidthRight = 1;
                cellsunday.HorizontalAlignment = 1;
                table.AddCell(cellsunday);

                cell.Phrase = new Phrase(Convert.ToString(Convert.ToDecimal(item.HorasMonday) + Convert.ToDecimal(item.HorasTuesday) + Convert.ToDecimal(item.HorasWendnsday) + Convert.ToDecimal(item.HorasThursday) + Convert.ToDecimal(item.HorasFriday) + Convert.ToDecimal(item.HorasSaturday) + Convert.ToDecimal(item.HorasSunday)));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
            }
            return table;
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: 
        // Parametros: 
        //          table       -> 
        //          deptID      -> 
        //          subdept     -> 
        //          dates       -> 
        //          type        -> 
        //----------------------------------------------------------------------------------------------------------------
        public PdfPTable ArchiveSpin4(PdfPTable table, int deptID, string subdept, string dates, bool type)
        {
            List<InfoReport> all = new List<InfoReport>();
            PdfPCell cell = new PdfPCell();
            PdfPCell cell2 = new PdfPCell();
            PdfPCell cellName = new PdfPCell();
            PdfPCell cellMonday = new PdfPCell();
            PdfPCell cellTuesday = new PdfPCell();
            PdfPCell cellWednesday = new PdfPCell();
            PdfPCell cellThursday = new PdfPCell();
            PdfPCell cellfriday = new PdfPCell();
            PdfPCell cellsaturday = new PdfPCell();
            PdfPCell cellsunday = new PdfPCell();
            Array[] list = { };

            iTextSharp.text.Font georgia = FontFactory.GetFont("georgia", 14f);
            georgia.Color = iTextSharp.text.BaseColor.BLUE;
            iTextSharp.text.Font labels = FontFactory.GetFont("georgia", 14f);
            labels.Color = iTextSharp.text.BaseColor.WHITE;
            iTextSharp.text.Font Headers = FontFactory.GetFont("georgia", 18f);
            Headers.Color = iTextSharp.text.BaseColor.BLACK;
            List<InfoReport> concat = new List<InfoReport>();

            PdfPTable table2 = new PdfPTable(24);
            table.TotalWidth = 5000f;

            float[] widths = new float[] { 1300f, 1300, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f };
            table.SetWidths(widths);
            DateTime week2 = Convert.ToDateTime(dates).AddDays(-7);
            DateTime week3 = Convert.ToDateTime(dates).AddDays(-14);
            DateTime week4 = Convert.ToDateTime(dates).AddDays(-21);
            DateTime week1 = Convert.ToDateTime(dates);
            List<InfoReport> listWeek1 = new List<InfoReport>();
            List<InfoReport> listWeek2 = new List<InfoReport>();
            List<InfoReport> listWeek3 = new List<InfoReport>();
            List<InfoReport> listWeek4 = new List<InfoReport>();
            List<List<InfoReport>> allWeeks = new List<List<InfoReport>>();
            List<InfoReport> potato = new List<InfoReport>();

            int count = -7;
            
            /*
            for (int i = 0; i < 3; i++)
            {
            */
                using (Anviz_Data_BaseEntities dc = new Anviz_Data_BaseEntities())
                {
                    if (type == true)
                    {
                        listWeek1 = (from e in dc.sp_matrix_weekdays(deptID, (week1.ToString("MM-dd-yyyy"))) select new InfoReport { Nombre = e.name, FechaEntradaMonday = e.Hora_de_LLegada_Lunes, FechaSalidaMonday = e.Hora_de_salida_Lunes, HorasMonday = e.Monday, FechaEntradaTuesday = e.Hora_de_LLegada_Martes, FechaSalidaTuesday = e.Hora_de_salida_Martes, HorasTuesday = e.Tuesday, FechaEntradaWendnsday = e.Hora_de_LLegada_Miercoles, FechaSalidaWendnsday = e.Hora_de_salida_Miercoles, HorasWendnsday = e.Wednsday, FechaEntradaThursday = e.Hora_de_LLegada_Jueves, FechaSalidaThursday = e.Hora_de_salida_Jueves, HorasThursday = e.Thursday, FechaEntradaFriday = e.Hora_de_LLegada_Viernes, FechaSalidaFriday = e.Hora_de_salida_Viernes, HorasFriday = e.Friday, FechaEntradaSaturday = e.Hora_de_LLegada_Sabado, FechaSalidaSaturday = e.Hora_de_salida_Sabado, HorasSaturday = e.Saturday, FechaEntradaSunday = e.Hora_de_LLegada_Domingo, FechaSalidaSunday = e.Hora_de_salida_Domingo, HorasSunday = e.Sunday }).ToList();
                        listWeek2 = (from e in dc.sp_matrix_weekdays(deptID, (week2.ToString("MM-dd-yyyy"))) select new InfoReport { Nombre = e.name, FechaEntradaMonday = e.Hora_de_LLegada_Lunes, FechaSalidaMonday = e.Hora_de_salida_Lunes, HorasMonday = e.Monday, FechaEntradaTuesday = e.Hora_de_LLegada_Martes, FechaSalidaTuesday = e.Hora_de_salida_Martes, HorasTuesday = e.Tuesday, FechaEntradaWendnsday = e.Hora_de_LLegada_Miercoles, FechaSalidaWendnsday = e.Hora_de_salida_Miercoles, HorasWendnsday = e.Wednsday, FechaEntradaThursday = e.Hora_de_LLegada_Jueves, FechaSalidaThursday = e.Hora_de_salida_Jueves, HorasThursday = e.Thursday, FechaEntradaFriday = e.Hora_de_LLegada_Viernes, FechaSalidaFriday = e.Hora_de_salida_Viernes, HorasFriday = e.Friday, FechaEntradaSaturday = e.Hora_de_LLegada_Sabado, FechaSalidaSaturday = e.Hora_de_salida_Sabado, HorasSaturday = e.Saturday, FechaEntradaSunday = e.Hora_de_LLegada_Domingo, FechaSalidaSunday = e.Hora_de_salida_Domingo, HorasSunday = e.Sunday }).ToList();
                        listWeek3 = (from e in dc.sp_matrix_weekdays(deptID, (week3.ToString("MM-dd-yyyy"))) select new InfoReport { Nombre = e.name, FechaEntradaMonday = e.Hora_de_LLegada_Lunes, FechaSalidaMonday = e.Hora_de_salida_Lunes, HorasMonday = e.Monday, FechaEntradaTuesday = e.Hora_de_LLegada_Martes, FechaSalidaTuesday = e.Hora_de_salida_Martes, HorasTuesday = e.Tuesday, FechaEntradaWendnsday = e.Hora_de_LLegada_Miercoles, FechaSalidaWendnsday = e.Hora_de_salida_Miercoles, HorasWendnsday = e.Wednsday, FechaEntradaThursday = e.Hora_de_LLegada_Jueves, FechaSalidaThursday = e.Hora_de_salida_Jueves, HorasThursday = e.Thursday, FechaEntradaFriday = e.Hora_de_LLegada_Viernes, FechaSalidaFriday = e.Hora_de_salida_Viernes, HorasFriday = e.Friday, FechaEntradaSaturday = e.Hora_de_LLegada_Sabado, FechaSalidaSaturday = e.Hora_de_salida_Sabado, HorasSaturday = e.Saturday, FechaEntradaSunday = e.Hora_de_LLegada_Domingo, FechaSalidaSunday = e.Hora_de_salida_Domingo, HorasSunday = e.Sunday }).ToList();
                        listWeek4 = (from e in dc.sp_matrix_weekdays(deptID, (week4.ToString("MM-dd-yyyy"))) select new InfoReport { Nombre = e.name, FechaEntradaMonday = e.Hora_de_LLegada_Lunes, FechaSalidaMonday = e.Hora_de_salida_Lunes, HorasMonday = e.Monday, FechaEntradaTuesday = e.Hora_de_LLegada_Martes, FechaSalidaTuesday = e.Hora_de_salida_Martes, HorasTuesday = e.Tuesday, FechaEntradaWendnsday = e.Hora_de_LLegada_Miercoles, FechaSalidaWendnsday = e.Hora_de_salida_Miercoles, HorasWendnsday = e.Wednsday, FechaEntradaThursday = e.Hora_de_LLegada_Jueves, FechaSalidaThursday = e.Hora_de_salida_Jueves, HorasThursday = e.Thursday, FechaEntradaFriday = e.Hora_de_LLegada_Viernes, FechaSalidaFriday = e.Hora_de_salida_Viernes, HorasFriday = e.Friday, FechaEntradaSaturday = e.Hora_de_LLegada_Sabado, FechaSalidaSaturday = e.Hora_de_salida_Sabado, HorasSaturday = e.Saturday, FechaEntradaSunday = e.Hora_de_LLegada_Domingo, FechaSalidaSunday = e.Hora_de_salida_Domingo, HorasSunday = e.Sunday }).ToList();

                        allWeeks.Add(listWeek1);
                        allWeeks.Add(listWeek2);
                        allWeeks.Add(listWeek3);
                        allWeeks.Add(listWeek4);

                        var y = week2.Year;
                        var Lr3 = listWeek1.Concat(listWeek2).Concat(listWeek3).Concat(listWeek4).ToList();

                        if (y % 4 == 0 && (y % 100 != 0 || y % 400 == 0) && DateTime.Today.Month==3)
                        {
                            DateTime week5 = Convert.ToDateTime(dates).AddDays(-28);
                            List<InfoReport> listWeek5 = (from e in dc.sp_matrix_weekdays(deptID, (week5.ToString("MM-dd-yyyy"))) select new InfoReport { Nombre = e.name, FechaEntradaMonday = e.Hora_de_LLegada_Lunes, FechaSalidaMonday = e.Hora_de_salida_Lunes, HorasMonday = e.Monday, FechaEntradaTuesday = e.Hora_de_LLegada_Martes, FechaSalidaTuesday = e.Hora_de_salida_Martes, HorasTuesday = e.Tuesday, FechaEntradaWendnsday = e.Hora_de_LLegada_Miercoles, FechaSalidaWendnsday = e.Hora_de_salida_Miercoles, HorasWendnsday = e.Wednsday, FechaEntradaThursday = e.Hora_de_LLegada_Jueves, FechaSalidaThursday = e.Hora_de_salida_Jueves, HorasThursday = e.Thursday, FechaEntradaFriday = e.Hora_de_LLegada_Viernes, FechaSalidaFriday = e.Hora_de_salida_Viernes, HorasFriday = e.Friday, FechaEntradaSaturday = e.Hora_de_LLegada_Sabado, FechaSalidaSaturday = e.Hora_de_salida_Sabado, HorasSaturday = e.Saturday, FechaEntradaSunday = e.Hora_de_LLegada_Domingo, FechaSalidaSunday = e.Hora_de_salida_Domingo, HorasSunday = e.Sunday }).ToList();

                            allWeeks.Add(listWeek5);

                            Lr3 = Lr3.Concat(listWeek5).ToList();
                        }

                        foreach (var row in Lr3)
                        {
                            int index = potato.FindIndex(a => a.Nombre == row.Nombre);
                            if (!(index >= 0))
                            {
                                potato.Add(row);

                            }
                            else
                            {
                                var tomato = potato[index];
                                decimal? tempHoursi = (row.HorasMonday.Equals(null) ? 0:row.HorasMonday);
                                decimal? tempHoursi2 =(row.HorasTuesday.Equals(null) ? 0:row.HorasTuesday) ;
                                decimal? tempHoursi3 = (row.HorasWendnsday.Equals(null) ? 0 : row.HorasWendnsday);
                                decimal? tempHoursi4 = (row.HorasThursday.Equals(null) ? 0:row.HorasThursday);
                                decimal? tempHoursi5 = (row.HorasFriday.Equals(null) ? 0:row.HorasFriday);
                                decimal? tempHoursi6 = (row.HorasSaturday.Equals(null) ? 0:row.HorasSaturday);
                                decimal? tempHoursi7 = (row.HorasSunday.Equals(null) ? 0:row.HorasSunday);


                                decimal? tempHours2 = (tomato.HorasMonday.Equals(null) ?0:tomato.HorasMonday) + tempHoursi;
                                decimal? tempHours3 = (tomato.HorasTuesday.Equals(null) ?0:tomato.HorasTuesday)+ tempHoursi2;
                                decimal? tempHours4 = (tomato.HorasWendnsday.Equals(null) ?0:tomato.HorasWendnsday) + tempHoursi3;
                                decimal? tempHours5 = (tomato.HorasThursday.Equals(null) ?0:tomato.HorasThursday) + tempHoursi4;
                                decimal? tempHours6 = (tomato.HorasFriday.Equals(null) ?0:tomato.HorasFriday) + tempHoursi5;
                                decimal? tempHours7 = (tomato.HorasSaturday.Equals(null) ?0:tomato.HorasSaturday) + tempHoursi6;
                                decimal? tempHours8 = (tomato.HorasSunday.Equals(null) ?0:tomato.HorasSunday) + tempHoursi7;


                                tomato.HorasMonday = tempHours2;
                                tomato.HorasTuesday = tempHours3;
                                tomato.HorasWendnsday = tempHours4;
                                tomato.HorasThursday = tempHours5;
                                tomato.HorasFriday = tempHours6;
                                tomato.HorasSaturday = tempHours7;
                                tomato.HorasSunday = tempHours8;

                                potato[index] = tomato;
                            }
                        }
                        all = potato;
                    }
                }
                DateTime datee = Convert.ToDateTime(dates);
                DateTime back = Convert.ToDateTime(datee.AddDays(-count));
                count = count - 7;
                dates = back.ToString();
            // }

            DateTime date = Convert.ToDateTime(dates);

            cell2 = new PdfPCell(new Phrase(subdept, Headers));
            cell2.HorizontalAlignment = Element.ALIGN_CENTER;
            cell2.BackgroundColor = new iTextSharp.text.BaseColor(245, 92, 24);
            cell2.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cell2.Colspan = 24;
            table.AddCell(cell2);

            PdfPCell nombre = new PdfPCell(new Phrase("Name", labels));
            nombre.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
            nombre.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            nombre.BorderWidthRight = 1;
            nombre.Rowspan = 2;
            nombre.Colspan = 2;
            nombre.HorizontalAlignment = 1;
            nombre.VerticalAlignment = 1;
            table.AddCell(nombre);

            PdfPCell june = new PdfPCell(new Phrase("Monday  " + "\n"));
            june.BorderWidthRight = 1;
            june.HorizontalAlignment = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Tuesday  " + "\n" ));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Wednesday  " + "\n"  ));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Thursday  " + "\n" ));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Friday   " + "\n" ));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Saturday  " + "\n" ));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Sunday   " + "\n" ));
            june.BorderWidthRight = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            june.HorizontalAlignment = 1;
            june.Colspan = 3;
            table.AddCell(june);

            june = new PdfPCell(new Phrase("Monthly Hours Worked", labels));
            june.BorderWidthRight = 1;
            june.Rowspan = 2;
            june.HorizontalAlignment = 1;
            june.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
            june.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            table.AddCell(june);

            PdfPCell In = new PdfPCell();
            PdfPCell OUT = new PdfPCell();
            PdfPCell second = new PdfPCell();

            for (var y = 0; y <= 6; y++)
            {
                second = new PdfPCell(new Phrase("Worked Hours", labels));
                second.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
                second.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                second.BorderWidthRight = 1;
                second.HorizontalAlignment = 1;
                second.Colspan = 3;
                table.AddCell(second);
            }
        
            foreach (var item in all)
            {
                cellName.Phrase = new Phrase(item.Nombre);
                cellName.HorizontalAlignment = 1;
                cellName.Colspan = 2;
                cellName.BorderWidthRight = 1;
                table.AddCell(cellName);

                cellMonday = new PdfPCell(new Phrase((item.HorasMonday).ToString()));
                cellMonday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellMonday.BorderWidthRight = 1;
                cellMonday.HorizontalAlignment = 1;
                cellMonday.Colspan = 3;
                table.AddCell(cellMonday);

                cellTuesday = new PdfPCell(new Phrase((item.HorasTuesday).ToString()));
                cellTuesday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellTuesday.BorderWidthRight = 1;
                cellTuesday.HorizontalAlignment = 1;
                cellTuesday.Colspan = 3;
                table.AddCell(cellTuesday);

                cellWednesday = new PdfPCell(new Phrase((item.HorasWendnsday).ToString()));
                cellWednesday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellWednesday.BorderWidthRight = 1;
                cellWednesday.HorizontalAlignment = 1;
                cellWednesday.Colspan = 3;
                table.AddCell(cellWednesday);

                cellThursday = new PdfPCell(new Phrase((item.HorasThursday).ToString()));
                cellThursday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellThursday.BorderWidthRight = 1;
                cellThursday.HorizontalAlignment = 1;
                cellThursday.Colspan = 3;
                table.AddCell(cellThursday);

                cellfriday = new PdfPCell(new Phrase((item.HorasFriday).ToString()));
                cellfriday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellfriday.BorderWidthRight = 1;
                cellfriday.HorizontalAlignment = 1;
                cellfriday.Colspan = 3;
                table.AddCell(cellfriday);

                cellsaturday = new PdfPCell(new Phrase((item.HorasSaturday).ToString()));
                cellsaturday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellsaturday.BorderWidthRight = 1;
                cellsaturday.HorizontalAlignment = 1;
                cellsaturday.Colspan = 3;
                table.AddCell(cellsaturday);

                cellsunday = new PdfPCell(new Phrase((item.HorasSunday).ToString()));
                cellsunday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellsunday.BorderWidthRight = 1;
                cellsunday.HorizontalAlignment = 1;
                cellsunday.Colspan = 3;
                table.AddCell(cellsunday);

                cell.Phrase = new Phrase(Convert.ToString(Convert.ToDecimal(item.HorasMonday) + Convert.ToDecimal(item.HorasTuesday) + Convert.ToDecimal(item.HorasWendnsday) + Convert.ToDecimal(item.HorasThursday) + Convert.ToDecimal(item.HorasFriday) + Convert.ToDecimal(item.HorasSaturday) + Convert.ToDecimal(item.HorasSunday)));
                cell.HorizontalAlignment = 1;

                table.AddCell(cell);
            }
            return table;
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: enviar correo electronico con reporte de asistencia
        // Parametros: 
        //          monday      -> fecha de inicio del reporte.
        //          name        -> nombre de la persona a quien va dirigido el reporte.
        //          dept_name   -> nombre del reporte (puede ser el nombre del empleado, del equipo o del empleado).
        //          to          -> correo electronico al que se enviara el reporte.
        //          path        -> carpeta donde se encuentra el reporte a enviar (\\SubDepts\\, \\Depts\\, \\Employees\\).
        //          typeReport  -> tipo de reporte que se esta enviando (para supervisar el envio):
        //                          0   ->  especial (Gina, Flor de Liz Reyes)
        //                          1   ->  departamentos
        //                          2   ->  equipos
        //                          3   ->  empleados
        //          id          -> identificador del departamento equipo o empleado (para supervisar el envio).
        //----------------------------------------------------------------------------------------------------------------
        public void sendEmail(string monday, string name, string dept_name, string to, string path, int typeReport, int id)
        {
            if (to != null)
            {
                // to = "test.reports.anviz@gmail.com";

                string subject = "Sending " + name + " email with attendance report  " + dept_name;

                string mailbody = "";

                if (name == dept_name)
                {
                    mailbody = "Dear Colleague,<br><br>" +
                                "This weekly attendance report shows your daily attendance and the office hours you have logged during the past week according to the fingerprint scanner and/or your remote time log.<br><br>" +
                                "It is important for us to provide this information so you can keep track and control your attendance and office hours.<br><br>" +
                                "Best Regards,<br><br>" +
                                "HR Team";
                }
                else
                {
                    string time = "";
                    if (path == "\\SubDepts\\")
                    {
                        time = "week";
                    }
                    else
                    {
                        DateTime now = DateTime.Now;
                        var oneweekAffter = now.AddDays(7);
                        if (oneweekAffter.Month != now.Month)
                        {
                            time = "month";
                        }
                        else
                        {
                            time = "week";
                        }
                    }

                    mailbody = "Dear Colleague,<br><br>" +
                                "This weekly attendance report shows your team’s daily attendance and the office hours they have logged during the past " + time + " according to the fingerprint scanner and/or their remote time log.<br><br>" +
                                "It is important for us to provide this information so you can keep track and control your team’s attendance and office hours.<br><br>" +
                                "If you do not wish to receive this report or would like someone else from your team to receive it, please contact Alana García, LNO Honduras HR Administrator, at alana.garcia@laureate.net.<br><br>" +
                                "Best Regards,<br><br>" +
                                "HR Team";
                }

                try
                {
                    Attachment att = new Attachment(@"C:\\Pdf\\Asistance_Report_" + monday + path + "Attendance_Report_" + dept_name + "_" + monday + ".pdf");

                    MailMessage mail = new MailMessage("no-reply@laureate.net", to, subject, mailbody);
                    mail.IsBodyHtml = true;
                    mail.SubjectEncoding = Encoding.Default;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.Attachments.Add(att);

                    SmtpClient client = new SmtpClient();

                    client.Host = "smtp.mandrillapp.com";
                    client.Port = int.Parse("587");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential("raul.rivera@laureate.net", "uuQ-6gYsZG5Yzz7Hv-a3yA");

                    try
                    {
                        Console.WriteLine("SendingEmailis " + dept_name + " for " + name + " ..................................");
                        client.Send(mail);

                        // Reporte enviado correctamente.
                        // Agregando detalle del reporte.
                        reportsDetails.Add(new ReportDetails(typeReport, id, 1, 0));
                    }
                    catch (Exception ex)
                    {
                        // Reporte no enviado.
                        // Agregando detalle del reporte.
                        reportsDetails.Add(new ReportDetails(typeReport, id, 0, 2));
                        reportState.TypeError = 2;

                        Console.WriteLine("SendingEmailisError " + ex + "!!! with " + to + "..............................");
                        throw ex;
                    }
                }
                catch
                {
                }
            }
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: almacena en la base de datos SQL el registro de estado de envio de reporte semanal.
        // Parametros: no recibe ningun parametro.
        //----------------------------------------------------------------------------------------------------------------
        public void storeStateIni()
        {
            Anviz_Data_BaseEntities dc = new Anviz_Data_BaseEntities();

            // Calculando fechas de inicio y finalización del reporte.
            DateTime lastMonday = new DateTime();
            DateTime now = DateTime.Now;
            var oneweekAffter = now.AddDays(7);
            DateTime day = DateTime.Today;
            int offset = day.DayOfWeek - DayOfWeek.Monday;
            lastMonday = Convert.ToDateTime(day.AddDays(-offset - 7));

            // Almacenando información en la base de datos.
            this.idRep = (from rs in dc.SP_InsertReportState(reportState.StateReport, reportState.PercentReports, lastMonday, lastMonday.AddDays(6), reportState.TypeError) select Convert.ToInt32(rs.Value)).ToList()[0];
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: almacenar en la base de datos SQL el estado de envio del reporte semanal y en caso de ser necesario
        //          los detalles del envio.
        // Parametros: No recibe ningun parametro.
        //----------------------------------------------------------------------------------------------------------------
        private void storeStateReport()
        {
            Anviz_Data_BaseEntities dc = new Anviz_Data_BaseEntities();

            // Estado de envio,
            int sent = 0;

            // Se generaron uno o mas reportes.
            if (reportsDetails.Count >= 1)
            {
                // Verificando la cantidad de reportes enviados y calculando el porcentaje del total.
                int send = 0;
                reportsDetails.OrderBy(param => param.TypeReport).ToList();
                foreach (ReportDetails rd in reportsDetails)
                {
                    if (rd.StateReport == 1)
                    {
                        send = send + 1;
                    }
                }
                reportState.PercentReports = Convert.ToDecimal((Convert.ToDouble(send) / Convert.ToDouble(reportsDetails.Count)) * 100);

                // No se envio nada y no se envio todo.
                // Se envio un porcentaje 'p' de reportes (0<p<100).
                if (reportState.PercentReports != 0 && reportState.PercentReports != 100)
                {
                    // Estado de envio parcialmente enviado.
                    sent = 2;

                    // Estableciendo estado parcialmente enviado.
                    reportState.StateReport = 2;

                    // Nuevo reporte.
                    if (this.newRep)
                    {
                        // Insertando ReportsDetail en base de datos.
                        foreach (ReportDetails rd in reportsDetails)
                        {
                            dc.SP_InsertReportDetail(rd.TypeReport, rd.IdReceptor, rd.StateReport, rd.TypeError, this.idRep);
                        }
                    }
                    // Reenviando reporte.
                    else
                    {
                        // Actualizando ReportsDetail en base de datos.
                        foreach (ReportDetails rd in reportsDetails)
                        {
                            dc.SP_UpdateReportDetail(rd.TypeReport, rd.StateReport, rd.TypeError, this.idRep, rd.IdReceptor);
                        }
                    }

                    // Actualizando ReportState en base de datos.
                    dc.SP_UpdateStateReport(this.idRep, reportState.StateReport, reportState.PercentReports, reportState.StartDate, reportState.EndDate, reportState.TypeError);
                }
                // Se envio nada o se envio todo.
                else
                {
                    // Se envio todo.
                    if (reportState.PercentReports == 100)
                    {
                        // Estado de envio enviado.
                        sent = 1;

                        // Estableciendo estado enviado.
                        reportState.StateReport = 1;
                    }
                    // Se envio nada.
                    else
                    {
                        // Estableciendo estado no enviado.
                        reportState.StateReport = 0;
                    }

                    // Si es un nuevo reporte o el estado de envio es diferente de 'no enviado'.
                    if (this.newRep || reportState.StateReport != 0)
                    {
                        // Actualizando ReportState.
                        dc.SP_UpdateStateReport(this.idRep, reportState.StateReport, reportState.PercentReports, reportState.StartDate, reportState.EndDate, reportState.TypeError);
                    }
                }
            }
            // No se genero ningun reporte.
            else
            {
                // Si es un nuevo reporte.
                if (this.newRep)
                {
                    // Insertando ReportState.
                    dc.SP_UpdateStateReport(this.idRep, reportState.StateReport, reportState.PercentReports, reportState.StartDate, reportState.EndDate, reportState.TypeError);
                }
            }

            // Estableciendo el cuerpo del correo electronico de confirmacion del envio de reportes.
            string mailbody = "";
            switch (sent)
            {
                case 0:
                    mailbody = "Envio de reportes no realizado.<br><br>" +
                                "Si desea volver a enviar los reportes no enviados ingrese a la siguiente direccion: ''";
                    break;
                case 1:
                    mailbody = "Envio de reportes realizado exitosamente.<br><br>";
                    break;
                case 2:
                    mailbody = "Envio de reportes parcialmente realizado.<br><br>" +
                                "Si desea volver a enviar los reportes no enviados ingrese a la siguiente direccion: ''";
                    break;
            }

            // Obteniendo los correo a los que se les debe enviar confirmacion.
            List<string> emails = (from ce in dc.SP_ConfirmEmail() select ce.email).ToList();

            foreach (string str in emails)
            {
                // Enviando correos de conformacion.
                sendConfirmEmail(reportState.StartDate.ToString("MM-dd-yyyy"), reportState.EndDate.ToString("MM-dd-yyyy"), str, mailbody);
            }
        }


        //----------------------------------------------------------------------------------------------------------------
        // Funcion: enviar correo electronico con reporte de asistencia
        // Parametros: 
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalizacion del reporte.
        //          to          -> correo electronico al que se enviara el reporte.
        //          mailbody    -> cuerpo del correo electronico.
        //----------------------------------------------------------------------------------------------------------------
        public void sendConfirmEmail(string startDate, string endDate, string to, string mailbody)
        {
            if (to != null)
            {
                // to = "test.reports.anviz@gmail.com";

                string subject = "Correo de confirmación de de envio de reportes: " + startDate + " - " + endDate;

                try
                {
                    MailMessage mail = new MailMessage("no-reply@laureate.net", to, subject, mailbody);
                    mail.IsBodyHtml = true;
                    mail.SubjectEncoding = Encoding.Default;
                    mail.BodyEncoding = Encoding.UTF8;

                    SmtpClient client = new SmtpClient();

                    client.Host = "smtp.mandrillapp.com";
                    client.Port = int.Parse("587");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential("raul.rivera@laureate.net", "uuQ-6gYsZG5Yzz7Hv-a3yA");
                    client.Send(mail);
                }
                catch
                {
                }
            }
        }


        public IEnumerable<object> query { get; set; }
    }
}