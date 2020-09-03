using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using ReportsManagement.Filters;
using ReportsManagement.Models;

namespace ReportsManagement.Controllers
{
    // ------------------------------------------------------------------------------
    // Funcion: esta clase se encarga de brindar toda la funcionalidad relacionada 
    //          con la seguridad en el sistema.
    // Propiedades: no tiene ninguna propiedad.
    // Metodos: tiene una serie de metodos con una descripción detallada.
    // ------------------------------------------------------------------------------

    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        // ------------------------------------------------------------------------------
        // GET: /Account/login?returnUrl=url
        // Funcion: accion encargada de presentar la vista donde los usuarios del sistema
        //          inician sesion.
        // Parametros: 
        //          returnUrl   -> representa la ruta hacia donde se redigira el flujo de ejecución
        //                         luego de iniciar sesion.
        // ------------------------------------------------------------------------------
        [AllowAnonymous]
        public ActionResult login(string returnUrl)
        {
            if ((string)Session["type"] == (string)"admin")
            {
                return RedirectToAction("employees", "Management");
            }
            ViewData["message"] = 1;
            ViewData["messageValue"] = "";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        // ------------------------------------------------------------------------------
        // POST: /Account/login
        // Funcion: accion encargada de validar las credenciales de un usuario e iniciar sesion.
        // Parametros: 
        //          model       -> objeto de la clase LoginModel que tiene los siguientes atributos:
        //                          UserName    -> nombre de usuario ingresado pór el usuario.
        //                          Password    -> conteaseña ingresada por el usuario.
        //                          RememberMe  -> atributo que representa si el usuario eligio la opcion
        //                                         recordar. (No se utiliza)
        //          returnUrl   -> ruta a donde dirigira el flujo de ejecución despues de realizar el login.
        // ------------------------------------------------------------------------------
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginModel model, string returnUrl)
        {
            // Verificando que las credenciales sean correctas.
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                Session["type"] = "admin";
                return redirectToLocal(returnUrl);
            }

            // Credenciales incorrectas.
            ViewData["message"] = 3;
            ViewData["messageValue"] = "Usuario o contraseña incorrecta. Por favor intentelo de nuevo.";
            return View(model);
        }


        // ------------------------------------------------------------------------------
        // POST: /Account/logout
        // Funcion: accion encargada de cerrar sesion.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult logout()
        {
            WebSecurity.Logout();
            Session["type"] = "public";

            return RedirectToAction("login", "Account");
        }

        // ------------------------------------------------------------------------------
        // POST: /Account/newUser
        // Funcion: accion encargada de crear un nuevo usuario
        // Parametros: 
        //          name    -> nombre de usuario que se va a agregar.
        //          pass    -> contraseña del nuevo usuario.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult newUser(string name, string pass)
        {
            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Agregando un nuevo usuario.
            try
            {
                WebSecurity.CreateUserAndAccount(name, pass);
                message = 2;
                messageValue = "Usuario agregado exitosamente.";
            }
            catch (MembershipCreateUserException e)
            {
                message = 3;
                messageValue = "Usuario no agregado. Por favor intentelo de nuevo.";
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "users",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Account/updateUser
        // Funcion: accion encargada de crear un nuevo usuario
        // Parametros: 
        //          oldpass -> contraseña anterior.
        //          pass    -> contraseña del nuevo usuario.
        // ------------------------------------------------------------------------------
        [HttpPost]
        public ActionResult updateUser(string oldpass, string pass)
        {
            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Cambiando contraseña.
            try
            {
                bool change = WebSecurity.ChangePassword(WebSecurity.CurrentUserName, oldpass, pass);
                if (!change)
                {
                    message = 3;
                    messageValue = "Contraseña no cambiada. Por favor intentelo de nuevo.";
                }
                else
                {
                    message = 2;
                    messageValue = "Contraseña cambiada exitosamente.";
                }
            }
            catch (MembershipCreateUserException e)
            {
                message = 3;
                messageValue = "Contraseña no cambiada. Por favor intentelo de nuevo.";
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "editUser",
                message = message,
                messageValue = messageValue,
            });
        }


        // ------------------------------------------------------------------------------
        // POST: /Account/updateUser
        // Funcion: accion encargada de eliminar un usuario
        // Parametros: 
        //          userId  -> identificador del usuario a eliminar.
        // ------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult deleteUser(int userId)
        {
            Conection conection = new Conection();

            // Representa los posibles escenarios que se pueden presentar en la ejecucion del sistema.
            int message;
            string messageValue;

            // Cambiando contraseña.
            try
            {
                conection.deleteUser(userId);
                message = 2;
                messageValue = "Usuario eliminado exitosamente.";
            }
            catch (MembershipCreateUserException e)
            {
                message = 3;
                messageValue = "Usuario no eliminado. Por favor intentelo de nuevo.";
            }

            // Redireccionando el flujo de ejecucion.
            return RedirectToRoute(new
            {
                controller = "Management",
                action = "users",
                message = message,
                messageValue = messageValue,
            });
        }

        // ------------------------------------------------------------------------------
        // POST: /Account/passwordVerify
        // Funcion: accion encargada de verificar si una contraseña ingresada corresponda con la del 
        //          usuario logeado.
        // Parametros:
        //          password    -> contraseña ingresada por el usuario.
        // Retorna:
        //          int         -> representa si la contraseña ingresada es correcta
        //                         (0 = no hay cambio; 1 = si hay cambio).
        // ------------------------------------------------------------------------------
        [HttpPost]
        public int passwordVerify(string password)
        {
            if(WebSecurity.Login(WebSecurity.CurrentUserName, password))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }


        // ------------------------------------------------------------------------------
        // Funcion: metodo estatico encargado de proporcionar el identificador del usuario logeado.
        // Parametros: no recibe ningun parametro.
        // ------------------------------------------------------------------------------
        public static int getIdUser()
        {
            return WebSecurity.CurrentUserId;
        }


        #region Helpers

        // ------------------------------------------------------------------------------
        // Funcion: accion encargada de redireccionar el flujo.
        // Parametros:
        //          returnUrl   -> ruta hacia donde se redigira el flujo.
        // ------------------------------------------------------------------------------
        private ActionResult redirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("employees", "Management");
            }
        }
        #endregion
    }
}
