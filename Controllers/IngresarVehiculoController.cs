using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class IngresarVehiculoController : BaseController
    {
        private readonly IIngresarVehiculosService _ingresarVehiculosService;
        private readonly ICatMarcasVehiculosService _catMarcasVehiculosService;
        private readonly IPlacaServices _placaServices;
        private readonly ICatMunicipiosService _catMunicipiosService;
        private readonly ICatDescripcionesEventoService _descripcionesEventoService;


        public IngresarVehiculoController(IIngresarVehiculosService ingresarVehiculosService, ICatMarcasVehiculosService catMarcasVehiculosService,
            IPlacaServices placaServices, ICatMunicipiosService catMunicipiosService, ICatDescripcionesEventoService descripcionesEventoService)
        {
            _ingresarVehiculosService = ingresarVehiculosService;
            _catMarcasVehiculosService = catMarcasVehiculosService;
            _placaServices = placaServices;
            _catMunicipiosService = catMunicipiosService;
            _descripcionesEventoService = descripcionesEventoService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult Marcas_Drop()
        {
            var result = new SelectList(_catMarcasVehiculosService.ObtenerMarcas(), "IdMarcaVehiculo", "MarcaVehiculo");
            return Json(result);
        }
        public JsonResult Placas_Read()
        {
            int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
            var result = new SelectList(_placaServices.GetPlacasByDelegacionId(idOficina), "IdDepositos", "Placa");
            return Json(result);
        }
        public JsonResult Municipios_Drop()
        {
            var result = new SelectList(_catMunicipiosService.GetMunicipios(), "IdMunicipio", "Municipio");
            return Json(result);
        }
        public JsonResult Descripcion_Drop()
        {
            var result = new SelectList(_descripcionesEventoService.ObtenerDescripciones(), "idDescripcion", "descripcionEvento");
            return Json(result);
        }
        [HttpPost]
        public IActionResult DirigirPorDependencia(string tipoIngreso)
        {
            if (tipoIngreso == "TransitoTransporte")
            {
                return Json(new { redirectTo = Url.Action("IngresoTransitoTransporte") });
            }
            else if (tipoIngreso == "OtraDependencia")
            {
                return Json(new { redirectTo = Url.Action("IngresoOtraDependencia") });
            }
            else
            {
                return Json(new { error = "Es necesario seleccionar una opción" });
            }
        }
        public IActionResult IngresoTransitoTransporte()
        {
            return View("IngresoTransitoTransporte");
        }
        public IActionResult IngresoOtraDependencia()
        {
            return View("IngresoOtraDependencia");
        }


        public IActionResult ajax_BusquedaDepositos(IngresoVehiculosModel model)
        {
            var listaDepositos = _ingresarVehiculosService.ObtenerDepositos(model);
            return Json(listaDepositos);
        }
        public IActionResult GuardarRegistroSeleccionado(int idDeposito)
        {
            var infoDeposito = _ingresarVehiculosService.DetallesDeposito(idDeposito);
            return Json(infoDeposito);
        }
        [HttpPost]
        [HttpPost]
        public IActionResult ajax_GuardarDatos(DatosIngresoModel datos)
        {
         var depositoModificado = _ingresarVehiculosService.GuardarFechaIngreso(datos);
            return Ok();

        }
    }
}
