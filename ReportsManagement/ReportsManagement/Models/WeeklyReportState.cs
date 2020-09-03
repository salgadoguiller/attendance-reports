using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de almacenar informacion relacionada con el estado
    //          de envio de los reportes semanales.
    // Propiedades: 
    //          Id              ->  identificador del reporte semanal.
    //          StateReport     ->  estado de envio del reporte semanal.
    //                              0 -> no enviado.
    //                              1 -> enviado.
    //                              2 -> parcialmente enviado.
    //          PercentReports  ->  porcentaje de envio del reporte semanal.
    //          StartDate       ->  fecha de inicio del reporte.
    //          EndDate         ->  fecha de finalizacion del reporte.
    //          TypeError       ->  tipo de error que ocurrio al enviar un reporte.
    //                              0 -> sin error.
    //                              1 -> error al conectar con la base de datos.
    //                              2 -> error al enviar los reportes.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    public class WeeklyReportState
    {
        public int Id { get; set; }
        public int StateReport { get; set; }
        public decimal PercentReports { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TypeError { get; set; }
    }
}