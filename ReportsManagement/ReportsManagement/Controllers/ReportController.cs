using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportsManagement.Models;

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
using Mandrill;

namespace ReportsManagement.Controllers
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de brindar toda la funcionalidad relacionada 
    //          con la generación de reportes de empleados, equipos y departamentos.
    // Propiedades: no tiene ninguna propiedad.
    // Metodos: tiene una serie de metodos con una descripción detallada.
    // ------------------------------------------------------------------------------
    [Authorize]
    public class ReportController : Controller
    {
        // ------------------------------------------------------------------------------
        // GET: /Report/configureReport?message=1&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se configuraran los parametros para generar un reporte.
        // Parametros: 
        //          message         -> representa el tipo mensaje de retroalimentación que se mostrara 
        //                             al usuario dependiendo del escenario que se presente.
        //          messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult configureReport(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["employees"] = conection.employees().OrderBy(param => param.Name).ToList();
                ViewData["departments"] = conection.departments();
                ViewData["teamsMongo"] = conection.teamsMongo().OrderBy(param => param.subdept).ToList();
                ViewData["departmentsMongo"] = conection.departmentsMongo().OrderBy(param => param.Nombre_Dept).ToList();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // POST: /Report/configureReport
        // Funcion: accion encargada de generar los reportes solicitados por el usuario.
        // Parametros:
        //          type        -> tipo de reporte que se va a generar.
        //          listItem    -> lista de identificadores de empleados, equipos y departamentos para los que se generaran los reportes.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          method      -> opciones para generar el reporte.
        //                              enviar a cada empleado, responsable de equipo o responsable de departamento segun sea el caso.
        //                              enviar a un nuevo correo definido por el usuario.
        //                              descargar.
        //          newEmail    -> parametro opcional en caso de seleccionar la opcion (method) enviar a un nuevo correo.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult configureReport(string type, int[] listItem, string selectAll,  DateTime startDate, DateTime endDate, string[] method, string newEmail = "")
        {
            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Llamando a los metodos encargados de generar reportes tanto de empleados, equipos y departamentos. 
            // Manejando cualquier excepcion que se presente.
            try
            {
                switch (type)
                {
                    case "employees":
                        employeesReports(listItem, selectAll, startDate, endDate, method, newEmail);
                        break;
                    case "teams":
                        teamsReports(listItem, selectAll, startDate, endDate, method, newEmail);
                        break;
                    case "departments":
                        departmentsReports(listItem, selectAll, startDate, endDate, method, newEmail);
                        break;
                }
                message = 2;
                messageValue = "Reportes generados exitosamente.";
            }
            catch (Exception e)
            {
                // Cargando informacion que se pasara a la vista en caso de error.
                message = 3;
                messageValue = "Reportes no generados. Por favor intentelo de nuevo.";
                messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Report",
                action = "configureReport",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de generar los reportes de departamentos.
        // Parametros:
        //          listItem    -> lista de identificadores de departamentos para los que se generaran los reportes.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          method      -> opciones para generar el reporte.
        //                              enviar a cada empleado, responsable de equipo o responsable de departamento segun sea el caso.
        //                              enviar a un nuevo correo definido por el usuario.
        //                              descargar.
        //          newEmail    -> parametro opcional en caso de seleccionar la opcion (method) enviar a un nuevo correo.
        // ------------------------------------------------------------------------------
        public void departmentsReports(int[] listItem, string selectAll, DateTime startDate, DateTime endDate, string[] method, string newEmail = "")
        {
            Conection conection = new Conection();

            if (selectAll == "all")
            {
                listItem = (from dept in conection.departments() where dept.SupDeptid == 1 && dept.DeptName != "VISITORS" select dept.Deptid).ToArray();
            }
            
            foreach (string option in method)
            {
                // Creando reporte individual por departamento y enviar al correo del responsable del departamento.
                if (option == "email")
                {
                    // Estableciendo el directorio donde se almacenara el reporte.
                    string path = @"C:\\Pdf\\Other_Reports\\Departments-" + DateTime.Now.ToString("MM-dd-yy") + "\\";

                    // Iteraando listItem (identificadores de departamentos).
                    foreach (int item in listItem)
                    {
                        // Obteniendo un departamento segun item (identificador de un departamento).
                        DepartmentMongo dept = conection.searchDepartmentMongo(item);

                        // Estableciendo el filename (nombre del archivo).
                        string nameDept = dept.Nombre_Dept.Replace("ñ", "n");
                        nameDept = nameDept.Replace("Ñ", "n");
                        nameDept = nameDept.Replace(" ", "_");
                        nameDept = nameDept.Replace("\u0001", " ");
                        string filename = "Attendance_Report_" + nameDept + "_" + startDate.ToString("MM-dd-yyyy") + "_to_" + endDate.ToString("MM-dd-yyyy") + ".pdf";

                        // Obteniendo los equipos pertenecientes al departamento.
                        List<GetReportMongo> teams = conection.searchDepartmentMongo(Convert.ToInt32(dept.Id_Dept)).GetReport.ToList();

                        // Creaando un reporte de un departamento.
                        createReport(startDate, endDate, path, filename, teams: teams);

                        // Cuerpo del correo electronico.
                        string mailbody = "Dear Colleague,<br><br>" +
                                "This weekly attendance report shows your team’s daily attendance and the office hours they have logged from " + startDate.ToString("MM-dd-yyyy") + " to " + endDate.ToString("MM-dd-yyyy") +
                                " according to the fingerprint scanner and/or their remote time log.<br><br>" +
                                "It is important for us to provide this information so you can keep track and control your team’s attendance and office hours.<br><br>" +
                                "If you do not wish to receive this report or would like someone else from your team to receive it, please contact Alana García, LNO Honduras HR Administrator, at alana.garcia@laureate.net.<br><br>" +
                                "Best Regards,<br><br>" +
                                "HR Team";

                        // Enviando el reporte por correo electronico al responsable del departamento.
                        sendEmail(dept.Name[0], dept.Nombre_Dept, mailbody, dept.correo[0], path + filename);
                    }
                }
                // Creaando un reporte con todos los departamentos seleccionados.
                if (option == "newEmail" || option == "download")
                {
                    // Estableciendo el directorio donde se almacenara el reporte, el nombre del archivo y el nombre del reporte.
                    string path = @"C:\\Pdf\\Other_Reports\\Departments-" + DateTime.Now.ToString("MM-dd-yy") + "\\";
                    string filename = "Attendance_Report_Departments_" + startDate.ToString("MM-dd-yyyy") + "_to_" + endDate.ToString("MM-dd-yyyy") + ".pdf";
                    string reportName = "";

                    // Creando lista de departamentos a los que se generara el reporte.
                    // Modificando el nombre del archivo y nombre del reporte.
                    List<DepartmentMongo> departments = new List<DepartmentMongo>();
                    foreach (int item in listItem)
                    {
                        // filename = filename + item;
                        DepartmentMongo dept = conection.searchDepartmentMongo(item);
                        reportName = reportName + dept.Nombre_Dept + ", ";
                        departments.Add(dept);
                    }
                    departments = departments.OrderBy(param => param.Nombre_Dept).ToList();
                    // filename = filename + ".pdf";

                    // Creando un reporte de uno o mas departamentos (segun la selección del usuario).
                    createReport(startDate, endDate, path, filename, departments: departments);

                    // Enviando por correo electronico el reporte generado (correo ingresado por el usuario).
                    if (option == "newEmail")
                    {
                        // Ruta donde se encuentra el archivo que contiene el reporte.
                        path = path + filename;
                        reportName = reportName.TrimEnd(new char[] { ' ', ',' });

                        // Cuerpo del correo electronico.
                        string mailbody = "Dear Colleague,<br><br>" +
                            "This attendance report shows the daily attendance and the office hours of the following departments: " + reportName + ". From " + startDate.ToString("MM-dd-yyyy") + " to " + endDate.ToString("MM-dd-yyyy") +
                            " according to the fingerprint scanner and/or their remote time log.<br><br>" +
                            "Best Regards,<br><br>" +
                            "HR Team";

                        // Enviando el reporte por correo electronico al correo ingresado por el usuario.
                        sendEmail("", reportName, mailbody, newEmail, path);
                    }
                    // Descargando el reporte generado.
                    if (option == "download")
                    {
                        path = path + filename;
                        download(filename, path);
                    }
                }
            }
            
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de generar los reportes de equipos.
        // Parametros:
        //          listItem    -> lista de identificadores de equipos para los que se generaran los reportes.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          method      -> opciones para generar el reporte.
        //                              enviar a cada empleado, responsable de equipo o responsable de departamento segun sea el caso.
        //                              enviar a un nuevo correo definido por el usuario.
        //                              descargar.
        //          newEmail    -> parametro opcional en caso de seleccionar la opcion (method) enviar a un nuevo correo.
        // ------------------------------------------------------------------------------
        public void teamsReports(int[] listItem, string selectAll, DateTime startDate, DateTime endDate, string[] method, string newEmail = "") 
        {
            Conection conection = new Conection();
            
            if (selectAll == "all")
            {
                listItem = (from dept in conection.departments() where dept.SupDeptid != 1 && dept.SupDeptid != 0 select dept.Deptid).ToArray();
            }
            foreach (string option in method)
            {
                // Creando reporte individual por equipo y enviar al correo del responsable del equipo.
                if (option == "email")
                {
                    // Estableciendo la ruta donde se almacenara el reporte.
                    string path = @"C:\\Pdf\\Other_Reports\\Teams-" + DateTime.Now.ToString("MM-dd-yy") + "\\";

                    // Iterando listItem (lista de identificadores de equipo).
                    foreach (int item in listItem)
                    {
                        // Obteniendo un equipo segun item (identificador de equipo).
                        GetReportMongo team = conection.searchTeamMongo(item, conection.searchDepartmentTeamMongo(item));

                        // Estableciendo el nombre del archivo que contendra el reporte.
                        string nameTeam = "";
                        try
                        {
                            nameTeam = team.subdept.Replace("ñ", "n");
                        }
                        catch
                        {
                            throw new Exception("" + item);
                        }
                        nameTeam = nameTeam.Replace("Ñ", "n");
                        nameTeam = nameTeam.Replace(" ", "_");
                        nameTeam = nameTeam.Replace("\u0001", " ");
                        string filename = "Attendance_Report_" + nameTeam + "_" + startDate.ToString("MM-dd-yyyy") + "_to_" + endDate.ToString("MM-dd-yyyy") + ".pdf";

                        // Obteniendo todos los empleados que pertenecen al equipo.
                        List<Employee> employees = conection.teamEmployees(item).OrderBy(param => param.Name).ToList();

                        // Creando un reporte de un equipo.
                        createReport(startDate, endDate, path, filename, employees: employees);

                        // Cuerpo del correo electronico.
                        string mailbody = "Dear Colleague,<br><br>" +
                                "This weekly attendance report shows your team’s daily attendance and the office hours they have logged from " + startDate.ToString("MM-dd-yyyy") + " to " + endDate.ToString("MM-dd-yyyy") +  
                                " according to the fingerprint scanner and/or their remote time log.<br><br>" +
                                "It is important for us to provide this information so you can keep track and control your team’s attendance and office hours.<br><br>" +
                                "If you do not wish to receive this report or would like someone else from your team to receive it, please contact Alana García, LNO Honduras HR Administrator, at alana.garcia@laureate.net.<br><br>" +
                                "Best Regards,<br><br>" +
                                "HR Team";

                        // Enviando el reporte por correo electronico al responsable del equipo.
                        sendEmail(team.subName, team.subdept, mailbody, team.subEmail, path + filename);
                    }
                }

                // Creaando un reporte con todos los equipos seleccionados.
                if (option == "newEmail" || option == "download")
                {
                    // Estableciendo el directorio donde se almacenara el reporte, el nombre del archivo y el nombre del reporte.
                    string path = @"C:\\Pdf\\Other_Reports\\Teams-" + DateTime.Now.ToString("MM-dd-yy") + "\\";
                    string filename = "Attendance_Report_Teams_" + startDate.ToString("MM-dd-yyyy") + "_to_" + endDate.ToString("MM-dd-yyyy") + ".pdf";
                    string reportName = "";

                    // Creaando lista de equipos a los que se generara el reporte.
                    // Modificando el nombre del archivo y nombre del reporte.
                    List<GetReportMongo> teams = new List<GetReportMongo>();
                    foreach (int item in listItem)
                    {
                        // filename = filename + item;
                        GetReportMongo team = conection.searchTeamMongo(item, conection.searchDepartmentTeamMongo(item));
                        reportName = reportName + team.subdept + ", ";
                        teams.Add(team);
                    }
                    teams = teams.OrderBy(param => param.subdept).ToList();
                    // filename = filename + ".pdf";

                    // Creando un reporte de uno o mas equipos segun la seleccion del usuario.
                    createReport(startDate, endDate, path, filename, teams: teams);

                    // Enviando por correo electronico el reporte generado (correo ingresado por el usuario).
                    if (option == "newEmail")
                    {
                        // Ruta donde se encuentra el archivo que contiene el reporte.
                        path = path + filename;
                        reportName = reportName.TrimEnd(new char[] { ' ', ',' });

                        // Cuerpo del correo electronico.
                        string mailbody = "Dear Colleague,<br><br>" +
                            "This attendance report shows the daily attendance and the office hours of the following teams: " + reportName + ". From " + startDate.ToString("MM-dd-yyyy") + " to " + endDate.ToString("MM-dd-yyyy") +
                            " according to the fingerprint scanner and/or their remote time log.<br><br>" +
                            "Best Regards,<br><br>" +
                            "HR Team";

                        // Enviando el reporte por correo electronico al correo ingresado por el usuario.
                        sendEmail("", reportName, mailbody, newEmail, path);
                    }
                    // Descargando el reporte generado.
                    if (option == "download")
                    {
                        path = path + filename;
                        download(filename, path);
                    }
                }
            }
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de generar los reportes de empleados.
        // Parametros:
        //          listItem    -> lista de identificadores de empleados, equipos y departamentos para los que se generaran los reportes.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          method      -> opciones para generar el reporte.
        //                              enviar a cada empleado, responsable de equipo o responsable de departamento segun sea el caso.
        //                              enviar a un nuevo correo definido por el usuario.
        //                              descargar.
        //          newEmail    -> parametro opcional en caso de seleccionar la opcion (method) enviar a un nuevo correo.
        // ------------------------------------------------------------------------------
        public void employeesReports(int[] listItem, string selectAll, DateTime startDate, DateTime endDate, string[] method, string newEmail = "")
        {
            Conection conection = new Conection();

            if (selectAll == "all")
            {
                listItem = (from emp in conection.employees() select emp.Id).ToArray(); 
            }
            
            foreach (string option in method)
            {
                // Creando reporte individual por empleado y enviar al correo del mismo.
                if (option == "email")
                {
                    // Estableciendo la ruta donde se almacenara el reporte.
                    string path = @"C:\\Pdf\\Other_Reports\\Employees-" + DateTime.Now.ToString("MM-dd-yy") + "\\";
                    
                    // Iterando listItem (lista de identificadores de empleados).
                    foreach (int item in listItem)
                    {
                        // Obteniendo un empleado segun item (identificador de empleado).
                        Employee emp = conection.searchEmployee(item);

                        // Estableciendo el nombre del archivo que contendra el reporte.
                        string name = emp.Name.Replace("ñ", "n");
                        name = name.Replace("Ñ", "n");
                        name = name.Replace(" ", "_");
                        name = name.Replace("\u0001", " ");
                        string filename = "Attendance_Report_" + name + "_" + startDate.ToString("MM-dd-yyyy") + "_to_" + endDate.ToString("MM-dd-yyyy") + ".pdf";
                        
                        // Creando un reporte de un empleado.
                        createReport(startDate, endDate, path, filename, employee: emp);

                        // Cuerpo del correo electronico.
                        string mailbody = "Dear Colleague,<br><br>" +
                            "This attendance report shows your daily attendance and the office hours you have logged from " + startDate.ToString("MM-dd-yyyy") + " to " + endDate.ToString("MM-dd-yyyy") + 
                            " according to the fingerprint scanner and/or your remote time log.<br><br>" +
                            "It is important for us to provide this information so you can keep track and control your attendance and office hours.<br><br>" +
                            "Best Regards,<br><br>" +
                            "HR Team";

                        // Enviando el reporte por correo electronico al empleado.
                        sendEmail(emp.Name, emp.Name, mailbody, emp.Email, path + filename);
                    }
                }
                // Creando un reporte con todos los empleados seleccionados.
                if (option == "newEmail" || option == "download")
                {
                    // Estableciendo el directorio donde se almacenara el reporte, el nombre del archivo y el nombre del reporte.
                    string path = @"C:\\Pdf\\Other_Reports\\Employees-" + DateTime.Now.ToString("MM-dd-yy") + "\\"; ;
                    string filename = "Attendance_Report_Employees_" + startDate.ToString("MM-dd-yyyy") + "_to_" + endDate.ToString("MM-dd-yyyy") + ".pdf";
                    string reportName = "";

                    // Creando lista de empleados a los que se generara el reporte.
                    // Modificando el nombre del archivo y nombre del reporte.
                    List<Employee> employees = new List<Employee>();
                    foreach (int item in listItem)
                    {
                        // filename = filename + item;
                        Employee emp = conection.searchEmployee(item);
                        reportName = reportName + emp.Name + ", ";
                        employees.Add(emp);
                    }
                    employees = employees.OrderBy(param => param.Name).ToList();
                    // filename = filename + ".pdf";

                    // Creando un reporte de uno o mas empleados segun la seleccion del usuario.
                    createReport(startDate, endDate, path, filename, employees: employees);

                    // Enviando por correo electronico el reporte generado (correo ingresado por el usuario).
                    if (option == "newEmail")
                    {
                        // Ruta donde se encuentra el archivo que contiene el reporte.
                        path = path + filename;
                        reportName = reportName.TrimEnd(new char[] {' ', ','});

                        // Cuerpo del correo electronico.
                        string mailbody = "Dear Colleague,<br><br>" +
                            "This attendance report shows the daily attendance and the office hours of: " + reportName + ". From " + startDate.ToString("MM-dd-yyyy") + " to " + endDate.ToString("MM-dd-yyyy") +
                            " according to the fingerprint scanner and/or their remote time log.<br><br>" +
                            "Best Regards,<br><br>" +
                            "HR Team";

                        // Enviando el reporte por correo electronico al correo ingresado por el usuario.
                        sendEmail("", reportName, mailbody, newEmail, path);
                    }
                    // Descargando el reporte generado.
                    if (option == "download")
                    {
                        path = path + filename;
                        download(filename, path);
                    }
                }
            }
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de crear los directorios y documentos pdf donde se escribiran los reportes.
        // Parametros:
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          path        -> ruta de la carpeta que contendra los reportes.
        //          filename    -> mombre del reporte.
        //          employee    -> parametro opcional, es un empleado al que se le generar un reporte.
        //          employees   -> parametro opcional, es una lista de empleados a los que se les generara un reporte.
        //          teams       -> parametro opcional, es una lista de GetReport(Teams) a los que se les generara un reporte.
        //          departments -> parametro opcional, es una lista de departamentos a los que se les generara un reporte.
        // ------------------------------------------------------------------------------
        public void createReport(DateTime startDate, DateTime endDate, string path, string filename, Employee employee = null, List<Employee> employees = null, List<GetReportMongo> teams = null, List<DepartmentMongo> departments = null )
        {
            Conection conection = new Conection();

            // Creando un nuevo documento.
            Document document = new Document(PageSize.A1, 5, 5, 155, 15);

            // Creando directorios
            string folder = @"C:\\Pdf\\Other_Reports";
            bool folderExists = Directory.Exists((folder));
            if (!folderExists)
                Directory.CreateDirectory((folder));
            folderExists = Directory.Exists((path));
            if (!folderExists)
                Directory.CreateDirectory((path));
            path = path + filename;

            // Agregando información (tablas) al documento. 
            // if ((!System.IO.File.Exists(path)))
            // {
            using (FileStream output = new FileStream((path), FileMode.Create))
            {
                using (PdfWriter writer = PdfWriter.GetInstance(document, output))
                {
                    document.Open();
                    if (departments != null)
                    {
                        createDocument(departments, startDate, endDate, document);
                    }
                    else
                    {
                        if (teams != null)
                        {
                            createDocument(teams, startDate, endDate, document);
                        }
                        else
                        {
                            if (employees != null)
                            {
                                createDocument(employees, startDate, endDate, document);
                            }
                            else
                            {
                                createDocument(employee, startDate, endDate, document);
                            }
                        }
                    }
                    PdfPTable tableLegend = new PdfPTable(24);
                    document.Add(legend(tableLegend));
                    document.Close();
                    output.Close();
                }
            }
            // }
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de crear reportes que incluyen varios departamentos.
        // Parametros:
        //          departments -> es una lista de departamentos a los que se les generara un reporte.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          document    -> documento en donde se generara el reporte.
        // ------------------------------------------------------------------------------
        public void createDocument(List<DepartmentMongo> departments, DateTime startDate, DateTime endDate, Document document)
        {
            foreach (DepartmentMongo dept in departments)
            {
                PdfPTable table = new PdfPTable(12);

                iTextSharp.text.Font Headers = FontFactory.GetFont("georgia", 24f);
                Headers.Color = iTextSharp.text.BaseColor.BLACK;

                PdfPCell cellTeam = new PdfPCell(new Phrase(dept.Nombre_Dept, Headers));
                cellTeam.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTeam.BackgroundColor = new iTextSharp.text.BaseColor(245, 92, 24);
                cellTeam.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellTeam.Colspan = 24;

                table.AddCell(cellTeam);

                document.Add(table);

                // Utilizando el metodo createDocument para varios equipos.
                createDocument(dept.GetReport.ToList(), startDate, endDate, document);
            }
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de crear reportes que incluyen varios equipos.
        //          Estos equipos pueden provenir de un mismo departamento o escogidos de entre todos los equipos.
        // Parametros:
        //          teams       -> es una lista de equipos a los que se les generara un reporte.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          document    -> documento en donde se generara el reporte.
        // ------------------------------------------------------------------------------
        public void createDocument(List<GetReportMongo> teams, DateTime startDate, DateTime endDate, Document document)
        {
            Conection conection = new Conection();
            foreach (GetReportMongo team in teams)
            {
                List<Employee> employees = conection.teamEmployees(Convert.ToInt32(team.deptID));

                PdfPTable table = new PdfPTable(12);

                iTextSharp.text.Font Headers = FontFactory.GetFont("georgia", 20f);
                Headers.Color = iTextSharp.text.BaseColor.BLACK;

                PdfPCell cellTeam = new PdfPCell(new Phrase(team.subdept, Headers));
                cellTeam.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTeam.BackgroundColor = new iTextSharp.text.BaseColor(245, 92, 24);
                cellTeam.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                cellTeam.Colspan = 24;

                table.AddCell(cellTeam);
                document.Add(table);

                // Utilizando el metodo createDocument para varios empleados.
                createDocument(employees, startDate, endDate, document);
            }
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de crear reportes que incluyen varios empleados.
        //          Estos empleados pueden provenir de un mismo equipo o escogidos de entre todos los empleados.
        // Parametros:
        //          employees   -> es una lista de empleados a los que se les generara un reporte.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          document    -> documento en donde se generara el reporte.
        // ------------------------------------------------------------------------------
        public void createDocument(List<Employee> employees, DateTime startDate, DateTime endDate, Document document)
        {
            foreach (Employee emp in employees)
            {
                // Utilizando el metodo createDocument para un empleado.
                createDocument(emp, startDate, endDate, document);
            }
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de crear reportes de un empleado.
        // Parametros:
        //          employee    -> es una empleado al que se le generara un reporte.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        //          document    -> documento en donde se generara el reporte.
        // ------------------------------------------------------------------------------
        public void createDocument(Employee employee, DateTime startDate, DateTime endDate, Document document)
        {
            PdfPTable table = new PdfPTable(24);
            table.TotalWidth = 5000f;
            document.Add(createTable(table, employee.Id, employee.Name, startDate, endDate));
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de descargar los reportes generados.
        // Parametros:
        //          filename    -> nombre del documento donde se genero el reporte.
        //          path        -> directorio donde se genero el reporte.
        // ------------------------------------------------------------------------------
        private void download(string filename, string path)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            // Response.AddHeader("Content-Disposition", "Attachment; filename=\"" + filename + "\"");
            Response.Flush();
            Response.TransmitFile(path);
            Response.End();
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de enviar por correo electronico los reportes generados.
        // Parametros:
        //          toName      -> nombre hacia quien va dirigido el correo.
        //          reportName  -> nombre del reporte.
        //          mailbody    -> cuerpo del correo.
        //          to          -> correo electronico a donde se envia el reporte.
        //          path        -> ruta donde se encuentra el archivo con el reporte.
        // ------------------------------------------------------------------------------
        public void sendEmail(string toName, string reportName, string mailbody, string to, string path)
        {
            if (to != null)
            {
                // to = "guillermosalgado22@gmail.com";
                // to = "test.reports.anviz@gmail.com";

                // Estableciendp el subject del correo electronico.
                string subject = "Sending " + toName + " email with attendance report  " + reportName + ".";

                try
                {
                    // Adjuntando archivo que contiene el reporte.
                    Attachment att = new Attachment(path);

                    // Estableciendo parametros del correo.
                    MailMessage mail = new MailMessage("no-reply@laureate.net", to, subject, mailbody);
                    mail.IsBodyHtml = true;
                    mail.SubjectEncoding = Encoding.Default;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.Attachments.Add(att);

                    // Estableciendo parametros del cliente SMTP para enviar el correo.
                    SmtpClient client = new SmtpClient();
                    client.Host = "smtp.mandrillapp.com";
                    client.Port = int.Parse("587");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("raul.rivera@laureate.net", "uuQ-6gYsZG5Yzz7Hv-a3yA");

                    // Enviando el correo.
                    client.Send(mail);
                }
                catch
                {

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


        // ------------------------------------------------------------------------------
        // Funcion: metodo encargado de descargar los reportes generados.
        // Parametros:
        //          table       -> en pocas palabras contiene el reporte por empleado.
        //                          Un reporte es un documento que contiene una o mas tablas.
        //          idEmp       -> identificador del empleado al que se le esta generando un reporte.
        //          name        -> nombre del empleado al que se le esta generando un reporte.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de fin del reporte.
        // ------------------------------------------------------------------------------
        public PdfPTable createTable(PdfPTable table, int idEmp, string name,  DateTime startDate, DateTime endDate)
        {
            Conection conection = new Conection();

            // Almacenara la información para generar un reporte de un empleado.
            List<InfoReport> infoReport = new List<InfoReport>();

            // Definiendo formato de letras para encabezados y etiquetas.
            iTextSharp.text.Font georgia = FontFactory.GetFont("georgia", 14f);
            georgia.Color = iTextSharp.text.BaseColor.BLUE;
            iTextSharp.text.Font labels = FontFactory.GetFont("georgia", 14f);
            labels.Color = iTextSharp.text.BaseColor.WHITE;
            iTextSharp.text.Font Headers = FontFactory.GetFont("georgia", 18f);
            Headers.Color = iTextSharp.text.BaseColor.BLACK;

            // Definiendo el ancho total de la tabla
            table.TotalWidth = 5000f;

            float[] widths = new float[] { 1300f, 1300, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f, 1300f };
            table.SetWidths(widths);

            // Obteniendo información para generar el reporte.
            infoReport = conection.reportEmployees(idEmp, startDate, endDate);

            // Obteniendo el lunes de la semana anterior a la fecha de inicio (startDate).
            int day = Convert.ToInt32(startDate.DayOfWeek);
            day = day - 1 + 7;
            DateTime date = startDate.AddDays(day * (-1));

            // Formateando las celdas de encabezado y agregandolas a la tabla.
            PdfPCell cellNameTop = new PdfPCell(new Phrase(name, Headers));
            cellNameTop.HorizontalAlignment = Element.ALIGN_CENTER;
            cellNameTop.BackgroundColor = new iTextSharp.text.BaseColor(245, 92, 24);
            cellNameTop.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            cellNameTop.Colspan = 24;
            table.AddCell(cellNameTop);

            PdfPCell employeeName = new PdfPCell(new Phrase("Week", labels));
            employeeName.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
            employeeName.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            employeeName.BorderWidthRight = 1;
            employeeName.Rowspan = 2;
            employeeName.Colspan = 2;
            employeeName.HorizontalAlignment = 1;
            employeeName.VerticalAlignment = 1;
            table.AddCell(employeeName);

            PdfPCell headers = new PdfPCell(new Phrase("Monday\n", Headers));
            headers.BorderWidthRight = 1;
            headers.HorizontalAlignment = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.Colspan = 3;
            table.AddCell(headers);

            headers = new PdfPCell(new Phrase("Tuesday\n", Headers));
            headers.BorderWidthRight = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.HorizontalAlignment = 1;
            headers.Colspan = 3;
            table.AddCell(headers);

            headers = new PdfPCell(new Phrase("Wednesday\n", Headers));
            headers.BorderWidthRight = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.HorizontalAlignment = 1;
            headers.Colspan = 3;
            table.AddCell(headers);

            headers = new PdfPCell(new Phrase("Thursday\n", Headers));
            headers.BorderWidthRight = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.HorizontalAlignment = 1;
            headers.Colspan = 3;
            table.AddCell(headers);

            headers = new PdfPCell(new Phrase("Friday\n", Headers));
            headers.BorderWidthRight = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.HorizontalAlignment = 1;
            headers.Colspan = 3;
            table.AddCell(headers);

            headers = new PdfPCell(new Phrase("Saturday\n", Headers));
            headers.BorderWidthRight = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.HorizontalAlignment = 1;
            headers.Colspan = 3;
            table.AddCell(headers);

            headers = new PdfPCell(new Phrase("Sunday\n", Headers));
            headers.BorderWidthRight = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(247, 150, 70);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            headers.HorizontalAlignment = 1;
            headers.Colspan = 3;
            table.AddCell(headers);

            headers = new PdfPCell(new Phrase("Weekly Hours Worked", labels));
            headers.BorderWidthRight = 1;
            headers.Rowspan = 2;
            headers.HorizontalAlignment = 1;
            headers.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
            headers.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
            table.AddCell(headers);

            // Formateando las celdas de encabezado de hora de entrada, hora de salida y horas trabajadas y agregandolas a la tabla.
            for (var y = 0; y <= 6; y++)
            {
                PdfPCell inHour = new PdfPCell(new Phrase("In", labels));
                inHour.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
                inHour.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                inHour.HorizontalAlignment = 1;
                table.AddCell(inHour);

                PdfPCell outHour = new PdfPCell(new Phrase("Out", labels));
                outHour.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
                outHour.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                outHour.HorizontalAlignment = 1;
                table.AddCell(outHour);

                PdfPCell hours = new PdfPCell(new Phrase("Worked Hours", labels));
                hours.BackgroundColor = new iTextSharp.text.BaseColor(89, 89, 89);
                hours.BorderColor = new iTextSharp.text.BaseColor(0, 0, 0);
                hours.BorderWidthRight = 1;
                hours.HorizontalAlignment = 1;
                table.AddCell(hours);
            }

            // Iterando los datos por semana de un empleado.
            // Agregando datos al reporte.
            foreach(InfoReport weekReport in infoReport)
            {
                // Lunes de la semana siguiente.
                date = date.AddDays(7);

                PdfPCell cell = new PdfPCell();
                PdfPCell cellDate = new PdfPCell();
                PdfPCell cellMonday = new PdfPCell();
                PdfPCell cellTuesday = new PdfPCell();
                PdfPCell cellWednesday = new PdfPCell();
                PdfPCell cellThursday = new PdfPCell();
                PdfPCell cellfriday = new PdfPCell();
                PdfPCell cellsaturday = new PdfPCell();
                PdfPCell cellsunday = new PdfPCell();

                cellDate.Phrase = new Phrase(date.ToShortDateString() + "  to  " + date.AddDays(6).ToShortDateString() );
                cellDate.HorizontalAlignment = 1;
                cellDate.Colspan = 2;
                cellDate.BorderWidthRight = 1;
                table.AddCell(cellDate);

                // For Monday
                if (weekReport.SensorInMonday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (weekReport.SensorOutMonday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaMonday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellMonday = new PdfPCell(new Phrase((weekReport.HorasMonday).ToString()));
                cellMonday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellMonday.BorderWidthRight = 1;
                cellMonday.HorizontalAlignment = 1;
                table.AddCell(cellMonday);

                //For Tuesday
                if (weekReport.SensorInTuesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (weekReport.SensorOutTuesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaTuesday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellTuesday = new PdfPCell(new Phrase((weekReport.HorasTuesday).ToString()));
                cellTuesday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellTuesday.BorderWidthRight = 1;
                cellTuesday.HorizontalAlignment = 1;
                table.AddCell(cellTuesday);

                //For Wednesday
                if (weekReport.SensorInWednesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (weekReport.SensorOutWednesday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaWendnsday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellWednesday = new PdfPCell(new Phrase((weekReport.HorasWendnsday).ToString()));
                cellWednesday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellWednesday.BorderWidthRight = 1;
                cellWednesday.HorizontalAlignment = 1;
                table.AddCell(cellWednesday);

                //For Thursday
                if (weekReport.SensorInThursday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (weekReport.SensorOutThursday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaThursday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellThursday = new PdfPCell(new Phrase((weekReport.HorasThursday).ToString()));
                cellThursday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellThursday.BorderWidthRight = 1;
                cellThursday.HorizontalAlignment = 1;
                table.AddCell(cellThursday);

                //For Friday

                if (weekReport.SensorInFriday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (weekReport.SensorOutFriday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaFriday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellfriday = new PdfPCell(new Phrase((weekReport.HorasFriday).ToString()));
                cellfriday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellfriday.BorderWidthRight = 1;
                cellfriday.HorizontalAlignment = 1;
                table.AddCell(cellfriday);

                //For Saturday

                if (weekReport.SensorInSaturday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                if (weekReport.SensorOutSaturday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaSaturday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }

                cellsaturday = new PdfPCell(new Phrase((weekReport.HorasSaturday).ToString()));
                cellsaturday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellsaturday.BorderWidthRight = 1;
                cellsaturday.HorizontalAlignment = 1;
                table.AddCell(cellsaturday);

                //For Sunday
                if (weekReport.SensorInSunday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaSunday).ToShortTimeString().ToString());

                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaEntradaSunday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);

                }

                if (weekReport.SensorOutSunday == 99)
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaSunday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(187, 176, 163);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else
                {
                    cell.Phrase = new Phrase(Convert.ToDateTime(weekReport.FechaSalidaSunday).ToShortTimeString().ToString());
                    cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }


                cellsunday = new PdfPCell(new Phrase((weekReport.HorasSunday).ToString()));
                cellsunday.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cellsunday.BorderWidthRight = 1;
                cellsunday.HorizontalAlignment = 1;
                table.AddCell(cellsunday);

                cell.Phrase = new Phrase(Convert.ToString(Convert.ToDecimal(weekReport.HorasMonday) + Convert.ToDecimal(weekReport.HorasTuesday) + Convert.ToDecimal(weekReport.HorasWendnsday) + Convert.ToDecimal(weekReport.HorasThursday) + Convert.ToDecimal(weekReport.HorasFriday) + Convert.ToDecimal(weekReport.HorasSaturday) + Convert.ToDecimal(weekReport.HorasSunday)));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(242, 242, 242);
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
            }
            return table;
        }
    }
}
