using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase permite almacenar la información de los usuarios del sistema.
    // Propiedades:
    //          UserId      -> identificador unico de un empleado.
    //          UserName    -> nombre del empleado.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}