using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatCinturonController : Controller
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
