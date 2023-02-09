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
    [Route("")]
    public class InicioController : Controller
    {

        private readonly ILogger<InicioController> _logger;
        private SeguridadService _seguridadService;


        public InicioController(ILogger<InicioController> logger, SeguridadService seguridadService)
        {
            _logger = logger;
            _seguridadService = seguridadService;
        }

        [HttpGet("login")]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if(User.Identity.IsAuthenticated)
                return View("Inicio");
            return View();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(string usuario, string password, string returnUrl)
        {
            var login = _seguridadService.GetLogin(usuario, password);

            if (login.IdUsuario < 1)
            {
                ViewData["msjerror"] = " Usuario y/o contraseña erronea";
                return Redirect("/login");
            }

            await SignInUser(login.IdUsuario, login.Nombre + " " + login.Paterno + " " + login.Materno, "Administrador");
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
        [Route("cerrar-sesion")]
        public async Task<IActionResult> CerrarSesion()
        {
              await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(".GtoAdminApp");   
            HttpContext.Session.Clear();      
            return Redirect("/login");
        }

        


        [Route("/inicio")]
        [Authorize]
        public IActionResult Inicio()
        {
            return View();
        }
        [Route("error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
