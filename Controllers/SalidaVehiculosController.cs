using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class SalidaVehiculosController : BaseController
    {
        private readonly ISalidaVehiculosService _salidaVehiculosService;
        private readonly ICatMarcasVehiculosService _catMarcasVehiculosService;
        private readonly IMarcasVehiculos _marcaServices;
        private readonly IPlacaServices _placaServices;



        public SalidaVehiculosController(ISalidaVehiculosService salidaVehiculosService, ICatMarcasVehiculosService catMarcasVehiculosService,
            IMarcasVehiculos marcaServices, IPlacaServices placaServices)
        {
            _salidaVehiculosService = salidaVehiculosService;
            _catMarcasVehiculosService = catMarcasVehiculosService;
            _marcaServices = marcaServices;
            _placaServices = placaServices;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult Marcas_Read()
        {
            var result = new SelectList(_marcaServices.GetMarcas(), "IdMarcaVehiculo", "MarcaVehiculo");
            return Json(result);
        }
        public JsonResult Placas_Read()
        {
            int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
            var result = new SelectList(_placaServices.GetPlacasByDelegacionId(idOficina), "IdDepositos", "Placa");
            return Json(result);
        }
        public IActionResult ajax_BusquedaIngresos(SalidaVehiculosModel model)
        {
            var listaDepositos = _salidaVehiculosService.ObtenerIngresos(model);
            return Json(listaDepositos);
        }
        public IActionResult DatosDeposito(int iDp)
        {
            var infoDeposito = _salidaVehiculosService.DetallesDeposito(iDp);

            return View(infoDeposito);
        }


    }
}
