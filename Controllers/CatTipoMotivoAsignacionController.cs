using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatTipoMotivoAsignacionController : Controller
    {
        private readonly ICatTipoMotivoAsignacionService _catTipoMotivoAsignacionService;


        public CatTipoMotivoAsignacionController(ICatTipoMotivoAsignacionService catTipoMotivoAsignacionService)
        {
            _catTipoMotivoAsignacionService = catTipoMotivoAsignacionService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
