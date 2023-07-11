using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using GuanajuatoAdminUsuarios.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class BusquedaAccidentesController : Controller
    {
        private readonly IBusquedaAccidentesService _busquedaAccidentesService;
        private readonly ICatCarreterasService _catCarreterasService;
        private readonly ICatTramosService _catTramosService;
        private readonly ICatDelegacionesOficinasTransporteService _catDelegacionesOficinasTransporteService;
        private readonly IOficiales _oficialesService;
        private readonly ICapturaAccidentesService _capturaAccidentesService;
        private readonly IPdfGenerator<BusquedaAccidentesModel> _pdfService;
        private readonly ICatDictionary _catDictionary;

        public BusquedaAccidentesController(IBusquedaAccidentesService busquedaAccidentesService, ICatCarreterasService catCarreterasService, ICatTramosService catTramosService,
            ICatDelegacionesOficinasTransporteService catDelegacionesOficinasTransporteService, IOficiales oficialesService, IPdfGenerator<BusquedaAccidentesModel> pdfService,
            ICapturaAccidentesService capturaAccidentesService,ICatDictionary catDictionary)
        {
            _busquedaAccidentesService = busquedaAccidentesService;
            _catCarreterasService = catCarreterasService;
            _catTramosService = catTramosService;
            _catDelegacionesOficinasTransporteService = catDelegacionesOficinasTransporteService;
            _oficialesService = oficialesService;
            _pdfService = pdfService;
            _capturaAccidentesService = capturaAccidentesService;
            _catDictionary = catDictionary;
        }
        #region DropDowns
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult Delegaciones_Drop()
        {
            var result = new SelectList(_catDelegacionesOficinasTransporteService.GetDelegacionesOficinasActivos(), "IdDelegacion", "Delegacion");
            return Json(result);
        }
        public JsonResult Carreteras_Drop()
        {
            var result = new SelectList(_catCarreterasService.ObtenerCarreteras(), "IdCarretera", "Carretera");
            return Json(result);
        }

        public JsonResult Tramos_Drop(int carreteraDDValue)
        {
            var result = new SelectList(_catTramosService.ObtenerTamosPorCarretera(carreteraDDValue), "IdTramo", "Tramo");
            return Json(result);
        }
        public JsonResult Oficiales_Drop()
        {
            var oficiales = _oficialesService.GetOficialesActivos()
                .Select(o => new
                {
                    IdOficial = o.IdOficial,
                    NombreCompleto = $"{o.Nombre} {o.ApellidoPaterno} {o.ApellidoMaterno}"
                });
            oficiales = oficiales.Skip(1);
            var result = new SelectList(oficiales, "IdOficial", "NombreCompleto");

            return Json(result);
        }
        #endregion

        public ActionResult ajax_BuscarAccidente(BusquedaAccidentesModel model)
        {
          var resultadoBusqueda = _busquedaAccidentesService.BusquedaAccidentes(model);
          return Json(resultadoBusqueda);

            
        }
        [HttpGet]
        public FileResult CreatePdfUnRegistro(int IdAccidente)
        {
            Dictionary<string, string> ColumnsNames = new Dictionary<string, string>()
            {
            {"numeroReporte","Folio"},
            {"municipio","Municipio"},
            {"carretera","Carretera"},
            {"nombrePropietarioCompleto","Propietario"},
            {"fecha","Fecha"},
            {"hora","Hora"},


            };
            var AccidenteModel = _busquedaAccidentesService.ObtenerAccidentePorId(IdAccidente);
            var result = _pdfService.CreatePdf("ReporteAccidente", "Accidentes", 6, ColumnsNames, AccidenteModel);
            return File(result.Item1, "application/pdf", result.Item2);
        }

        [HttpGet]
        public FileResult CreatePdf(string data)
        {
            var model = JsonConvert.DeserializeObject<BusquedaAccidentesModel>(data,
               new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            model.placasBusqueda = model.placasBusqueda == string.Empty ? null : model.placasBusqueda;
            model.serieBusqueda = model.serieBusqueda == string.Empty ? null : model.serieBusqueda;
            model.folioBusqueda = model.folioBusqueda == string.Empty ? null : model.folioBusqueda;
            model.propietarioBusqueda = model.propietarioBusqueda == string.Empty ? null : model.propietarioBusqueda;
            model.conductorBusqueda = model.conductorBusqueda == string.Empty ? null : model.conductorBusqueda;
            model.IdDelegacionBusqueda = model.IdDelegacionBusqueda == 0 ? null : model.IdDelegacionBusqueda;

            Dictionary<string, string> ColumnsNames = new Dictionary<string, string>()
            {
            {"municipio","Municipio"},
            {"tramo","Tramo"},
            {"carretera","Carretera"},
            {"kilometro","Kilometro"},
            {"fecha","Fecha" },
            {"hora","Hora" },
           
            };
            var ListTransitoModel = _busquedaAccidentesService.ObtenerAccidentes(model);
            var result = _pdfService.CreatePdf("ReporteAccidentes", "Accidentes", 6, ColumnsNames, ListTransitoModel);
            return File(result.Item1, "application/pdf", result.Item2);
        }

    }
}
