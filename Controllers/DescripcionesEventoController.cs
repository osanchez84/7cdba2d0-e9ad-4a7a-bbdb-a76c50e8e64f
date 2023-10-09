using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class DescripcionesEventoController : BaseController
    {
        private readonly ICatDescripcionesEventoService _catDescripcionesEventoService;


        public DescripcionesEventoController(ICatDescripcionesEventoService catDescripcionesEventoService)
        {
            _catDescripcionesEventoService = catDescripcionesEventoService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
