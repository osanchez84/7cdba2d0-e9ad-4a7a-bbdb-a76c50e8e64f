using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuanajuatoAdminUsuarios.Data;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using AdminUsuarios.Models.Commons;
using AdminUsuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using AdminUsuarios.Helpers;

namespace GuanajuatoAdminUsuarios.Controllers
{

    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        Seguridad _Seguridad = new Seguridad();
        private IConfiguration configuration;


        public HomeController(ILogger<HomeController> logger, IConfiguration conf)
        {
            _logger = logger;
            configuration = conf;
        }

        [Route("/login")]
        [Route("")]
        [AllowAnonymous]
        public IActionResult VistaLogin()
        {
            return View("Login");
        }


        [HttpPost]
        public async Task<IActionResult> Login(string usuario, string password, string returnUrl)
        {
            var logi = _Seguridad.GetLogin(usuario, password);

            if (logi.IdUsuario < 1)
            {
                ViewData["msjerror"] = " Usuario y/o contraseña erronea";
                return Redirect("/login");
            }

            /* var usuarioSession = new UsuarioSession{
                 IdUsuario =  logi.IdUsuario,
                 Nombre = logi.Nombre + " " + logi.Paterno + " " + logi.Materno,
                 Perfil = "Administrador"
             };

            HttpContext.Session.SetObject("usuarioSession",usuarioSession);*/

            await SignInUser(logi.IdUsuario, logi.Nombre + " " + logi.Paterno + " " + logi.Materno, "Administrador");
            if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
            {
                returnUrl = "/index";
            }

            return Redirect(returnUrl);
        }



        private async Task SignInUser(int idUsuario, string nombre, string perfil)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomClaims.IdUsuario, idUsuario.ToString()),
                 new Claim(CustomClaims.Nombre, nombre),
                new Claim(CustomClaims.Perfil, perfil)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

        public async Task<IActionResult> CerrarSesion()
        {
              await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(".GtoAdminApp");   
            HttpContext.Session.Clear();      
            return Redirect("/login");
        }

         public async Task<IActionResult> CambiarContrasena()
        {
              await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(".GtoAdminApp");         
            return Redirect("/login");
        }


        [Route("/index")]
        [Authorize]
        public IActionResult Inicio()
        {
            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
