using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;
using GuanajuatoAdminUsuarios.Utils;
using GuanajuatoAdminUsuarios.Framework;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class InfraccionesController : Controller
    {
        private readonly IEstatusInfraccionService _estatusInfraccionService;
        private readonly ITipoCortesiaService _tipoCortesiaService;
        private readonly IDependencias _dependeciaService;
        private readonly IDelegacionesService _delegacionesService;
        private readonly IGarantiasService _garantiasService;
        private readonly IInfraccionesService _infraccionesService;
        private readonly IPdfGenerator<Infracciones1Model> _pdfService;
        private readonly ICatDictionary _catDictionary;
        private readonly IVehiculosService _vehiculosService;
        private readonly IPersonasService _personasService;

        public InfraccionesController(
            IEstatusInfraccionService estatusInfraccionService, IDelegacionesService delegacionesService,
            ITipoCortesiaService tipoCortesiaService, IDependencias dependeciaService, IGarantiasService garantiasService,
            IInfraccionesService infraccionesService, IPdfGenerator<Infracciones1Model> pdfService,
            ICatDictionary catDictionary,
            IVehiculosService vehiculosService,
            IPersonasService personasService
           )
        {
            _catDictionary = catDictionary;
            _estatusInfraccionService = estatusInfraccionService;
            _tipoCortesiaService = tipoCortesiaService;
            _dependeciaService = dependeciaService;
            _delegacionesService = delegacionesService;
            _garantiasService = garantiasService;
            _infraccionesService = infraccionesService;
            _pdfService = pdfService;
            _vehiculosService = vehiculosService;
            _personasService = personasService;
        }

        public IActionResult Index()
        {

            InfraccionesBusquedaModel searchModel = new InfraccionesBusquedaModel();
            List<Infracciones1Model> listInfracciones = _infraccionesService.GetAllInfracciones();
            searchModel.ListInfracciones = listInfracciones;
            return View(searchModel);
        }

        [HttpPost]
        public ActionResult ajax_BuscarInfracciones(InfraccionesBusquedaModel model)
        {
            var listReporteAsignacion = _infraccionesService.GetAllInfracciones(model);
            return PartialView("_ListadoInfracciones", listReporteAsignacion);
        }

        [HttpGet]
        public FileResult CreatePdf(string data)
        {
            var model = JsonConvert.DeserializeObject<InfraccionesBusquedaModel>(data,
               new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

            model.folioInfraccion = model.folioInfraccion == string.Empty ? null : model.folioInfraccion;
            model.placas = model.placas == string.Empty ? null : model.placas;
            model.NumeroEconomico = model.NumeroEconomico == string.Empty ? null : model.NumeroEconomico;
            model.Conductor = model.Conductor == string.Empty ? null : model.Conductor;
            model.Propietario = model.Propietario == string.Empty ? null : model.Propietario;
            model.NumeroLicencia = model.NumeroLicencia == string.Empty ? null : model.NumeroLicencia;

            Dictionary<string, string> ColumnsNames = new Dictionary<string, string>()
            {
            {"folioInfraccion","Folio"},
            {"conductor","Conductor"},
            {"propietario","Propietario"},
            {"FullFechaConductor","Fecha Aplicada a"},
            {"garantia","Garantía"},
            {"delegacion","Delegación/Oficina"}
            };
            var ListTransitoModel = _infraccionesService.GetAllInfracciones(model);
            var result = _pdfService.CreatePdf("ReporteInfracciones", "Infracciones", 6, ColumnsNames, ListTransitoModel);
            return File(result.Item1, "application/pdf", result.Item2);
        }

        [HttpGet]
        public FileResult CreatePdfUnRegistro(int IdInfraccion)
        {
            Dictionary<string, string> ColumnsNames = new Dictionary<string, string>()
            {
            {"folioInfraccion","Folio"},
            {"conductor","Conductor"},
            {"propietario","Propietario"},
            {"FullFechaConductor","Fecha Aplicada a"},
            {"garantia","Garantía"},
            {"delegacion","Delegación/Oficina"}
            };
            var InfraccionModel = _infraccionesService.GetInfraccionById(IdInfraccion);
            var result = _pdfService.CreatePdf("ReporteInfracciones", "Infracciones", 6, ColumnsNames, InfraccionModel);
            return File(result.Item1, "application/pdf", result.Item2);
        }

        [HttpGet]
        public ActionResult Detail(int IdInfraccion)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Update(int IdInfraccion)
        {
            return View();
        }

        public JsonResult Estatus_Read()
        {
            var result = new SelectList(_estatusInfraccionService.GetEstatusInfracciones(), "idEstatusInfraccion", "estatusInfraccion");
            return Json(result);
        }

        public JsonResult Cortesias_Read()
        {
            //catTipoCortesia
            var result = new SelectList(_tipoCortesiaService.GetTipoCortesias(), "idTipoCortesia", "tipoCortesia");
            return Json(result);
        }

        public JsonResult Dependencias_Read()
        {
            var result = new SelectList(_dependeciaService.GetDependencias(), "IdDependencia", "NombreDependencia");
            return Json(result);
        }

        public JsonResult Garantias_Read()
        {
            //catGarantias
            var result = new SelectList(_garantiasService.GetGarantias(), "idGarantia", "garantia");
            return Json(result);
        }

        public JsonResult Delegaciones_Read()
        {
            var result = new SelectList(_delegacionesService.GetDelegaciones(), "IdDelegacion", "Delegacion");
            return Json(result);
        }

        public ActionResult Crear()
        {
            var catOficiales = _catDictionary.GetCatalog("CatOficiales", "0");
            var catMunicipios = _catDictionary.GetCatalog("CatMunicipios", "0");
            var catCarreteras = _catDictionary.GetCatalog("CatCarreteras", "0");
            var vehiculosList = _vehiculosService.GetAllVehiculos();
            var personasList = _personasService.GetAllPersonas();

            ViewBag.CatOficiales = new SelectList(catOficiales.CatalogList, "Id", "Text");
            ViewBag.CatMunicipios = new SelectList(catMunicipios.CatalogList, "Id", "Text");
            ViewBag.CatCarreteras = new SelectList(catCarreteras.CatalogList, "Id", "Text");
            ViewBag.Vehiculos = vehiculosList;
            ViewBag.Personas = personasList;
            return View(new InfraccionesModel());
        }

        public ActionResult Editar(int id)
        {
            var model = _infraccionesService.GetInfraccion2ById(id);
            model.isPropietarioConductor = model.Vehiculo.idPersona == model.idPersona;
            var catTramos = _catDictionary.GetCatalog("CatTramosByFilter", model.idCarretera.ToString());
            var catOficiales = _catDictionary.GetCatalog("CatOficiales", "0");
            var catMunicipios = _catDictionary.GetCatalog("CatMunicipios", "0");
            var catCarreteras = _catDictionary.GetCatalog("CatCarreteras", "0");
            var vehiculosList = _vehiculosService.GetAllVehiculos();
            var personasList = _personasService.GetAllPersonas();

            ViewBag.CatTramos = new SelectList(catTramos.CatalogList, "Id", "Text");
            ViewBag.CatOficiales = new SelectList(catOficiales.CatalogList, "Id", "Text");
            ViewBag.CatMunicipios = new SelectList(catMunicipios.CatalogList, "Id", "Text");
            ViewBag.CatCarreteras = new SelectList(catCarreteras.CatalogList, "Id", "Text");
            ViewBag.Vehiculos = vehiculosList;
            ViewBag.Personas = personasList;

            return View(model);
        }

        [HttpPost]
        public ActionResult ajax_crearInfraccion(InfraccionesModel model)
        {
            var idPersonaInfraccion = _infraccionesService.CrearPersonaInfraccion((int)model.idPersona);
            model.idPersonaInfraccion = idPersonaInfraccion;
            var idInfraccion = _infraccionesService.CrearInfraccion(model);
            return Json(new { id = idInfraccion });
        }


        [HttpGet]
        public ActionResult ajax_detalleVehiculo(int idVehiculo)
        {
            var model = _vehiculosService.GetVehiculoById(idVehiculo);
            return PartialView("_DetalleVehiculo", model);
        }

        [HttpGet]
        public ActionResult ajax_detallePersona(int idPersona)
        {
            var model = _personasService.GetPersonaById(idPersona);
            return PartialView("_DetallePersona", model);
        }

        [HttpGet]
        public ActionResult ajax_listadoVehiculoInfracciones()
        {
            var modelList = _vehiculosService.GetAllVehiculos();
            return PartialView("_ListadoVehiculos", modelList);
        }

        [HttpGet]
        public ActionResult ajax_listadoPersonasInfracciones()
        {
            var modelList = _personasService.GetAllPersonas();
            return PartialView("_ListadoPersonas", modelList);
        }


    }
}
