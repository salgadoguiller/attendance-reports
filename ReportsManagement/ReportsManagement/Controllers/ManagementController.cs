 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportsManagement.Models;


namespace ReportsManagement.Controllers
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de brindar toda la funcionalidad relacionada 
    //          con la gestión de empleados, equipos y departamentos.
    // Propiedades: no tiene ninguna propiedad.
    // Metodos: tiene una serie de metodos con una descripcion detallada.
    // ------------------------------------------------------------------------------

    // ------------------------------------------------------------------------------
    // Descripción de la variable message utilizada en varios de los metodos.
    //      message = 1 -> No hay mensaje que mostrar.
    //      message = 2 -> Exito.
    //      message = 3 -> Error.
    // ------------------------------------------------------------------------------
    [Authorize]
    public class ManagementController : Controller
    {
        // ------------------------------------------------------------------------------
        // GET: /Management/employees
        //      /Management/employees?message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos 
        //          los empleados de la empresa.
        // Parametros: Puede recibir dos parametros o ninguno dependiendo de donde se llame:
        //              Si se redirecciona desde alguna accion del controlador:
        //                      message         -> representa el tipo mensaje de retroalimentación que se mostrara 
        //                                         al usuario dependiendo del escenario que se presente.
        //                      messageValue    -> contiene el mensaje que se mostrara al usuario.
        //              En caso contrario no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult employees(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            // try
            // {
                // Cargando información que se pasara a la vista.
                ViewData["employees"] = conection.employees();
                ViewData["teams"] = conection.departments();
                ViewData["departments"] = conection.departments();
            /*}
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }            
            */
            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/specialReports
        //      /Management/specialReports?message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos 
        //          los empleados de la empresa que reciben reportes especiales.
        // Parametros:
        //              message         -> representa el tipo mensaje de retroalimentación que se mostrara 
        //                                 al usuario dependiendo del escenario que se presente.
        //              messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult specialReports(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["specialReports"] = conection.specialReports();
                ViewData["allEmployees"] = conection.employees();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/addSpecialReport
        // Funcion: accion encargada de agregar un nuevo empleado que recibira reportes especiales.
        // Parametros:
        //          idEmp   -> identificador del empleado.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult addSpecialReport(int idEmp)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                conection.addSpecialReports(idEmp);
                message = 2;
                messageValue = "Reporte especial agregado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Reporte especial no agregado. Por favor intentelo de nuevo.";
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "specialReports",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/removeSpecialReport?idEmp=6
        // Funcion: accion encargada de eliminar reporte especial para un empleado.
        // Parametros:
        //          idEmp   ->  identificador del empleado.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult removeSpecialReport(int idEmp)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Eliminando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                conection.removeSpecialReports(idEmp);
                message = 2;
                messageValue = "Empleado eliminado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Empleado no eliminado. Por favor intentelo de nuevo.";
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "specialReports",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/confirmEmail
        //      /Management/confirmEmail?message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos 
        //          los correos a donde se envia la confirmación del envio de reportes semanal.
        // Parametros:
        //              message         -> representa el tipo mensaje de retroalimentación que se mostrara 
        //                                 al usuario dependiendo del escenario que se presente.
        //              messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult confirmEmails(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["confirmEmails"] = conection.confirmEmails();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/addConfirmEmail
        // Funcion: accion encargada de agregar un nuevo email para enviar la confirmacion de envio 
        //          de los reportes semanales.
        // Parametros:
        //          email   -> email al que se enviaran correos de confirmación.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult addConfirmEmail(string email)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                conection.addconfirmEmails(email);
                message = 2;
                messageValue = "Correo de confirmación agregado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Correo de confirmación no agregado. Por favor intentelo de nuevo.";
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "confirmEmails",
                message = message,
                messageValue = messageValue,
            });
        }

        // ------------------------------------------------------------------------------
        // GET: /Management/removeConfirmEmail?id=6
        // Funcion: accion encargada de eliminar un email al que se envian las confirmaciones de
        //          envio de reportes semanales.
        // Parametros:
        //          id  ->  identificador del correo.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult removeConfirmEmail(int id)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Eliminando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                conection.removeconfirmEmails(id);
                message = 2;
                messageValue = "Correo de confirmación eliminado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Correo de confirmación no eliminado. Por favor intentelo de nuevo.";
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "confirmEmails",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/departments
        //      /Management/departments?message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos los 
        //          departamentos de la empresa.
        // Parametros: Puede recibir dos parametro o ninguno dependiendo de donde se llame:
        //              Si se redirecciona desde alguna accion del controlador
        //                      message         -> representa el tipo de mensaje de retroalimentación que se 
        //                                         mostrara al usuario dependiendo del escenario que se presente.
        //                      messageValue    -> contiene el mensaje que se mostrara al usuario.
        //              En caso contrario no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult departments(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["departments"] = conection.departments();
                ViewData["departmentsMongo"] = conection.departmentsMongo();
                ViewData["allEmployees"] = conection.employees();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/employeesDepartment?idDept=6
        //      /Management/employeesDepartment?idDept=6&message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos los empleados 
        //          de un departamento especifico de la empresa.
        // Parametros: 
        //          idDept          -> identificador de departamento.
        //          message         -> parametro opcional que representa el tipo de mensaje de retroalimentación que se 
        //                             mostrara al usuario dependiendo del escenario que 
        //                             se presente.
        //          messageValue    -> parametro opcional que contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult employeesDepartment(int idDept, int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["dept"] = conection.searchDepartment(idDept);
                ViewData["deptMongo"] = conection.searchDepartmentMongo(idDept);
                List<Department> teams = conection.searchSubDepartments(idDept);
                List<Department> teams2 = new List<Department>();
                List<Employee> employees = new List<Employee>();
                foreach(Department team in teams)
                {
                    teams2.AddRange(conection.searchSubDepartments(team.Deptid));
                }
                teams2.AddRange(teams);

                foreach (Department team in teams2)
                {
                    employees.AddRange(conection.teamEmployees(team.Deptid));
                }
                employees.AddRange(conection.teamEmployees(idDept));
                ViewData["employees"] = employees;
                ViewData["allEmployees"] = conection.employees();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/department?idDept=3
        //      /Management/department?idDept=3&message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos los 
        //          equipos de un departamento especifico de la empresa.
        // Parametros: Puede recibir un parametro o tres dependiendo de donde se llame al controlador:
        //              Si se redirecciona desde una accion del controlador:
        //                      idDept          -> identificador del departamento.
        //                      message         -> representa el tipo de mensaje de retroalimentación que se 
        //                                          mostrara al usuario dependiendo del escenario que 
        //                                          se presente.
        //                      messageValue    -> contiene el mensaje que se mostrara al usuario.
        //              En caso contrario:
        //                      idDept          -> identificador del departamento.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult department(int idDept, int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["dept"] = conection.searchDepartment(idDept);
                ViewData["department"] = conection.searchSubDepartments(idDept);
                ViewData["departmentMongo"] = conection.searchDepartmentMongo(idDept);
                ViewData["allEmployees"] = conection.employees();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/team?idTeam=6
        //      /Management/team?idTeam=6&idDept=3&message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos los empleados 
        //          de un equipo especifico de un departamento de la empresa.
        // Parametros: Puede recibir un parametro o tres dependiendo de donde se llame al controlador:       
        //              Si se redirecciona desde una accion del controlador:
        //                          idTeam          -> identificador de equipo.
        //                          message         -> representa el tipo de mensaje de retroalimentación que se 
        //                                              mostrara al usuario dependiendo del escenario que 
        //                                              se presente.
        //                          messageValue    -> contiene el mensaje que se mostrara al usuario. 
        //              En caso contrario:
        //                          idTeam          -> identificador de equipo.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult team(int idTeam, int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["team"] = conection.searchDepartment(idTeam);
                ViewData["teamMongo"] = conection.searchTeamMongo(idTeam, conection.searchDepartmentTeamMongo(idTeam));
                List<Department> teams = conection.searchSubDepartments(idTeam);
                List<Department> teams2 = new List<Department>();
                if (teams.Count == 0)
                {
                    ViewData["employees"] = conection.teamEmployees(idTeam);
                }
                else
                {
                    List<Employee> employees = new List<Employee>();
                    foreach (Department team in teams)
                    {
                        teams2.AddRange(conection.searchSubDepartments(team.Deptid));
                    }
                    teams2.AddRange(teams);
                    foreach (Department team in teams2)
                    {
                        employees.AddRange(conection.teamEmployees(team.Deptid));
                    }
                    employees.AddRange(conection.teamEmployees(idTeam));
                    ViewData["employees"] = employees;
                }
                ViewData["allEmployees"] = conection.employees();                
            }
            catch(Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/subTeams?idTeam=3
        //      /Management/subTeams?idTeam=3&message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos los 
        //          subequipos de un equipo especifico de la empresa.
        // Parametros: 
        //          idTeam          -> identificador del equipo.
        //          message         -> parametro opcional que representa el tipo de mensaje de retroalimentación que se 
        //                             mostrara al usuario dependiendo del escenario que 
        //                             se presente.
        //          messageValue    -> parametro opcional que contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult subTeams(int idTeam, int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["allEmployees"] = conection.employees();
                ViewData["team"] = conection.searchDepartment(idTeam);
                ViewData["subTeams"] = conection.searchSubDepartments(idTeam);
                ViewData["teamMongo"] = conection.searchTeamMongo(idTeam, conection.searchDepartmentTeamMongo(idTeam));
                try
                {
                    ViewData["teamsMongo"] = conection.searchDepartmentMongo(conection.searchDepartment(idTeam).SupDeptid);
                }
                catch (Exception e)
                {
                    ViewData["teamsMongo"] = new DepartmentMongo();
                }
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/editEmployee?id=6&isTeam=1
        // Funcion: accion encargada de presentar la vista donde se editara la informacion de un empleado.
        // Parametros:
        //          id      -> identificador del empleado.
        //          isTeam  -> indica de donde se llamo esta accion (view employees o view team).       
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult editEmployee(int id, int isTeam)
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre un error.
            ViewData["isTeam"] = isTeam;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["teams"] = conection.departments();
                ViewData["employee"] = conection.searchEmployee(id);
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/editEmployee
        // Funcion: accion encargada de actualizar en la base de datos la informacion 
        //          referente a un empleado.
        // Parametros:
        //          id          -> identificador del empleado que se esta editando.
        //          team        -> identificador del nuevo equipo al que pertenecera el empleado.
        //          isTeam      -> indica desde donde se llama a esta accion (desde la vista employees o desde la vista team) 
        //                         con el objetivo de saber hacia que controlador redireccionar (employees o team) luego de actualizar un empleado.
        //          name        -> nombre del empleado que se esta editando.
        //          email       -> email del empleado que se esta editando.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult editEmployee(int id,  int team, int isTeam, string name, string email)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                conection.updateEmployee(id, team, name, email, AccountController.getIdUser());
                message = 2;
                messageValue = "Empleado actualizado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Empleado no actualizado. Por favor intentelo de nuevo.";
            }

            // Decidiendo hacia que controlador redirigir el flujo de ejecución.
            if (isTeam == 1)
            {
                int department;
                try
                {
                    department = conection.searchDepartmentTeamMongo(team);
                }
                catch(Exception e)
                {
                    department = -1;
                }

                if (department != -1)
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "team",
                        idTeam = team,
                        message = message,
                        messageValue = messageValue,
                    });
                }
                else
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "departments",
                        message = message,
                        messageValue = messageValue,
                    });
                }
            }
            else
            {
                if(isTeam == 2)
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "employeesDepartment",
                        idDept = conection.searchDepartment(team).SupDeptid,
                        message = message,
                        messageValue = messageValue,
                    });
                }
                else
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "employees",
                        message = message,
                        messageValue = messageValue,
                    });
                }
            }
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/deleteEmployee?id=6&isTeam=1&idTeam=3
        // Funcion: accion encargada de eliminar de la base de datos un empleado
        // Parametros:
        //          id          ->  identificador del empleado que se esta eliminando.
        //          isTeam      ->  indica de donde se llamo esta accion (view employees o view team).
        //          idTeam      ->  parametro opcional que representa el identificador del equipo al que pertenece
        //                          el empleado que se va a eliminar.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult deleteEmployee(int id, int isTeam, int idTeam = -1)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Eliminando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                conection.deleteEmployee(id, AccountController.getIdUser());
                message = 2;
                messageValue = "Empleado eliminado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Empleado no eliminado. Por favor intentelo de nuevo.";
            }

            // Decidiendo hacia que controlador redirigir el flujo de ejecución.
            if (isTeam == 1)
            {
                int idDept;
                try
                {
                    idDept = conection.searchDepartmentTeamMongo(idTeam);
                }
                catch(Exception e)
                {
                    idDept = -1;
                }
                if(idDept != -1)
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "team",
                        idTeam = idTeam,
                        message = message,
                        messageValue = messageValue,
                    });
                }
                else
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "departments",
                        message = message,
                        messageValue = messageValue,
                    });
                }
            }
            else
            {
                if(isTeam == 2)
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "employeesDepartment",
                        idDept = conection.searchDepartment(idTeam).Deptid,
                        message = message,
                        messageValue = messageValue,
                    });
                }
                else
                {
                    return RedirectToRoute(new
                    {
                        controller = "Management",
                        action = "employees",
                        message = message,
                        messageValue = messageValue,
                    });
                }
            }
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/addEmployeeTeam
        // Funcion: accion encargada de agregar un empleado a un equipo especifico.
        // Parametros:
        //          idEmp   -> identificador del empleado que se esta editando.
        //          team    -> identificador del nuevo equipo al que pertenecera el empleado.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult addEmployeeTeam(int idEmp, int idTeam, string isTeam)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                Employee emp = conection.searchEmployee(idEmp);
                conection.updateEmployee(idEmp, idTeam, emp.Name, emp.Email, AccountController.getIdUser());
                message = 2;
                messageValue = "Empleado agregado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Empleado no agregado. Por favor intentelo de nuevo.";
            }
            if (isTeam == "team")
            {
                // Redireccionando el flujo de ejecucion.
                return RedirectToRoute(new
                {
                    controller = "Management",
                    action = "team",
                    idTeam = idTeam,
                    message = message,
                    messageValue = messageValue,
                });
            }
            else
            {
                // Redireccionando el flujo de ejecucion.
                return RedirectToRoute(new
                {
                    controller = "Management",
                    action = "employeesDepartment",
                    idDept = idTeam,
                    message = message,
                    messageValue = messageValue,
                });
            }
            
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/editTeam?IdTeam=6&IsDept=1
        // Funcion: accion encargada de presentar la vista donde se editara la 
        //          informacion de un equipo.
        // Parametros (recibe dos parametros mediante un objeto de la clase EditTeam):
        //          IdTeam  -> identificador del equipo.
        //          IsDept  -> indica de donde se llamo esta accion (view departments o view teams).
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult editTeam(int idTeam, int isDept)
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no un error.
            ViewData["error"] = false;
            ViewData["isDept"] = isDept;

            try
            {
                // Cargando información que se pasara a la vista.
                int idDept = conection.searchDepartmentTeamMongo(idTeam);
                ViewData["team"] = conection.searchDepartment(idTeam);
                ViewData["teamMongo"] = conection.searchTeamMongo(idTeam, idDept);
                ViewData["teamEmployees"] = conection.teamEmployees(idTeam);
                ViewData["allEmployees"] = conection.employees();
                ViewData["department"] = idDept;
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }



        // ------------------------------------------------------------------------------
        // POST: /Management/editTeam
        // Funcion: accion encargada de actualizar en la base de datos la informacion 
        //          referente a un equipo.
        // Parametros:
        //          idTeam      -> identificador del equipo que se va a editar.
        //          idDept      -> identificador del departamento al que pertenece el equipo.
        //          name        -> nuevo nombre del equipo.
        //          oldSubDept  -> nombre actual del equipo (se editara).
        //          oldSubEmail -> email del responsable actual del equipo (se editara).
        //          oldSubName  -> nombre del responsable actual del equipo (se editara).
        //          isDept      -> indica de donde se llamo esta accion (view departments o view teams).
        //          options     -> representa la opción seleccionada para el nuevo responsable:
        //                              employee    -> nuevo responsable sera un empleado.
        //                              other       -> nuevo responsable no sera un empleado.
        //                              anyone      -> ningun responsable.
        //          employee    -> parametro opcional que representa el identificador del nuevo responsable del departamento.
        //          reasign     -> parametro opcional que indica si se va a reasignar el responsable o no.
        //          responsable     -> nombre del nuevo responsable del departamento.
        //          email           -> email del nuevo responsable del departamento.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult editTeam(int idTeam, int idDept, string name, string oldSubDept, string oldSubEmail, string oldSubName, int isDept, string options, int employee = -1, string reasign = "no-reasign", string responsable = "", string email = "")
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar.
            int message;
            string messageValue;
            int idDept2 = -1;

            // Actualizando equipo en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                idDept2 = conection.searchDepartmentTeamMongo(idTeam);
                conection.updateDepartment(idTeam, name);

                switch (options)
                {
                    case "employee":
                        Employee emp = conection.searchEmployee(employee);
                        if (reasign == "reasign")
                        {
                            conection.updateEmployee(emp.Id, idTeam, emp.Name, emp.Email, AccountController.getIdUser());
                        }
                        conection.updateTeamMongo(idDept2, idTeam, emp.Name, emp.Email, name, oldSubDept, oldSubEmail, oldSubName, AccountController.getIdUser());
                        break;
                    case "other":
                        conection.updateTeamMongo(idDept2, idTeam, responsable, email, name, oldSubDept, oldSubEmail, oldSubName, AccountController.getIdUser());
                        break;
                    case "anyone":
                        conection.updateTeamMongo(idDept2, idTeam, "", "", name, oldSubDept, oldSubEmail, oldSubName, AccountController.getIdUser());
                        break;
                }

                message = 2;
                messageValue = "Equipo actualizado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Equipo no actualizado. Por favor intentelo de nuevo.";
            }                        

            // Decidiendo hacia donde redirigir el flujo de ejecución.
            if (isDept == 0)
            {
                return RedirectToRoute(new
                {
                    controller = "Management",
                    action = "subTeams",
                    idTeam = idDept,
                    message = message,
                    messageValue = messageValue,
                });
            }
            else
            {
                return RedirectToRoute(new
                {
                    controller = "Management",
                    action = "department",
                    idDept = conection.searchDepartmentTeamMongo(idTeam),
                    message = message,
                    messageValue = messageValue,
                });
            }
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/editDepartment?idDept=6
        // Funcion: accion encargada de presentar la vista donde se editara la 
        //          informacion de un departamento.
        // Parametros:
        //          idDept  -> identificador de un departamento
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult editDepartment(int idDept)
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no un error.
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["department"] = conection.searchDepartment(idDept);
                ViewData["departmentMongo"] = conection.searchDepartmentMongo(idDept);
                ViewData["allEmployees"] = conection.employees();
            }
            catch(Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/editDepartment
        // Funcion: accion encargada de actualizar en la base de datos la informacion 
        //          referente a un departamento.
        // Parametros:
        //          idDept          -> identificador del departamento que se va a editar.
        //          name            -> nuevo nombre del departamento.
        //          oldResponsable  -> nombre del responsable actual del departamento (se editara).
        //          oldEmail        -> correo del responsable aactual del departamento (se editara).
        //          oldName         -> nombre actual del departamento (se editara).
        //          options         -> representa la opción seleccionada para el nuevo responsable:
        //                              employee    -> nuevo responsable sera un empleado.
        //                              other       -> nuevo responsable no sera un empleado.
        //                              anyone      -> ningun responsable.
        //          employee        -> parametro opcional que representa el identificador del nuevo responsable del departamento.
        //          reasign         -> parametro opcional que indica si se va a reasignar el responsable o no.
        //          responsable     -> nombre del nuevo responsable del departamento.
        //          email           -> email del nuevo responsable del departamento.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult editDepartment(int idDept, string name, string oldResponsable, string oldEmail, string oldName, string options, int employee = -1, string reasign = "no-reasign", string responsable = "", string email = "")
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar
            int message;
            string messageValue;

            // Actualizando departamento en la base de datos
            // Manejando cualquier excepcion que se presente
            try
            {
                conection.updateDepartment(idDept, name);
                switch (options)
                {
                    case "employee":
                        Employee emp = conection.searchEmployee(employee);
                        if (reasign == "reasign")
                        {
                            conection.updateEmployee(emp.Id, idDept, emp.Name, emp.Email, AccountController.getIdUser());
                        }
                        conection.updateDepartmentMongo(idDept, emp.Name, oldResponsable, emp.Email, oldEmail, name, oldName, AccountController.getIdUser());
                        break;
                    case "other":
                        conection.updateDepartmentMongo(idDept, responsable, oldResponsable, email, oldEmail, name, oldName, AccountController.getIdUser());
                        break;
                    case "anyone":
                        conection.updateDepartmentMongo(idDept, "", oldResponsable, "", oldEmail, name, oldName, AccountController.getIdUser());
                        break;
                }

                message = 2;
                messageValue = "Departamento actualizado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Departamento no actualizado. Por favor intentelo de nuevo.";
                messageValue = e.Message;
            }

            // Redirigiendo el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "departments",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/addDepartment
        // Funcion: accion encargada de agregar un nuevo departamento.
        // Parametros:
        //          name            -> nombre del nuevo departamento.
        //          idResponsable   -> identificador del responsable del departamento.
        //          empty           -> indica si el nuevo departamento no tendra responsable.
        //          reasign         -> indica si el responsable del departamento se reasignara.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult addDepartment(string name, string empty, string reasign = "no-reasign", int idResponsable = -1)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                // Insertando departamento en SQL Server.
                int id = conection.insertDepartment(name, 1);
                DepartmentMongo dept = new DepartmentMongo();
                dept.Nombre_Dept = name;
                dept.Id_Dept = Convert.ToString(id);
                dept.GetReport = new GetReportMongo[] {};

                if (empty != "empty")
                {
                    Employee emp = conection.searchEmployee(idResponsable);

                    // Reasignando empleado.
                    if (reasign == "reasign")
                    {
                        conection.updateEmployee(idResponsable, id, emp.Name, emp.Email, AccountController.getIdUser());
                    }
                    dept.Name = new string[] { emp.Name };
                    dept.correo = new string[] {emp.Email};
                }
                else
                {
                    dept.Name = new string[] { "" };
                    dept.correo = new string[] { "" };
                }

                // Insertando departamento en MongoDB
                conection.insertDepartmentMongo(dept);
                conection.insertTeamMongo(id, id, "", "", name);

                message = 2;
                messageValue = "Departamento agregado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Departamento no agregado. Por favor intentelo de nuevo.";
                // messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "departments",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/addTeam
        // Funcion: accion encargada de agregar un nuevo equipo.
        // Parametros:
        //          IdSupDept       -> identificador del departamento al que pertenece el nuevo equipo.
        //          name            -> nombre del nuevo equipo.
        //          idResponsable   -> identificador del responsable del equipo.
        //          empty           -> indica si el nuevo equipo no tendra responsable.
        //          reasign         -> indica si el responsable del equipo se reasignara.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult addTeam(int idSupDept, string name, string empty, string reasign = "no-reasign", int idResponsable = -1)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                // Insertando equipo en SQLServer.
                int id = conection.insertDepartment(name, idSupDept);
                DepartmentMongo dept = new DepartmentMongo();
                dept.Nombre_Dept = name;
                dept.Id_Dept = Convert.ToString(id);
                dept.GetReport = new GetReportMongo[] { };

                if (empty != "empty")
                {
                    Employee emp = conection.searchEmployee(idResponsable);

                    // Reasignando empleado.
                    if (reasign == "reasign")
                    {
                        conection.updateEmployee(idResponsable, id, emp.Name, emp.Email, AccountController.getIdUser());
                    }

                    dept.Name = new string[] { emp.Name };
                    dept.correo = new string[] { emp.Email };
                }
                else
                {
                    dept.Name = new string[] { "" };
                    dept.correo = new string[] { "" };
                }

                // Insertando Equipo en MongoDB.
                conection.insertTeamMongo(idSupDept, id, dept.Name[0], dept.correo[0], name);

                message = 2;
                messageValue = "Equipo agregado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Equipo no agregado. Por favor intentelo de nuevo.";
                // messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "department",
                idDept = idSupDept,
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/addSubTeam
        // Funcion: accion encargada de agregar un nuevo subequipo.
        // Parametros:
        //          idSupTeam       -> identificador del equipo al que pertenece el nuevo subequipo.
        //          name            -> nombre del nuevo subequipo.
        //          idResponsable   -> identificador del responsable del subequipo.
        //          empty           -> indica si el nuevo subequipo no tendra responsable.
        //          reasign         -> indica si el responsable del subequipo se reasignara.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult addSubTeam(int idSupTeam, string name, string empty, string reasign = "no-reasign", int idResponsable = -1)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                // Insertando subequipo en SQLServer.
                int id = conection.insertDepartment(name, idSupTeam);
                DepartmentMongo dept = new DepartmentMongo();
                dept.Nombre_Dept = name;
                dept.Id_Dept = Convert.ToString(id);
                dept.GetReport = new GetReportMongo[] { };

                if (empty != "empty")
                {
                    Employee emp = conection.searchEmployee(idResponsable);

                    // Reasignando empleado.
                    if (reasign == "reasign")
                    {
                        conection.updateEmployee(idResponsable, id, emp.Name, emp.Email, AccountController.getIdUser());
                    }

                    dept.Name = new string[] { emp.Name };
                    dept.correo = new string[] { emp.Email };
                }
                else
                {
                    dept.Name = new string[] { "" };
                    dept.correo = new string[] { "" };
                }

                // Insertando subequipo en MongoDB.
                Department department = conection.searchDepartment(idSupTeam);
                conection.insertTeamMongo(department.SupDeptid, id, dept.Name[0], dept.correo[0], name);

                message = 2;
                messageValue = "Sub-Equipo agregado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Sub-Equipo no agregado. Por favor intentelo de nuevo.";
                // messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "subTeams",
                idTeam = idSupTeam,
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/removeDepartment
        // Funcion: accion encargada de eliminar un departamento.
        // Parametros:
        //          IdDept      -> identificador del departamento que se va a eliminar.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult removeDepartment(int idDept)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                List<Department> teamsDeleted = new List<Department>();
                List<Employee> employees = conection.employees();
                List<Department> departments = conection.departments();

                // Eliminando todos los equipos y actualizando empleados.
                foreach(Department team in departments)
                {
                    if (team.SupDeptid == idDept)
                    {
                        teamsDeleted.Add(team);
                        conection.deleteDepartment(team.Deptid);
                        conection.deleteTeamMongo(idDept, team.Deptid, AccountController.getIdUser());
                        foreach (Employee emp in employees)
                        {
                            if (emp.TeamId == team.Deptid)
                            {
                                conection.updateEmployee(emp.Id, 1, emp.Name, emp.Email, AccountController.getIdUser());
                            }
                        }
                    }
                }

                // Eliminando todos los subequipos y actualizando empleados.
                foreach (Department subTeam in departments)
                {
                    foreach(Department deptDel in teamsDeleted)
                    {
                        if (subTeam.SupDeptid == deptDel.Deptid)
                        {
                            conection.deleteDepartment(subTeam.Deptid);
                            conection.deleteTeamMongo(idDept, subTeam.Deptid, AccountController.getIdUser());
                            foreach (Employee emp in employees)
                            {
                                if (emp.TeamId == subTeam.Deptid)
                                {
                                    conection.updateEmployee(emp.Id, 1, emp.Name, emp.Email, AccountController.getIdUser());
                                }
                            }
                        }
                    }
                }

                // Eliminando departamento y actualizando empleados.
                conection.deleteDepartment(idDept);
                conection.deleteDepartmentMongo(idDept, AccountController.getIdUser());
                foreach (Employee emp in employees)
                {
                    if (emp.TeamId == idDept)
                    {
                        conection.updateEmployee(emp.Id, 1, emp.Name, emp.Email, AccountController.getIdUser());
                    }
                }
                
                message = 2;
                messageValue = "Departamento eliminado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Departamento no eliminado. Por favor intentelo de nuevo.";
                // messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "departments",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/removeTeam
        // Funcion: accion encargada de eliminar un equipo.
        // Parametros:
        //          IdSupDept   -> identificador del departamento al que pertenece el equipo.
        //          IdTeam      -> identificador del equipo que se va a eliminar.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult removeTeam(int idSupDept, int idTeam)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                List<Employee> employees = conection.employees();
                List<Department> departments = conection.departments();

                // Eliminando equipo y actualizando empleados.
                conection.deleteDepartment(idTeam);
                conection.deleteTeamMongo(idSupDept, idTeam, AccountController.getIdUser());
                foreach (Employee emp in employees)
                {
                    if (emp.TeamId == idTeam)
                    {
                        conection.updateEmployee(emp.Id, idSupDept, emp.Name, emp.Email, AccountController.getIdUser());
                    }
                }

                // Eliminando todos los subequiposy actualizando empleados.
                foreach (Department subTeam in departments)
                {
                    if (subTeam.SupDeptid == idTeam)
                    {
                        conection.deleteDepartment(subTeam.Deptid);
                        conection.deleteTeamMongo(idSupDept, subTeam.Deptid, AccountController.getIdUser());
                        foreach (Employee emp in employees)
                        {
                            if (emp.TeamId == subTeam.Deptid)
                            {
                                conection.updateEmployee(emp.Id, idSupDept, emp.Name, emp.Email, AccountController.getIdUser());
                            }
                        }
                    }
                }

                message = 2;
                messageValue = "Equipo eliminado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Equipo no eliminado. Por favor intentelo de nuevo.";
                // messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "department",
                idDept = idSupDept,
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/removeSubTeam
        // Funcion: accion encargada de eliminar un departamento.
        // Parametros:
        //          IdSupDept   -> identificador del departamento al que pertenece el subequipo.
        //          idSupTeam   -> identificador del equipo al que pertenece el subequipo
        //          IdTeam      -> identificador del subequipo que se va a eliminar. 
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult removeSubTeam(int idSupDept, int idSupTeam, int idTeam)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Actualizando empleado en la base de datos.
            // Manejando cualquier excepcion que se presente.
            try
            {
                List<Employee> employees = conection.employees();

                // Eliminando subequipo y actualizando empleados.
                conection.deleteDepartment(idTeam);
                conection.deleteTeamMongo(idSupDept, idTeam, AccountController.getIdUser());
                foreach (Employee emp in employees)
                {
                    if (emp.TeamId == idTeam)
                    {
                        conection.updateEmployee(emp.Id, idSupTeam, emp.Name, emp.Email, AccountController.getIdUser());
                    }
                }                

                message = 2;
                messageValue = "Sub-Equipo eliminado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Sub-Equipo  no eliminado. Por favor intentelo de nuevo.";
                // messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "subTeams",
                idTeam = idSupTeam,
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/employeeVerify
        // Funcion: accion encargada de verificar si al intentar actualizar un empleado
        //          se han realizado cambios.
        // Parametros:
        //          idEmp   -> identificador del empleado que se va a verificar.
        //          name    -> nombre del empleado.
        //          email   -> correo electronico del empleado.
        //          team    -> identificador del equipo al que pertenece el empleado.
        // Retorna:
        //          int -> representa si existe algun cambio al intentar actualizar un empleado
        //                 (0 = no hay cambio; 1 = si hay cambio).
        // ------------------------------------------------------------------------------
        [HttpPost]
        public int employeeVerify(int idEmp, string name, string email, int team)
        {
            Conection conection = new Conection();
            int response = 1;
            try
            {
                Employee emp = conection.searchEmployee(idEmp);
                if (emp.Name == name && emp.Email == email && emp.TeamId == team)
                {
                    response = 0;
                }
            }
            catch (Exception e)
            {

            }

            return response;
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/teamVerify
        // Funcion: accion encargada de verificar si al intentar actualizar un equipo
        //          se han realizado cambios.
        // Parametros:
        //          idDept      -> identificador del departamento al que pertenece el equipo.
        //          idTeam      -> identificador del equipo que se va a verificar.
        //          name        -> nombre del equipo.
        //          employee    -> identificador del responsable del equipo.
        //          empty       -> representa si el equipo no tendra responsable.
        // Retorna:
        //          int -> representa si existe algun cambio al intentar actualizar un empleado
        //                 (0 = no hay cambio; 1 = si hay cambio).
        // ------------------------------------------------------------------------------
        [HttpPost]
        public int teamVerify(int idDept, int idTeam, string name, string empty, int employee)
        {
            Conection conection = new Conection();
            int response = 1;
            try
            {
                string nameEmp = "";
                string emailEmp = "";

                if (empty != "empty")
                {
                    Employee emp = conection.searchEmployee(employee);
                    nameEmp = emp.Name;
                    emailEmp = emp.Email;
                }

                GetReportMongo teamMongo = conection.searchTeamMongo(idTeam, conection.searchDepartmentTeamMongo(idTeam));
                Department team = conection.searchDepartment(idTeam);
                if (team.DeptName == name && teamMongo.subName == nameEmp && teamMongo.subEmail == emailEmp)
                {
                    response = 0;
                }
            }
            catch (Exception e)
            {

            }

            return response;
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/departmentVerify
        // Funcion: accion encargada de verificar si al intentar actualizar un departamento
        //          se han realizado cambios.
        // Parametros:
        //          idDept      -> identificador del departamento que se va a verificar.
        //          name        -> nombre del departamento.
        //          employee    -> identificador del responsable del departamento.
        //          empty       -> representa si el departamento no tendra responsable.
        // Retorna:
        //          int -> representa si existe algun cambio al intentar actualizar un empleado
        //                 (0 = no hay cambio; 1 = si hay cambio).
        // ------------------------------------------------------------------------------
        [HttpPost]
        public int departmentVerify(int idDept, string name, string empty, int employee)
        {
            Conection conection = new Conection();
            int response = 1;
            try
            {
                string nameEmp = "";
                string emailEmp = "";

                if (empty != "empty")
                {
                    Employee emp = conection.searchEmployee(employee);
                    nameEmp = emp.Name;
                    emailEmp = emp.Email;
                }

                DepartmentMongo deptMongo = conection.searchDepartmentMongo(idDept);
                Department dept = conection.searchDepartment(idDept);
                if (dept.DeptName == name && deptMongo.Name[0] == nameEmp && deptMongo.correo[0] == emailEmp)
                {
                    response = 0;
                }
            }
            catch (Exception e)
            {

            }

            return response;
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/subDepartmentsVerify
        // Funcion: accion encargada de verificar si un departamento tiene subdepartamentos (equipos).
        // Parametros:
        //          idDept      -> identificador del departamento que se va a verificar.
        // Retorna:
        //          int -> representa si existen subdepartamentos en el departamento que se esta verificando
        //                 (0 = no hay; 1 = si hay).
        // ------------------------------------------------------------------------------
        [HttpPost]
        public int subDepartmentsVerify(int idDept)
        {
            Conection conection = new Conection();
            int response = 1;
            try
            {
                List<Department> department = conection.searchSubDepartments(idDept);
                if (department.Count() < 1)
                {
                    response = 0;
                }
            }
            catch (Exception e)
            {

            }

            return response;
        }


        // ------------------------------------------------------------------------------
        // POST: /Management/haveEmployeesVerify
        // Funcion: accion encargada de verificar si un departamento tiene empleados.
        // Parametros:
        //          idDept      -> identificador del departamento que se va a verificar.
        // Retorna:
        //          int -> representa si existen empleados en el departamento que se esta verificando
        //                 (0 = no hay; 1 = si hay).
        // ------------------------------------------------------------------------------
        [HttpPost]
        public int haveEmployeesVerify(int idDept)
        {
            Conection conection = new Conection();
            int response = 0;
            try
            {
                List<Employee> employees = conection.employees();
                foreach (Employee emp in employees)
                {
                    if(emp.TeamId == idDept)
                    {
                        response = 1;
                        break;
                    }

                    List<Department> teams = conection.searchSubDepartments(idDept);
                    List<Department> teams2 = new List<Department>();
                    foreach (Department team in teams)
                    {
                        teams2.AddRange(conection.searchSubDepartments(team.Deptid));
                    }
                    teams2.AddRange(teams);

                    foreach (Department team in teams2)
                    {
                        if (emp.TeamId == team.Deptid)
                        {
                            response = 1;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }

            return response;
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/users
        //      /Management/users?message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se gestionan los usuarios del sistema.
        // Parametros:
        //              message         -> representa el tipo mensaje de retroalimentación que se mostrara 
        //                                  al usuario dependiendo del escenario que se presente.
        //              messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult users(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["users"] = conection.users();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/editUser
        //      /Management/editUser?message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se actualiza un usuario.
        // Parametros:
        //              message         -> representa el tipo mensaje de retroalimentación que se mostrara 
        //                                  al usuario dependiendo del escenario que se presente.
        //              messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult editUser(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["user"] = conection.searchUser(AccountController.getIdUser());
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/teams
        //      /Management/teams?message=2&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos 
        //          los equipos de la empresa.
        // Parametros: Puede recibir dos parametros o ninguno dependiendo de donde se llame:
        //              message         -> representa el tipo de mensaje de retroalimentación que se mostrara 
        //                                  al usuario dependiendo del escenario que se presente.
        //              messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult teams(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["departments"] = conection.departments();
                ViewData["departmentsMongo"] = conection.departmentsMongo();
                ViewData["allEmployees"] = conection.employees();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Management/attendance
        //      /Management/attendance?message=2&messageValue=message
        // Funcion: accion encargada de mostrar la vista para registrar asistencia de un empleado.
        // Parametros:
        //              id              -> identificador del empleado.
        //              message         -> representa el tipo de mensaje de retroalimentación que se mostrara 
        //                                  al usuario dependiendo del escenario que se presente.
        //              messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult attendance(int id, int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["employee"] = conection.searchEmployee(id);
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }

        // ------------------------------------------------------------------------------
        // POST: /Management/attendance
        // Funcion: accion encargada de registrar el marcaje de un empleado.
        // Parametros: 
        //              id      -> identificador del empleado.
        //              date    -> fecha en la que se va a registrar el marcaje.
        //              place   -> representa si el marcaje que se va a realizar fue presencial o remoto.
        //              options -> representa si se va a marcar entrada, salida o ambas.
        //              inHour  -> hora de entrada.
        //              outHour -> hora de salida.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult attendance(int id, DateTime date, string place, string options, TimeSpan inHour, TimeSpan outHour)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Registrando marcaje.
            // Manejando cualquier excepcion que se presente.
            try
            {
                switch (options)
                {
                    case "in":
                        conection.checkAttendance(id, date.Add(inHour), "I", place);
                        break;
                    case "out":
                        conection.checkAttendance(id, date.Add(outHour), "O", place);
                        break;
                    case "two":
                        conection.checkAttendance(id, date.Add(inHour), "I", place);
                        conection.checkAttendance(id, date.Add(outHour), "O", place);
                        break;
                }
                message = 2;
                messageValue = "Marcaje realizado exitosamente.";
            }
            catch (Exception e)
            {
                message = 3;
                // messageValue = "Marcaje no realizado. Por favor intentelo de nuevo.";
                messageValue = e.Message;
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "attendance",
                message = message,
                messageValue = messageValue,
            });
        }
    }
}
