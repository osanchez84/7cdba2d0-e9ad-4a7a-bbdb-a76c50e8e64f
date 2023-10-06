using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatCinturonController : BaseController
    {
        private readonly ICatCinturon _catCinturon;

        public CatCinturonController(ICatCinturon catCinturon)
        {
            _catCinturon = catCinturon;

        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
