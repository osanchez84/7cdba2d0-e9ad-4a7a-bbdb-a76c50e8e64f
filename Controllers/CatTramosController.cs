using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatTramosController : Controller
    {
        private readonly ICatTramosService _catTramosService;

        public CatTramosController(ICatTramosService catTramosService)
        {
            _catTramosService = catTramosService;
        }

        

    }
}