using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase permite almacenar la información de correos de confirmacion de
    //          reportes semanales.
    // Propiedades:
    //          Id      -> identificador unico de un empleado.
    //          Email   -> email del empleado.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    public class ConfirmEmail
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}