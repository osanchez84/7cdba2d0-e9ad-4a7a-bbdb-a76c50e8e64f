using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatCarreterasController : Controller
    {
        private readonly ICatCarreterasService _catCarreterasService;

        public CatCarreterasController(ICatCarreterasService catCarreterasService)
        {
            _catCarreterasService = catCarreterasService;
        }
        public IActionResult Index()
        {
            var ListCarreterassModel = _catCarreterasService.ObtenerCarreteras();
            return View(ListCarreterassModel);
        }
    }
}
