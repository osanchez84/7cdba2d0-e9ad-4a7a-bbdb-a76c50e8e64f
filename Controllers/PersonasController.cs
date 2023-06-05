using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ICatDictionary _catDictionary;
        private readonly IPersonasService _personasService;

        public PersonasController(ICatDictionary catDictionary, IPersonasService personasService)
        {
            _catDictionary = catDictionary;
            _personasService = personasService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
