using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportsManagement.Models;
using System.Diagnostics;

namespace ReportsManagement.Controllers
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de brindar toda la funcionalidad relacionada 
    //          con el monitoreo del estado de envio de los reportes semanales.
    // Propiedades: no tiene ninguna propiedad.
    // Metodos: tiene una serie de metodos con una descripción detallada.
    // ------------------------------------------------------------------------------
    [Authorize]
    public class SupervisionController : Controller
    {
        // ------------------------------------------------------------------------------
        // GET: /Supervision/report
        // Funcion: accion encargada de presentar la vista donde se muestra el estado de 
        //          envio de los reportes semanales.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult report(int message = 1, string messageValue = "")
        {
            Conection conection = new Conection();

            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["message"] = message;
            ViewData["messageValue"] = messageValue;
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["weeklyReports"] = conection.weeklyReportsStates();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }            

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Supervision/details
        // Funcion: accion encargada de presentar la vista donde se muestran los detalles del
        //          envio del reporte semanal.
        // Parametros:
        //          idRep    -> identificador del envio semanal. 
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult details(int idRep)
        {
            Conection conection = new Conection();
            // Cargando información que se pasara a la vista independientemente si ocurre o no ocurre un error.
            ViewData["error"] = false;

            try
            {
                // Cargando información que se pasara a la vista.
                ViewData["weeklyReportsDetails"] = conection.weeklyReportsDetails(idRep);
                ViewData["employees"] = conection.employees();
                ViewData["departments"] = conection.departments();
            }
            catch (Exception e)
            {
                // Cargando información que se pasara a la vista si ocurre un error.
                ViewData["error"] = true;
            }

            return View();
        }


        // ------------------------------------------------------------------------------
        // GET: /Supervision/fordward
        // Funcion: accion encargada de reenviar los reportes de horas que no se enviaron.
        // Parametros:
        //          idRep   -> identificador del envio semanal.
        //          all     -> indica si se enviaran todos los reportes o solo un porcentaje.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult forward(int idRep, bool all = false)
        {
            Conection conection = new Conection();
            ReportController report = new ReportController();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Reenviando reportes.
            // Manejando cualquier excepcion que se presente.
            try
            {
                // Reenviar
                if (all)
                {
                    // Estableciendo metodo email

                    // Reenviando todos los reportes.
                    Process.Start("C:\\inetpub\\wwwroot\\ReportsManagement\\_AttendanceReports\\ConsoleApplication2.0\\bin\\Debug\\ConsoleApplication2.0.exe", "all," + idRep);
                }
                else
                {
                    Process.Start("C:\\inetpub\\wwwroot\\ReportsManagement\\_AttendanceReports\\ConsoleApplication2.0\\bin\\Debug\\ConsoleApplication2.0.exe", "partial," + idRep);
                }
                message = 2;
                messageValue = "Realizando reenvío.";
            }
            catch (Exception e)
            {
                message = 3;
                messageValue = "Error al realizar el reenvío. Por favor intentelo de nuevo.";
                messageValue = "Error al reenviar. Por favor intentelo de nuevo.";
            }
            
            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Supervision",
                action = "report",
                message = message,
                messageValue = messageValue,
            });
        }
    }
}
