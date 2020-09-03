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
    //          con la bitacora dentro del sistema.
    // Propiedades: no tiene ninguna propiedad.
    // Metodos: tiene una serie de metodos con una descripción detallada.
    // ------------------------------------------------------------------------------
    [Authorize]
    public class LogController : Controller
    {
        // ------------------------------------------------------------------------------
        // GET: /Log/showLog?message=1&messageValue=message
        // Funcion: accion encargada de presentar la vista donde se muestran todos los 
        //          cambios realizados en las bases de datos desde el sistema.
        // Parametros:
        //          message         -> representa el tipo de mensaje de retroalimentación que se mostrara 
        //                             al usuario dependiendo del escenario que se presente.
        //          messageValue    -> contiene el mensaje que se mostrara al usuario.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult showLog(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["employeesBK"] = conection.allEmployeesBK();
                ViewData["teamsBK"] = conection.allTeamsBK();
                ViewData["departmentsBK"] = conection.allDepartmentsBK();
                ViewData["teams"] = conection.departments();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Log/rollBackEmployee?idEmp=76&idEmpBK=25&typeAction=1
        // Funcion: accion encargada de presentar una vista donde se muestra información del empleado 
        //          al que se quiere revertir los cambios, se muestra el estado actual y el estado en la bitacora.
        // Parametros:
        //          idEmp       -> identificador del estado actual del empleado.
        //          idEmpBK     -> identificador del estado en la bitacora del empleado.
        //          typeAction  -> identifica si la accion que se quiere revertir es una
        //                         actualizacion o una eliminacion. (1 = update; 2 = delete)
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult rollBackEmployee(int idEmp, int idEmpBK, int typeAction)
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no un error.
            ViewData["error"] = false;
            ViewData["type"] = 2; //El empleado a revertir fue eliminado.
            ViewData["stateDepartment"] = "No-Deleted"; // El departamento al que pertenecia el empleado no ha sido eliminado.

            try
            {
                // Cargando información que se pasara a la vista.
                int state = conection.endStateEmployeeBK(idEmp);
                if (typeAction != 2 && state != 2)
                {
                    ViewData["type"] = state; // El usuario a revertir aun existe.
                    Employee emp = conection.searchEmployee(idEmp);
                    Department team = conection.searchDepartment(emp.TeamId);
                    ViewData["employee"] = emp;
                    ViewData["team"] = team;
                }
                EmployeeBK empBK = conection.searchEmployeeBK(idEmpBK);
                ViewData["employeeBK"] = empBK;
                if (conection.ExistDepartment(empBK.TeamId))
                {
                    Department teamBK = conection.searchDepartment(empBK.TeamId);
                    ViewData["teamBK"] = teamBK;
                }
                else
                {
                    ViewData["stateDepartment"] = "Deleted";
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
        // POST: /Log/rollBackEmployeeExecute
        // Funcion: accion encargada de revertir los cambios realizados en un empleado.
        // Parametros:
        //          idEmp   -> identificador del estado actual del empleado.
        //          idEmpBK -> identificador del estado en la bitacora del empleado.
        //          type    -> identifica si la accion que se quiere revertir es una
        //                     actualizacion o una eliminacion. (1 = update; 2 = delete)
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult rollBackEmployeeExecute(int idEmp, int idEmpBK, int type)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Revirtiendo los cambios
            // Manejando cualquier excepcion que se pueda presentar.
            try
            {
                EmployeeBK empBK = conection.searchEmployeeBK(idEmpBK);
                if (type == 1)
                {
                    // Revertir
                    Employee emp = conection.searchEmployee(idEmp);
                    conection.updateEmployee(idEmp, empBK.TeamId, empBK.Name, empBK.Email, AccountController.getIdUser());
                }
                else
                {
                    // Revertir
                    if (type == 2)
                    {
                        // Revertir
                        conection.rollBackEmployeeDeleted(AccountController.getIdUser(), idEmpBK);
                    }
                    else
                    {
                        // Revertir
                        conection.deleteEmployee(idEmp, AccountController.getIdUser());
                    }
                    
                }
                message = 2;
                messageValue = "Cambios revertidos exitosamente.";
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                message = 3;
                messageValue = "Cambios no revertidos. Por favor intentelo de nuevo.";
            }
            
            // Redireccionando a la ruta principal de la bitacora.
            return RedirectToRoute(new
            {
                controller = "Log",
                action = "showLog",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // GET: /Log/rollBackTeam?idDept=76&idDeptBK=25
        // Funcion: accion encargada de presentar una vista donde se muestra información del equipo 
        //          al que se quiere revertir los cambios, se muestra el estado actual y el estado en la bitacora.
        // Parametros:
        //          idTeam       -> identificador del estado actual del equipo.
        //          idTeamBK     -> identificador del estado en la bitacora del equipo.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult rollBackTeam(int idTeam, int idTeamBK)
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no un error.
            ViewData["error"] = false;
            ViewData["stateDepartment"] = "No-Deleted";

            try
            {
                // Cargando información que se pasara a la vista.
                
                
                ViewData["teamBK"] = conection.searchTeamBK(idTeamBK);

                if (conection.ExistDepartment(idTeam))
                {
                    ViewData["team"] = conection.searchDepartment(idTeam);
                    ViewData["teamMongo"] = conection.searchTeamMongo(idTeam, conection.searchDepartmentTeamMongo(idTeam));
                }
                else
                {
                    ViewData["stateDepartment"] = "Deleted";
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
        // POST: /Log/rollBackTeamExecute
        // Funcion: accion encargada de revertir los cambios realizados en un equipo.
        // Parametros:
        //          idTeam   -> identificador del estado actual del equipo.
        //          idTeamBK -> identificador del estado en la bitacora del equipo.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult rollBackTeamExecute(int idTeam, int idTeamBK)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Revirtiendo los cambios
            // Manejando cualquier excepcion que se presente.
            try
            {
                // Revertir
                int idDept = conection.searchDepartmentTeamMongo(idTeam);
                TeamBK teamBK = conection.searchTeamBK(idTeamBK);
                GetReportMongo team = conection.searchTeamMongo(idTeam, idDept);
                conection.updateTeamMongo(idDept, idTeam, teamBK.subName, teamBK.subEmail, teamBK.subdept, team.subdept, team.subEmail, team.subName, AccountController.getIdUser());
                message = 2;
                messageValue = "Cambios revertidos exitosamente.";
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                message = 3;
                messageValue = "Cambios no revertidos. Por favor intentelo de nuevo.";
            }

            // Redireccionando a la ruta principal de la bitacora.
            return RedirectToRoute(new
            {
                controller = "Log",
                action = "showLog",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // GET: /Log/rollBackDepartment?idDept=76&idDeptBK=25
        // Funcion: accion encargada de presentar una vista donde se muestra información del departamento 
        //          al que se quiere revertir los cambios, se muestra el estado actual y el estado en la bitacora.
        // Parametros:
        //          idDept       -> identificador del estado actual del departamento.
        //          idDeptBK     -> identificador del estado en la bitacora del departamento.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult rollBackDepartment(int idDept, int idDeptBK)
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no un error.
            ViewData["error"] = false;
            ViewData["stateDepartment"] = "No-Deleted";

            try
            {
                // Cargando información que se pasara a la vista.
                if (conection.ExistDepartment(idDept))
                {
                    ViewData["department"] = conection.searchDepartment(idDept);
                    ViewData["departmentMongo"] = conection.searchDepartmentMongo(idDept);
                }
                else
                {
                    ViewData["stateDepartment"] = "Deleted";
                }
                ViewData["departmentBK"] = conection.searchDepartmentBK(idDeptBK);
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Log/rollBackDepartment?idDept=76&idDeptBK=25
        // Funcion: accion encargada de revertir los cambios realizados en un departamento.
        // Parametros:
        //          idDept       -> identificador del estado actual del departamento.
        //          idDeptBK     -> identificador del estado en la bitacora del departamento.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult rollBackDepartmentExecute(int idDept, int idDeptBK)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Revirtiendo los cambios.
            // Manejando cualquier excepcion que se presente.
            try
            {
                // Revertir
                DepartmentBK deptBK = conection.searchDepartmentBK(idDeptBK);
                DepartmentMongo dept = conection.searchDepartmentMongo(idDept);
                conection.updateDepartmentMongo(idDept, deptBK.Name[0], dept.Name[0], deptBK.correo[0], dept.correo[0], deptBK.Nombre_Dept, dept.Nombre_Dept, AccountController.getIdUser());
                message = 2;
                messageValue = "Cambios revertidos exitosamente.";
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                message = 3;
                messageValue = "Cambios no revertidos. Por favor intentelo de nuevo.";
            }

            // Redireccionando a la ruta principal de la bitacora.
            return RedirectToRoute(new
            {
                controller = "Log",
                action = "showLog",
                message = message,
                messageValue = messageValue,
            });
        }
    }
}
