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
		public async Task<IActionResult> ActualizarContra(string NuevaContrasena, string Contrasena)
		{
			var usuario = User.FindFirst(CustomClaims.Usuario).Value;
			var IdUsuario = User.FindFirst(CustomClaims.IdUsuario).Value;

			try
			{
				var credencialesValidas = await VerificarCredenciales(usuario, Contrasena);

				if (credencialesValidas)
				{
					// Las credenciales son válidas, continuar con la lógica de actualización de contraseña
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
						string url = $"https://10.16.157.142:9096/serviciosinfracciones/getActualizaPwd?userWS=1&claveWS=18&idUsuario={IdUsuario}&contraseña={NuevaContrasena}";

						var ip = HttpContext.Connection.RemoteIpAddress.ToString();

						HttpResponseMessage response = await client.GetAsync(url);

						// Devolver la URL como JSON (puedes ajustar esto según tus necesidades)
						return Json(url);
					}
				}
				else
				{
					// Las credenciales no son válidas, devolver un mensaje indicando al usuario
					return BadRequest("La contraseña proporcionada no es correcta.");
				}
			}
			catch (Exception ex)
			{
				HttpContext.Session.Clear();
				return StatusCode(500, $"Error en el servidor: {ex.Message}");
			}
		}

		public async Task<bool> VerificarCredenciales(string usuario, string contrasena)
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
				string url = $"https://10.16.158.31:9096/serviciosinfracciones/getlogin?userWS=1&claveWS=18&usuario={usuario}&contraseña={contrasena}";

				var ip = HttpContext.Connection.RemoteIpAddress.ToString();

				HttpResponseMessage response = await client.GetAsync(url);

				string content = await response.Content.ReadAsStringAsync();

				if (!response.IsSuccessStatusCode)
				{
					// Si la respuesta no es exitosa, devolver false indicando que las credenciales no son válidas
					return false;
				}

				// Verificar si el contenido indica que las credenciales son incorrectas
				if (content.Contains("Unable to cast object of type 'System.DBNull' to type 'System.String'"))
				{
					// Las credenciales proporcionadas no son correctas, devolver false
					return false;
				}

				// Las credenciales son correctas, devolver true
				return true;
			}
		}


	}
}
