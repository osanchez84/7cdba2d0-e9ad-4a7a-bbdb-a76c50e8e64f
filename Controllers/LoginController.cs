using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> ConsumirServicio(string usuario, string contrasena)
        {
            var url = $"https://virtual.zeitek.net:9094/serviciosinfracciones/getlogin?userWS=1&claveWS=1&usuario={usuario}&contraseña={contrasena}";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
          
            return View("ConsumirServicio", (object)content);
        }

    }
}
