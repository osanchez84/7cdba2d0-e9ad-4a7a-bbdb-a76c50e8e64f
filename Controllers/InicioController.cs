using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Route("")]
    public class InicioController : Controller
    {

        private readonly ILogger<InicioController> _logger;


        public InicioController(ILogger<InicioController> logger)
        {
            _logger = logger;

        }

        [HttpGet("Inicio")]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return View("Inicio");
            return View("Marca");
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




        [Route("/MarcasVehiculos")]
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
