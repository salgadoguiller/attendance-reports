using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;

namespace ConsoleApplication2._0
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase almacena la informacion de los departamentos de la base de datos MongoDB.
    // Propiedades: 
    //          Id              -> identificador del departamento.
    //          Nombre_Dept     -> nombre del departamento.
    //          Name            -> nombre del responsable del departamento.
    //          correo          -> correo electronico del responsable de departamento.
    //          Id_Dept         -> identificador del departamento (SQLServer).
    //          GetReport       -> arreglo de equipos que pertenecen al departamento.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    class InfoDept
    {
        public ObjectId Id { get; set; }
        public string Nombre_Dept { get; set; }
        public string[] Name { get; set; }
        public string[] correo { get; set; }
        public string Id_Dept { get; set; }
        public GetReport[] GetReport { get; set; }
    }
}
