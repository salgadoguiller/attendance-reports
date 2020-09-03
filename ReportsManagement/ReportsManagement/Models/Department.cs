using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase permite almacenar la información de departamentos proveniente de la base de datos sql. 
    // Propiedades:
    //          Deptid      -> identificador unico de un departamento o subdepartamento(equipo).
    //          DeptName    -> nombre de un departamento o subdepartamento(equipo).
    //          SupDeptid   -> identificador de el departamento al que pertenece un equipo.
    //                          0       -> LNO
    //                          1       -> departamentos
    //                          otro    ->  subdepartamentos (equipos)
    // Metodos: no tiene ningun metodo ademas del constructor.
    // ------------------------------------------------------------------------------
    public class Department
    {
        public int Deptid { get; set; }
        public string DeptName { get; set; }
        public int SupDeptid { get; set; }
    }
}