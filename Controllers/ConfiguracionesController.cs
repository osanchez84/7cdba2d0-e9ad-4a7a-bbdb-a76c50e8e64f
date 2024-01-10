using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class ConfiguracionesController : BaseController
    {
       
        public ActionResult Index()
        {

			return View();
        }

		[HttpPost]
		public async Task<IActionResult> ActualizarContra(string NuevaContrasena)
		{
			var usuario = User.FindFirst(CustomClaims.IdUsuario).Value;
			try
			{
				var handler = new HttpClientHandler
				{
					ServerCertificateCustomValidationCallback = (request, cert, chain, errors) =>
					{
						Console.WriteLine("SSL error skipped");
						return true;
					}
				};

				using (HttpClient client = new HttpClient(handler))
				{
					string url = $"https://10.16.157.142:9096/serviciosinfracciones/getActualizaPwd?userWS=1&claveWS=18&idUsuario={usuario}&contraseña={NuevaContrasena}";

					var ip = HttpContext.Connection.RemoteIpAddress.ToString();

					HttpResponseMessage response = await client.GetAsync(url);

					if (response.IsSuccessStatusCode)
					{
						string content = await response.Content.ReadAsStringAsync();
						dynamic json = JsonConvert.DeserializeObject(content);

						if (json != null && json.Count > 0)
						{
							string nombre = json[0].nombre;
							string oficina = json[0].oficina;
							string idDependenciaStr = json[0].tipo_oficina;

							if (int.TryParse(idDependenciaStr, out int idDependencia))
							{
								HttpContext.Session.SetInt32("IdDependencia", idDependencia);

							}
							else
							{

							}
							string idOficinaStr = json[0].clave_oficina;
							// string idDependenciaStr = json[0].tipo_oficina;
							string idUsuario = json[0].idUsuario;
							string TipoOfi = json[0].tipo_oficina;

							if (int.TryParse(idOficinaStr, out int idOficina))
							{
								HttpContext.Session.SetInt32("IdOficina", idOficina);

							}
							else
							{

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
								// HttpContext.Session.SetInt32("IdDependencia", idDependencia);

								return Json(listaIdsPermitidosJson);
							}
						}
					}



					// En caso de respuestas inválidas o vacías, limpiar la variable de sesión
					HttpContext.Session.Remove("IdsPermitidos");
					HttpContext.Session.Remove("Nombre");
					HttpContext.Session.Remove("Oficina");
					HttpContext.Session.Remove("IdDependencia");


					return BadRequest("Error en la respuesta del servicio");
				}
			}
			catch (Exception ex)
			{
				HttpContext.Session.Clear();
				return StatusCode(500, $"Error en el servidor: {ex.Message}");
			}
		}



	}
}
