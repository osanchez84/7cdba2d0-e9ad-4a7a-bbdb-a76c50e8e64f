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
    public class InicioController : Controller
    {

        private readonly ILogger<InicioController> _logger;
        Seguridad _Seguridad = new Seguridad();
        private IConfiguration configuration;


        public InicioController(ILogger<InicioController> logger, IConfiguration conf)
        {
            _logger = logger;
            configuration = conf;
        }

        [HttpGet("/login")]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
                return View("Inicio");
            return View();
        }


        [HttpPost("/login")]
        public async Task<IActionResult> Login(string usuario, string password, string returnUrl)
        {
            var logi = _Seguridad.GetLogin(usuario, password);

            if (logi.IdUsuario < 1)
            {
                ViewData["msjerror"] = " Usuario y/o contraseña erronea";
                return Redirect("/login");
            }

            await SignInUser(logi.IdUsuario, logi.Nombre + " " + logi.Paterno + " " + logi.Materno, "Administrador");
            if (string.IsNullOrWhiteSpace(returnUrl) || !returnUrl.StartsWith("/"))
            {
                returnUrl = "/inicio";
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


        [Route("/inicio")]
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
