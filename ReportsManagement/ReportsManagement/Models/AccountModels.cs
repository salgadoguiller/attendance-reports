using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace ReportsManagement.Models
{
    // ------------------------------------------------------------------------------
    // Funcion: 
    // Propiedades:
    //          UserProfiles    ->
    // Metodos: 
    //          Constructor     ->
    // ------------------------------------------------------------------------------
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("ReportsManagement")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }


    // ------------------------------------------------------------------------------
    // Funcion: almacenar información del usuario.
    // Propiedades:
    //          UserId      -> identificador del usuario.
    //          UserName    -> nombre del usuario.
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }


    // ------------------------------------------------------------------------------
    // Funcion: almacenar información del login.
    // Propiedades:
    //          UserName    -> nombre de usuario ingresado.
    //          Password    -> contraseña ingresada.
    //          RememberMe  -> representa si el usuario eligio la opcion recordar. (No se utiliza)
    // Metodos: no tiene ningun metodo.
    // ------------------------------------------------------------------------------
    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
