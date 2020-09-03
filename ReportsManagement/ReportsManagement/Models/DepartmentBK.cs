using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de almacenar la información referente a 
    //          los backup de departamentos.
    //          Extiende de la clase Department.
    // Propiedades:
    //          IdBK        -> identificador del backup de un equipo equipo.
    //          TypeAction  -> representa el tipo de accion que se dio para generar el backup
    //                         del equipo (1 = update; 2 = delete).
    //          DateAction  -> fecha en que se genero el backup del equipo.
    //          UserName    -> representa el responsable de realizar la accion (update o delete)
    //                         para generar el backup.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    public class DepartmentBK : DepartmentMongo
    {
        public int IdBK { get; set; }
        public int TypeAction { get; set; }
        public DateTime DateAction { get; set; }
        public string UserName { get; set; }
    }
}