using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase permite almacenar la información proveniente de la base de datos SQL. 
    //          Mapea la estructura de esta base de datos.
    // Propiedades:
    //          Id      -> identificador unico de un empleado.
    //          DeptId  -> identificador del departamento al que pertenece un empleado asociado a la base de datos mongo: 
    //                              Department.Id_Dept para los responsables de departamentos.
    //                              Department.GetReport[x].deptID para los responsables de equipos. 
    //          TeamId  -> ******** Por ahora este campo es el que hace lo descrito en DeptId, y DeptId no se utiliza ********
    //          Name    -> nombre del empleado.
    //          Email   -> email del empleado.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    public class Employee
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<string> ResponsableOf { get; set; }
    }
}