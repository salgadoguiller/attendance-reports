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
    //          Mapea la estructura de esta base de datos.
    // Propiedades:
    //          Id          -> identificador unico de un documento que contiene información de un departamento.
    //          Nombre_Dept -> nombre de un departamento.
    //          Name        -> nombre del responsable de un departamento.
    //          correo      -> correo del responsable de un departamento.
    //          Id_Dept     -> identificador de un documento que contiene información de un 
    //                         departamento y que esta asociado a la base de datos SQL.
    //          GetReport   -> array de objetos GetReport (información de los equipos pertenecientes al departamento).
    // Metodos: no tiene ningun metodo ademas de los constructores.
    // ------------------------------------------------------------------------------
    public class DepartmentMongo
    {
        public ObjectId Id { get; set; }
        public string Nombre_Dept { get; set; }
        public string[] Name { get; set; }
        public string[] correo { get; set; }
        public string Id_Dept { get; set; }
        public GetReportMongo[] GetReport { get; set; }

        public DepartmentMongo()
        {

        }

        public DepartmentMongo(ObjectId id, string nombreDept, string[] name, string[] correo, string idDept, GetReportMongo getReport)
        {
            this.Id = id;
            this.Nombre_Dept = nombreDept;
            this.Name = name;
            this.correo = correo;
            this.Id_Dept = idDept;
            this.GetReport = GetReport;
        }

        public override string ToString()
        {
            return "Id: " + this.Id +
                    "\nId Departamento: " + this.Id_Dept +
                    "\nNombre Departamento: " + this.Nombre_Dept +
                    "\nNombre Responsable: " + this.Name +
                    "\nCorreo Responsable: " + this.correo;
        }
    }
}