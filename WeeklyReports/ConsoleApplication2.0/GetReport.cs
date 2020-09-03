using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

namespace ConsoleApplication2._0
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase almacena la informacion de los equipos de la base de datos MongoDB.
    // Propiedades: 
    //          Id          -> identificador del equipo.
    //          subdept     -> nombre del equipo.
    //          deptID      -> identificador del departamento (SQLServer).
    //          subEmail    -> correo electronico del responsable de equipo.
    //          subName     -> nombre del responsable del equipo.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    class GetReport
    {
        public ObjectId Id { get; set; }
        public string subdept { get; set; }
        public string deptID { get; set; }
        public string subEmail { get; set; }
        public string subName { get; set; }
    }
}
