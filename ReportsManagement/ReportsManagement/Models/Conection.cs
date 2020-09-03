using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB.Bson;
using MongoDB.Driver;


namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase es el principal modelo. Se encarga de proporcionar a los 
    //          controladores toda la informacion necesaria proveniente de las bases de datos. 
    //          Ademas de actualizar los registros o documentos en las bases de datos.
    // Propiedades: no tiene ninguna propiedad.
    // Metodos: tiene una serie de metodos con una descripción detallada.
    // ------------------------------------------------------------------------------
    public class Conection
    {
        // ------------------------------------------------------------------------------
        // Funcion: crear y retornar una conexion con la base de datos SQL.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public Anviz_Data_BaseEntities Sql()
        {
            return new Anviz_Data_BaseEntities();
        }


        // ------------------------------------------------------------------------------
        // Funcion: obtener y retornar la coleccion managers de la base de datos mongo.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public IMongoCollection<DepartmentMongo> Mongo()
        {
            MongoClient client = new MongoClient();
            IMongoDatabase db = client.GetDatabase("structures");
            return db.GetCollection<DepartmentMongo>("responsables");
        }


        // ------------------------------------------------------------------------------
        // Funcion: obtener y retornar todos los departamentos de la base de datos mongo.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<DepartmentMongo> departmentsMongo()
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();
            List<DepartmentMongo> documents = departments.Find("{}").ToListAsync().Result;

            return documents;
        }


        // ------------------------------------------------------------------------------
        // Funcion: obtener y retornar un departamento especifico de la base de datos mongo.
        // Parametros: 
        //          id -> identificador del departamento que se va a buscar.
        // ------------------------------------------------------------------------------
        public DepartmentMongo searchDepartmentMongo(int id)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            List<DepartmentMongo> documents = departments
                    .Find(a => a.Id_Dept == Convert.ToString(id))
                    .ToListAsync()
                    .Result;

            return documents[0];
        }


        // ------------------------------------------------------------------------------
        // Funcion: buscar y retornar todos los equipos (GetReport) de todos los departamentos
        //          de la base de datos mongo.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<GetReportMongo> teamsMongo()
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            List<GetReportMongo> teams = new List<GetReportMongo>();

            List<DepartmentMongo> documents = departments.Find("{}").ToListAsync().Result;

            foreach (DepartmentMongo dept in documents)
            {
                teams.AddRange(dept.GetReport);
            }

            return teams;
        }


        // ------------------------------------------------------------------------------
        // Funcion: buscar y retornar un equipo (GetReport) especifico de un departamento 
        //          especifico de la base de datos mongo.
        // Parametros:
        //          idTeam  -> identificador del equipo (GetReport) que se desea buscar.
        //          idDept  -> identificador del departamento al que pertenece el equipo
        //                     que se desea buscar.
        // ------------------------------------------------------------------------------
        public GetReportMongo searchTeamMongo(int idTeam, int idDept)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            GetReportMongo team = new GetReportMongo();

            List<DepartmentMongo> documents = departments
                    .Find(a => a.Id_Dept == Convert.ToString(idDept))
                    .ToListAsync()
                    .Result;

            foreach (DepartmentMongo dept in documents)
            {
                foreach (GetReportMongo t in dept.GetReport)
                {
                    if (t.deptID == Convert.ToString(idTeam))
                    {
                        team = t;
                    }
                }
            }

            return team;
        }


        // ------------------------------------------------------------------------------
        // Funcion: buscar y retornar el equipo del cual es responsable un empleado.
        // Parametros:
        //          name    -> nombre del empleado.
        //          email   -> correo electronico del empleado.
        // ------------------------------------------------------------------------------
        public List<string> searchTeamEmployee(string name, string email)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            List<DepartmentMongo> documents = departments.Find("{}").ToListAsync().Result;

            List<string> responsableOf = new List<string>();

            foreach (DepartmentMongo dept in documents)
            {
                if ((dept.Name[0] == name && name != "" && name != null && name != string.Empty))
                {
                    responsableOf.Add(dept.Id_Dept);
                }

                foreach (GetReportMongo team in dept.GetReport)
                {
                    if (team.subName != null)
                    {
                        char[] delimiterChars = { ','};
                        string[] subName = team.subName.Split(delimiterChars);

                        foreach (string s in subName)
                        {
                            if ((s.Trim() == name && name != "" && name != null))
                            {
                                responsableOf.Add(team.deptID);
                            }
                        }
                    }
                }
            }

            return responsableOf;
        }


        // ------------------------------------------------------------------------------
        // Funcion: actualizar la información de un documento especifico (departamento, Department) 
        //          de la coleccion en la base de datos mongo.
        // Parametros: 
        //          idDept              -> identificador del departamento que se va a actualizar.
        //          newResponsable      -> nombre del nuevo responsable del departamento.
        //          newEmail            -> email del nuevo responsable del departamento.
        //          newName             -> nuevo nombre del departamento
        //          oldResponsable      -> nombre del anterior responsable del departamento.
        //          oldEmail            -> email del anterior responsable del departamento.
        //          oldName             -> anterior nombre del departamento.
        //          responsableAction   -> identificador del usuario responsable de actualizar el departamento.
        // ------------------------------------------------------------------------------
        public void updateDepartmentMongo(int idDept, string newResponsable, string oldResponsable, string newEmail, string oldEmail, string newName, string oldName, int responsableAction)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();
            Anviz_Data_BaseEntities employeesConection = Sql();

            // registrando la actualización del departamento en la bitacora.
            employeesConection.SP_DepartmentBK(responsableAction, 1, DateTime.Now, idDept, oldName, oldResponsable, oldEmail);

            employeesConection.SP_UpdateDepartment(idDept, newName);

            string filter = "{ Id_Dept: \"" + Convert.ToString(idDept) + "\" }";
            string update = "{ $set: { Nombre_Dept: \"" + newName + "\", Name:[\"" + newResponsable + "\"], correo:[\"" + newEmail + "\"] } }";

            // Actualizando departamento.
            departments.UpdateOne(filter, update);
        }


        // ------------------------------------------------------------------------------
        // Funcion: actualizar la información de un subdocumento especifico (GetReport) de un
        //          documento de la coleccion en la base de datos mongo.
        // Parametros: 
        //          idDept              -> identificador del departamento al que pertenece el 
        //                              equipo (GetReport) que se va a actualizar.
        //          idTeam              -> identificador del equipo que se va a actualizar.
        //          newResponsable      -> nombre del nuevo responsable del equipo.
        //          newEmail            -> email del nuevo responsable del equipo.
        //          newName             -> nuevo nombre del equipo.
        //          oldSubDept          -> nombre anterior del equipo.
        //          oldSubEmail         -> email del anterior responsable del equipo.
        //          oldSubName          -> nombre del anterior responsable del equipo.
        //          responsableAction   -> identificador del usuario responsable de actualizar el equipo.
        // ------------------------------------------------------------------------------
        public void updateTeamMongo(int idDept, int idTeam, string newResponsable, string newEmail, string newName, string oldSubDept, string oldSubEmail, string oldSubName, int responsableAction)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            Anviz_Data_BaseEntities employeesConection = Sql();

            // registrando la actualización del equipo en la bitacora.
            employeesConection.SP_TeamBK(responsableAction, 1, DateTime.Now, idDept, idTeam, oldSubDept, oldSubEmail, oldSubName);

            employeesConection.SP_UpdateDepartment(idTeam, newName);

            List<DepartmentMongo> documents = departments
                    .Find(a => a.Id_Dept == Convert.ToString(idDept))
                    .ToListAsync()
                    .Result;

            DepartmentMongo dept = documents[0];

            int indexTeam = -1;
            for (int index = 0; index < dept.GetReport.Count(); index++)
            {
                if (dept.GetReport[index].deptID == Convert.ToString(idTeam))
                {
                    indexTeam = index;
                }
            }

            string filter = "{ Id_Dept: \"" + Convert.ToString(idDept) + "\" }";
            string update = "{ $set: { \"GetReport." + indexTeam + ".subName\": \"" + newResponsable + "\", " +
                                     " \"GetReport." + indexTeam + ".subEmail\": \"" + newEmail + "\", " +
                                     " \"GetReport." + indexTeam + ".subdept\": \"" + newName + "\"  } }";

            //Actualizando equipo.
            departments.UpdateOne(filter, update);
        }


        // ------------------------------------------------------------------------------
        // Funcion: agregar un equipo a la base de datos mongo
        // Parametros: 
        //          idDept          -> identificador del departamento al que se va a insertar un equipo.
        //          idTeam          -> identificador del nuevo equipo.
        //          responsable     -> nombre del responsable del equipo.
        //          email           -> email del responsable del equipo.
        //          name            -> nombre del equipo.
        // ------------------------------------------------------------------------------
        public void insertTeamMongo(int idDept, int idTeam, string responsable, string email, string name)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            List<DepartmentMongo> documents = departments
                    .Find(a => a.Id_Dept == Convert.ToString(idDept))
                    .ToListAsync()
                    .Result;

            DepartmentMongo dept = documents[0];
            int index = dept.GetReport.Count();

            string filter = "{ Id_Dept: \"" + Convert.ToString(idDept) + "\" }";
            string update = "{ $set: { \"GetReport." + index + ".subName\": \"" + responsable + "\", " +
                                     " \"GetReport." + index + ".deptID\": \"" + idTeam + "\", " + 
                                     " \"GetReport." + index + ".subEmail\": \"" + email + "\", " +
                                     " \"GetReport." + index + ".subdept\": \"" + name + "\"  } }";

            //insertando equipo.
            departments.UpdateOne(filter, update);
        }


        // ------------------------------------------------------------------------------
        // Funcion: agregar un departamento a la base de datos mongo
        // Parametros: 
        //          dept    ->  departamento que se va a insertar.
        // ------------------------------------------------------------------------------
        public void insertDepartmentMongo(DepartmentMongo dept)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            //insertando departamento.
            departments.InsertOne(dept);
        }


        // ------------------------------------------------------------------------------
        // Funcion: eliminar un subdocumento especifico (GetReport) de un
        //          documento de la coleccion en la base de datos mongo.
        // Parametros: 
        //          idDept              -> identificador del departamento al que pertenece el 
        //                              equipo (GetReport) que se va a eliminar.
        //          idTeam              -> identificador del equipo que se va a eliminar.
        //          responsableAction   -> identificador del responsable de ejecutar la eliminacion.
        // ------------------------------------------------------------------------------
        public void deleteTeamMongo(int idDept, int idTeam, int responsableAction)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();
            Anviz_Data_BaseEntities employeesConection = Sql();

            GetReportMongo team = this.searchTeamMongo(idTeam, idDept);

            // registrando la eliminación del equipo en la bitacora.
            employeesConection.SP_TeamBK(responsableAction, 2, DateTime.Now, idDept, idTeam, team.subdept, team.subEmail, team.subName);


            string filter = "{ Id_Dept: \"" + Convert.ToString(idDept) + "\" }";
            string update = "{ $pull: { \"GetReport\": { \"deptID\" : \"" + idTeam + "\"  } } }";

            //Actualizando equipo.
            departments.UpdateOne(filter, update);
        }


        // ------------------------------------------------------------------------------
        // Funcion: eliminar un departamento a la base de datos mongo
        // Parametros: 
        //          idDept    ->  identificador del departamento que se va a eliminar.
        //          responsableAction   -> identificador del responsable de ejecutar la eliminacion.
        // ------------------------------------------------------------------------------
        public void deleteDepartmentMongo(int idDept, int responsableAction)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();
            Anviz_Data_BaseEntities employeesConection = Sql();

            DepartmentMongo dept = this.searchDepartmentMongo(idDept);

            // registrando la eliminación del departamento en la bitacora.
            employeesConection.SP_DepartmentBK(responsableAction, 2, DateTime.Now, idDept, dept.Nombre_Dept, dept.Name[0], dept.correo[0]);

            string filter = "{ Id_Dept: \"" + Convert.ToString(idDept) + "\" }";

            //insertando departamento.
            departments.DeleteOne(filter);
        }


        // ------------------------------------------------------------------------------
        // Funcion: buscar y retornar el identificador del departamento al que pertenece un
        //          equipo (GetReport) especifico de la base de datos mongo.
        // Parametros:
        //          idTeam  -> identificador del equipo del cual se desea saber a que 
        //                     departamento pertenece.
        // ------------------------------------------------------------------------------
        public int searchDepartmentTeamMongo(int idTeam)
        {
            IMongoCollection<DepartmentMongo> departments = Mongo();

            int idDept = -1;

            List<DepartmentMongo> documents = departments.Find("{}").ToListAsync().Result;

            foreach (DepartmentMongo dept in documents)
            {
                foreach (GetReportMongo team in dept.GetReport)
                {
                    if (team.deptID == Convert.ToString(idTeam))
                    {
                        idDept = Convert.ToInt32(dept.Id_Dept);
                    }
                }
            }
            return idDept;
        }


        // ------------------------------------------------------------------------------
        // Funcion: obtener y retornar todos los empleados de la base de datos SQL. 
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<Employee> employees()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<Employee> employees = employees = (from emp in employeesConection.SP_AllEmployees() select new Employee { Id = Convert.ToInt32(emp.Userid), TeamId = emp.Deptid, Name = emp.Name, Email = emp.Address, ResponsableOf = searchTeamEmployee(emp.Name, emp.Address) }).ToList();
            return employees;
        }


        // ------------------------------------------------------------------------------
        // Funcion: obtener y retornar todos los empleados de la base de datos SQL 
        //          pertenecientes a un equipo especifico.
        // Parametros: 
        //          idDept  -> identificador del equipo al que pertenecen los empleados
        //                      que se desean buscar.
        // ------------------------------------------------------------------------------
        public List<Employee> teamEmployees(int idDept)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<Employee> employees = employees = (from emp in employeesConection.SP_TeamEmployees(idDept) select new Employee { Id = Convert.ToInt32(emp.Userid), TeamId = emp.Deptid, Name = emp.Name, Email = emp.Address }).ToList();
            return employees;
        }


        // ------------------------------------------------------------------------------
        // Funcion: obtener y retornar la información de un empleado especifico de la base de datos SQL.
        // Parametros:
        //          idEmp   -> identificador del empleado que se va a buscar.
        // ------------------------------------------------------------------------------
        public Employee searchEmployee(int idEmp)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<Employee> employee = employee = (from emp in employeesConection.SP_Employee(idEmp) select new Employee { Id = Convert.ToInt32(emp.Userid), TeamId = emp.Deptid, Name = emp.Name, Email = emp.Address }).ToList();
            return employee[0];
        }


        // ------------------------------------------------------------------------------
        // Funcion: actualizar la informacion de un empleado especifico en la base de datos SQL.
        // Parametros: 
        //          idEmp               -> identificador del empleado que se va a actualizar.
        //          idTeam              -> identificador del nuevo equipo al que pertenece el empleado 
        //                                 que se va a actualizar.
        //          name                -> nombre del empleado.
        //          email               -> email del empleado.
        //          idResponsableAction -> reponsable de ejecutar la accion.
        // ------------------------------------------------------------------------------
        public void updateEmployee(int idEmp, int idTeam, string name, string email, int idResponsableAction)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_UpdateEmployee(idEmp, idTeam, name, email, idResponsableAction, DateTime.Now);
        }


        // ------------------------------------------------------------------------------
        // Funcion: eliminar un empleado especifico en la base de datos SQL.
        // Parametros: 
        //          idEmp               -> identificador del empleado que se va a eliminar.
        //          idResponsableAction -> reponsable de ejecutar la accion.
        // ------------------------------------------------------------------------------
        public void deleteEmployee(int idEmp, int idResponsableAction)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_DeleteEmployee(idEmp, idResponsableAction, DateTime.Now);
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los registros de empleados en la bitacora.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<EmployeeBK> allEmployeesBK()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<EmployeeBK> employees = employees = (from emp in employeesConection.SP_AllEmployeesBK() select new EmployeeBK {IdBK = emp.Id, Id = Convert.ToInt32(emp.Userid), TeamId = emp.Deptid, Name = emp.Name, Email = emp.Address, TypeAction = (int)emp.TypeAction, DateAction = (DateTime)emp.DateAction, UserName = emp.UserName }).ToList();
            return employees;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los registros de equipos en la bitacora.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<TeamBK> allTeamsBK()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<TeamBK> teams = teams = (from team in employeesConection.SP_AllTeamsBK() select new TeamBK { IdBK = team.Id, subdept = team.Subdept, deptID = Convert.ToString(team.DeptID), subEmail = team.SubEmail, subName = team.SubName, TypeAction = (int)team.TypeAction, DateAction = (DateTime)team.DateAction, UserName = team.UserName }).ToList();
            return teams;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los registros de departamentos en la bitacora.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<DepartmentBK> allDepartmentsBK()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<DepartmentBK> departments = departments = (from dept in employeesConection.SP_AllDepartmetsBK() select new DepartmentBK { IdBK = dept.Id,  Nombre_Dept = dept.Nombre_Dept, Name = new string[] { dept.Name }, correo = new string[] { dept.Correo }, Id_Dept = Convert.ToString(dept.Id_Dept), TypeAction = (int)dept.TypeAction, DateAction = (DateTime)dept.DateAction, UserName = dept.UserName }).ToList();
            return departments;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los registros de empleados en la bitacora.
        // Parametros: 
        //          empId       -> identificador del empleado al que se generara el reporte.
        //          startDate   -> fecha de inicio del reporte.
        //          endDate     -> fecha de finalización del reporte.
        // ------------------------------------------------------------------------------
        public List<InfoReport> reportEmployees(int empId, DateTime startDate, DateTime endDate)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<InfoReport> reportInfo = new List<InfoReport>();
            
            // Diferencia en dias horas y minutos.
            TimeSpan ts = endDate - startDate;
            // Diferencia en dias.
            int differenceInDays = ts.Days;

            int day = Convert.ToInt32(startDate.DayOfWeek);
            day = day - 1;
            DateTime date = startDate.AddDays(day * (-1));
            while (date < endDate )
            {
                List<InfoReport> reportInfoTemp = (from e in employeesConection.GetEmployes(empId, date.ToString("MM-dd-yyyy")) select new InfoReport {
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

                reportInfo.AddRange(reportInfoTemp); 
                date = date.AddDays(7);
            }

            return reportInfo;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar un registro de un empleado de la bitacora.
        // Parametros: 
        //          idEmpBK       -> identificador del registro en la bitacora de empleados que se va a obtener.
        // ------------------------------------------------------------------------------
        public EmployeeBK searchEmployeeBK(int idEmpBK)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<EmployeeBK> employee = (from emp in employeesConection.SP_SearchEmployeeBK(idEmpBK) select new EmployeeBK { IdBK = emp.Id, Id = Convert.ToInt32(emp.Userid), TeamId = emp.Deptid, Name = emp.Name, Email = emp.Address, TypeAction = (int)emp.TypeAction, DateAction = (DateTime)emp.DateAction, UserName = emp.UserName }).ToList();
            return employee[0];
        }


        // ------------------------------------------------------------------------------
        // Funcion: verifica si aun existe un departamento especifico (0=NO EXISTE 1= EXISTE).
        // Parametros: 
        //          idDept       -> identificador del departamento que se va a verificar.
        // ------------------------------------------------------------------------------
        public bool ExistDepartment(int idDept)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            int count = (from depts in employeesConection.SP_ExistDepartment(idDept) select depts.Value).ToList()[0];
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // ------------------------------------------------------------------------------
        // Funcion: retorna el ultimo estado de un empleado (1=update, 2=delete);
        // Parametros: 
        //          idEmp       -> identificador del empleado del que se va a obtener el ultimo estado.
        // ------------------------------------------------------------------------------
        public int endStateEmployeeBK(int idEmp)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            int state = (from stt in employeesConection.SP_EndEstateEmployeeBK(idEmp) select stt.Value).ToList()[0];
            return state;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar un registro de un equipo de la bitacora.
        // Parametros: 
        //          idTeamBK       -> identificador del registro en la bitacora de equipos que se va a obtener.
        // ------------------------------------------------------------------------------
        public TeamBK searchTeamBK(int idTeamBK)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<TeamBK> teams = (from team in employeesConection.SP_SearchTeamBK(idTeamBK) select new TeamBK { IdBK = team.Id, subdept = team.Subdept, deptID = Convert.ToString(team.DeptID), subEmail = team.SubEmail, subName = team.SubName, TypeAction = (int)team.TypeAction, DateAction = (DateTime)team.DateAction, UserName = team.UserName }).ToList();
            return teams[0];
        }


        // ------------------------------------------------------------------------------
        // Funcion: retorna el ultimo estado de un equipo (1=update, 2=delete);
        // Parametros: 
        //          idTeam       -> identificador del equipo del que se va a obtener el ultimo estado.
        // ------------------------------------------------------------------------------
        public int endStateTeamBK(int idTeam)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            int state = (from stt in employeesConection.SP_EndEstateTeamBK(idTeam) select stt.Value).ToList()[0];
            return state;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar un registro de un departamento de la bitacora.
        // Parametros: 
        //          idDeptBK       -> identificador del registro en la bitacora de departamentos que se va a obtener.
        // ------------------------------------------------------------------------------
        public DepartmentBK searchDepartmentBK(int idDeptBK)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<DepartmentBK> departments = (from dept in employeesConection.SP_SearchDepartmentBK(idDeptBK) select new DepartmentBK { IdBK = dept.Id, Nombre_Dept = dept.Nombre_Dept, Name = new string[] { dept.Name }, correo = new string[] { dept.Correo }, Id_Dept = Convert.ToString(dept.Id_Dept), TypeAction = (int)dept.TypeAction, DateAction = (DateTime)dept.DateAction, UserName = dept.UserName }).ToList();
            return departments[0];
        }


        // ------------------------------------------------------------------------------
        // Funcion: retorna el ultimo estado de un departamento (1=update, 2=delete);
        // Parametros: 
        //          idDept       -> identificador del departamento del que se va a obtener el ultimo estado.
        // ------------------------------------------------------------------------------
        public int endStateDepartmentBK(int idDept)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            int state = (from stt in employeesConection.SP_EndEstateDepartmentBK(idDept) select stt.Value).ToList()[0];
            return state;
        }


        // ------------------------------------------------------------------------------
        // Funcion: revertir la eliminación de un empleado.
        // Parametros: 
        //          idEmpBK     -> identificador del registro en la bitacora de empleados que se va a revertir.
        //          respId      -> identificador del responsable de realizar la accion.
        // ------------------------------------------------------------------------------
        public void rollBackEmployeeDeleted(int respId, int idEmpBK)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_RollBackEmployeeDeleted(respId, DateTime.Now, idEmpBK);
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar los estados de envios de reportes semanales.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<WeeklyReportState> weeklyReportsStates()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<WeeklyReportState> reportsStates = (from rep in employeesConection.SP_AllWeeklyReportsState() select new WeeklyReportState { Id = rep.Id, StateReport = rep.StateReport, PercentReports = rep.PercentReports, StartDate = Convert.ToDateTime(rep.StartDate), EndDate = Convert.ToDateTime(rep.EndDate), TypeError = rep.TypeError }).ToList();
            return reportsStates;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar los estados de envios de reportes semanales.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<WeeklyReportDetail> weeklyReportsDetails(int idReport)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<WeeklyReportDetail> reportsDetails = (from repD in employeesConection.SP_WeeklyReportState(idReport) select new WeeklyReportDetail { Id = repD.Id, TypeReport = repD.TypeReport, IdReceptor = repD.IdReceptor, StateReport = repD.StateReport, TypeError = repD.TypeError, IdWeeklyReportState = repD.IdWeeklyReportState }).ToList();
            return reportsDetails;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los departamentos y subdepartamentos (equipo) de la empresa.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<Department> departments()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<Department> departments = (from dept in employeesConection.SP_AllDepartments() select new Department { Deptid = dept.Deptid, DeptName = dept.DeptName, SupDeptid = dept.SupDeptid }).ToList();
            return departments;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los subdepartamentos (equipo) de un departamento de la empresa.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<Department> searchSubDepartments(int deptId)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<Department> teams = (from dept in employeesConection.SP_SearchSubDepartments(deptId) select new Department { Deptid = dept.Deptid, DeptName = dept.DeptName, SupDeptid = dept.SupDeptid }).ToList();
            return teams;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar un departamento o equipo especifico de la empresa.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public Department searchDepartment(int deptId)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            Department department = (from dept in employeesConection.SP_SearchDepartment(deptId) select new Department { Deptid = dept.Deptid, DeptName = dept.DeptName, SupDeptid = dept.SupDeptid }).ToList()[0];
            return department;
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los empleados que reciben reportes especiales.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<Employee> specialReports()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<Employee> specialReports = (from sr in employeesConection.SP_SpecialReports() select new Employee { Id = Convert.ToInt32(sr.Userid), TeamId = sr.Deptid, Name = sr.Name, Email = sr.Address }).ToList();
            return specialReports;
        }


        // ------------------------------------------------------------------------------
        // Funcion: añadir un nuevo empleado que recibe reportes especiales.
        // Parametros:
        //          idEmp   ->  identificador del empleadp.
        // ------------------------------------------------------------------------------
        public void addSpecialReports(int idEmp)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_InsertSpecialReport(idEmp);
            
        }


        // ------------------------------------------------------------------------------
        // Funcion: eliminar reporte especial para un empleado.
        // Parametros:
        //          idEmp   ->  identificador del empleadp.
        // ------------------------------------------------------------------------------
        public void removeSpecialReports(int idEmp)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_DeleteSpecialReport(idEmp);

        }


        // ------------------------------------------------------------------------------
        // Funcion: agregar un nuevo departamento a la base de datos.
        // Parametros:
        //          name    ->  nombre del departamento.
        //          idSup   ->  identificador del departamento superior.
        // ------------------------------------------------------------------------------
        public int insertDepartment(string name, int idSup)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            int id = (from dept in employeesConection.SP_InsertDepartment(name, idSup) select Convert.ToInt32(dept.Value)).ToList()[0];
            return id;
        }


        // ------------------------------------------------------------------------------
        // Funcion: eliminar un nuevo departamento de la base de datos SQL.
        // Parametros:
        //          idDept  ->  identificador del departamento que se eliminara.
        // ------------------------------------------------------------------------------
        public void deleteDepartment(int idDept)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_DeleteDepartment(idDept);
        }


        // ------------------------------------------------------------------------------
        // Funcion: actualizar un nuevo departamento de la base de datos SQL.
        // Parametros:
        //          idDept      ->  identificador del departamento que se actualizara.
        //          deptName    ->  nuevo nombre del departamento.
        // ------------------------------------------------------------------------------
        public void updateDepartment(int idDept, string deptName)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_UpdateDepartment(idDept, deptName);
        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los correos a los que se les envia confirmacion de reportes
        //          semanales.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<ConfirmEmail> confirmEmails()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<ConfirmEmail> emails = (from ce in employeesConection.SP_ConfirmEmail() select new ConfirmEmail { Id = ce.Id, Email = ce.email }).ToList();
            return emails;
        }


        // ------------------------------------------------------------------------------
        // Funcion: añadir un nuevo correo al que se le enviara confirmacion de reportes
        //          semanales.
        // Parametros:
        //          email   ->  email a agregar.
        // ------------------------------------------------------------------------------
        public void addconfirmEmails(string email)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_InsertConfirmEmail(email);

        }


        // ------------------------------------------------------------------------------
        // Funcion: eliminar correo al que se le enviara confirmacion de reportes
        //          semanales.
        // Parametros:
        //          id   ->  identificador del correo.
        // ------------------------------------------------------------------------------
        public void removeconfirmEmails(int id)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_DeleteConfirmEmail(id);

        }


        // ------------------------------------------------------------------------------
        // Funcion: retornar todos los usuarios del sistema.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public List<User> users()
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            List<User> users = (from user in employeesConection.SP_AllUsers() select new User { UserId = user.UserId, UserName = user.UserName }).ToList();
            return users;
        }

        // ------------------------------------------------------------------------------
        // Funcion: retornar un usuario del sistema.
        // Parametros: 
        //             userId   -> identificador del usuario
        // ------------------------------------------------------------------------------
        public User searchUser(int userId)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            User user = (from us in employeesConection.SP_SearchUsers(userId) select new User { UserId = us.UserId, UserName = us.UserName }).ToList()[0];
            return user;
        }

        // ------------------------------------------------------------------------------
        // Funcion: eliminar un usuario del sistema.
        // Parametros: 
        //             userId   -> identificador del usuario
        // ------------------------------------------------------------------------------
        public void deleteUser(int userId)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            employeesConection.SP_DeleteUser(userId);
        }

        // ------------------------------------------------------------------------------
        // Funcion: realizar marcaje de un empleado.
        // Parametros: 
        //              userId      -> identificador del usuario.
        //              checkTime   -> fecha y hora del marcaje.
        //              checkType   -> tipo de marcaje: entrada o salida.
        //              place       -> marcaje presencial o remoto.
        // ------------------------------------------------------------------------------
        public void checkAttendance(int userId, DateTime checkTime, string checkType, string place)
        {
            Anviz_Data_BaseEntities employeesConection = Sql();
            try
            {
                employeesConection.SP_Attendance(userId, checkTime, checkType, place);
            }
            catch(Exception)
            {
                throw new Exception("Registro ya existe.");
            }
        }
    }
}