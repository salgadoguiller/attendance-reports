using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase permite almacenar la información proveniente de la base de datos mongo. 
    //          Mapea la estructura de los GerReport (equipos).
    // Propiedades:
    //          Id          -> identificador unico de un documento que contiene información 
    //                         de un GetReport (equipo).
    //          subdept     -> nombre de un equipo.
    //          deptID      -> identificador de un documento que contiene información de un 
    //                         equipo y que esta asociado a la base de datos SQL.
    //          subEmail    -> email del responsable de un equipo.
    //          subName     -> nombre del responsable de un equipo.
    // Metodos: no tiene ningun metodo ademas del constructor.
    // ------------------------------------------------------------------------------
    public class GetReportMongo
    {
        public ObjectId Id { get; set; }
        public string subdept { get; set; }
        public string deptID { get; set; }
        public string subEmail { get; set; }
        public string subName { get; set; }

        public GetReportMongo()
        { 
        
        }

        public GetReportMongo(ObjectId id, string subdept, string deptId, string subEmail, string subName)
        {
            this.Id = id;
            this.subdept = subdept;
            this.deptID = deptId;
            this.subEmail = subEmail;
            this.subName = subName;
        }
    }
}