using Microsoft.AspNetCore.Mvc;

namespace Example.WebUI.Controllers
{
    public class DepositosController : Controller
    {
        public IActionResult Depositos()
        {

            return View();

        }
    }
}
