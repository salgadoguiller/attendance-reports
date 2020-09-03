using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2._0
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de almacenar informacion relacionada con los detalles
    //          de envio de los reportes semanales.
    // Propiedades: 
    //          TypeReport          ->  tipo de reporte
    //                                  0 -> especial (Gina y Flor de Liz Reyes)
    //                                  1 -> departamento
    //                                  2 -> equipo
    //                                  3 -> empleado
    //          IdReceptor          ->  identificador del tipo de entidad para la que se genera el reporte.
    //                                  (departamento, equipo, empleado, especial)
    //          StateReport         ->  estado de envio del reporte semanal.
    //                                  0 -> no enviado.
    //                                  1 -> enviado.
    //          TypeError           ->  tipo de error que ocurrio al enviar un reporte.
    //                                  0 -> sin error.
    //                                  1 -> error al conectar con la base de datos.
    //                                  2 -> error al enviar los reportes.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    class ReportDetails
    {
        public int TypeReport { get; set; }
        public int IdReceptor { get; set; }
        public int StateReport { get; set; }
        public int TypeError { get; set; }

        public ReportDetails(int typeReport, int idReceptor, int stateReport, int typeError)
        {
            this.TypeReport = typeReport;
            this.IdReceptor = idReceptor;
            this.StateReport = stateReport;
            this.TypeError = typeError;
        }
    }
}
