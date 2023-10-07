using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatTipoInvolucradoController : BaseController
    {
        private readonly ICatTipoInvolucradoService _catTipoInvolucradoService;
    
        public CatTipoInvolucradoController(ICatTipoInvolucradoService catTipoInvolucradoService)
        {
        _catTipoInvolucradoService = catTipoInvolucradoService;
     
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
