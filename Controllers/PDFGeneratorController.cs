using GuanajuatoAdminUsuarios.Helpers;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.Controllers
{
	//[Authorize]
	public class PDFGeneratorController : Controller
	{
		private readonly ICapturaAccidentesService _capturaAccidentesService;
		private readonly ICatAutoridadesDisposicionService _catAutoridadesDisposicionservice;
		public PDFGeneratorController(ICapturaAccidentesService capturaAccidentesService, ICatAutoridadesDisposicionService catAutoridadesDisposicionservice)
		{
			_capturaAccidentesService = capturaAccidentesService;
			_catAutoridadesDisposicionservice = catAutoridadesDisposicionservice;
		}

		[HttpGet]
		public IActionResult AccidentesDetallado(int idAccidente)
		{
			int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
			//explotar y usar la variable de sesion para pasar un dato de esta manera no es correcto, en este caso no se supo manipular el id de accidente entre las pantallas que tambien no es la mejor manera estar cambiando de pantallas constantemente para un mismo flujo, equipo tecnico revisar esto

			//HttpContext.Session.GetInt32("LastInsertedId") ?? 0; // Obtener el valor de lastInsertedId desde la variable de sesión
			var AccidenteSeleccionado = _capturaAccidentesService.ObtenerAccidentePorId(idAccidente, idOficina);
			DatosAccidenteModel datosAccidente = _capturaAccidentesService.ObtenerDatosFinales(idAccidente);
			var ListVehiculosInvolucrados = _capturaAccidentesService.VehiculosInvolucrados(idAccidente);

			var ListClasificaciones = _capturaAccidentesService.ObtenerDatosGrid(idAccidente);
			var ListFactores = _capturaAccidentesService.ObtenerDatosGridFactor(idAccidente);
			var ListCausas = _capturaAccidentesService.ObtenerDatosGridCausa(idAccidente);
			var ListInfracciones = _capturaAccidentesService.InfraccionesDeAccidente(idAccidente);

			PDFAccidenteDetalladoModel model = new PDFAccidenteDetalladoModel();
			model.ParteAccidente = AccidenteSeleccionado;
			model.ParteAccidenteComplemento = datosAccidente;
			model.VehiculosInvolucrados = ListVehiculosInvolucrados;
			model.Clasificaciones = ListClasificaciones;
			model.Factores = ListFactores;
			model.CausasDeterminantes = ListCausas;
			model.Infracciones = ListInfracciones;
			model.ADisposicion = _catAutoridadesDisposicionservice.ObtenerAutoridadesActivas().Where(w => w.IdAutoridadDisposicion == AccidenteSeleccionado.IdAutoridadDisposicion).Select(s => s.NombreAutoridadDisposicion).FirstOrDefault();
			//string jsonView = await this.RenderViewAsync("AccidentesDetallado", AccidenteSeleccionado, false);

			//return Json(jsonView);
			return PartialView("_AccidentesDetallado", model);
		}
	}
}
