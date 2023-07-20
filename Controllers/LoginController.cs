using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
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

        // Parsear el JSON de respuesta
        dynamic json = JsonConvert.DeserializeObject(content);
        string nombre = json[0].nombre;
        string oficina = json[0].oficina;
            //string[] vector = json[0].vector.ToObject<string[]>();
            List<RespuestaServicio> listaRespuestas = JsonConvert.DeserializeObject<List<RespuestaServicio>>(content);

            // Obtener el vector de la primera respuesta (en este caso solo hay una)
            string vectorString = listaRespuestas.FirstOrDefault()?.Vector;

            // Convertir el vector en una lista de enteros
            List<int> listaIdsPermitidos = vectorString.Split(',').Select(int.Parse).ToList();
            string listaIdsPermitidosJson = JsonConvert.SerializeObject(listaIdsPermitidos);
            // Guardar la lista en la variable de sesión
            HttpContext.Session.SetString("IdsPermitidos", listaIdsPermitidosJson);            
            HttpContext.Session.SetString("Nombre", nombre);
        HttpContext.Session.SetString("Oficina", oficina);
        //HttpContext.Session.SetString("Vector", string.Join(",", vector));

        return View("ConsumirServicio", (object)content);
    }


}
}
