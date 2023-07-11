using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class EnvioInfraccionesController : Controller
    {
        private readonly IEnvioInfraccionesService _envioInfraccionesService;

        public EnvioInfraccionesController(IEnvioInfraccionesService envioInfraccionesService)
        {
            _envioInfraccionesService = envioInfraccionesService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
