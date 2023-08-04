using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            try
            {
                var url = $"https://virtual.zeitek.net:9094/serviciosinfracciones/getlogin?userWS=1&claveWS=1&usuario={usuario}&contraseña={contrasena}";

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(content);

                    if (json != null && json.Count > 0)
                    {
                        string nombre = json[0].nombre;
                        string oficina = json[0].oficina;
                        string idOficinaStr = Regex.Match(oficina, @"\d+").Value;
                        string entidad = Regex.Match(oficina, @"\d+(.+?)\|").Groups[1].Value.Trim();
                        if (int.TryParse(idOficinaStr, out int idOficina))
                        {
                            HttpContext.Session.SetInt32("IdOficina", idOficina);

                        }
                        else
                        {
                            // Manejar el caso en el que 'oficina' no contiene un número válido al inicio.
                            // En este caso, idOficina mantendrá su valor predeterminado (cero).
                        }
                        string delegacion = Regex.Match(oficina, @"\|(.+)").Groups[1].Value.Trim();

                        List<RespuestaServicio> listaRespuestas = JsonConvert.DeserializeObject<List<RespuestaServicio>>(content);
                        string vectorString = listaRespuestas.FirstOrDefault()?.Vector;
                    if (!string.IsNullOrEmpty(vectorString))
                    {
                            List<int> listaIdsPermitidos = vectorString.Split(',').Select(int.Parse).ToList();
                            string listaIdsPermitidosJson = JsonConvert.SerializeObject(listaIdsPermitidos);

                            // Guardar la lista en la variable de sesión
                            HttpContext.Session.SetString("IdsPermitidos", listaIdsPermitidosJson);
                            HttpContext.Session.SetString("Nombre", nombre);
                            HttpContext.Session.SetString("Oficina", oficina);

                            return Json(listaIdsPermitidosJson);
                        }
                    }
                }

                // En caso de respuestas inválidas o vacías, limpiar la variable de sesión
                HttpContext.Session.Remove("IdsPermitidos");
                HttpContext.Session.Remove("Nombre");
                HttpContext.Session.Remove("Oficina");

                return BadRequest("Error en la respuesta del servicio");
            }
            catch (Exception ex)
            {
                // En caso de errores, limpiar la variable de sesión y manejar el error
                HttpContext.Session.Clear();
                return StatusCode(500, $"Error en el servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetIdsPermitidos()
        {
            var idsPermitidosJson = HttpContext.Session.GetString("IdsPermitidos");
            var idsPermitidos = JsonConvert.DeserializeObject<List<int>>(idsPermitidosJson) ?? new List<int>();
            return Json(idsPermitidos);
        }


    }
}
