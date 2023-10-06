using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatTipoUsuarioController : BaseController
    {
        private readonly ICatTipoUsuarioService _catTipoUsuarioService;


        public CatTipoUsuarioController(ICatTipoUsuarioService catTipoUsuarioService)
        {
            _catTipoUsuarioService = catTipoUsuarioService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
