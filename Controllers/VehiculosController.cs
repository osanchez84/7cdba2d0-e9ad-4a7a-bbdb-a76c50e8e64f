using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static GuanajuatoAdminUsuarios.Utils.CatalogosEnums;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class VehiculosController : Controller
    {
        private readonly ICatDictionary _catDictionary;
        private readonly IVehiculosService _vehiculosService;
        private readonly IPersonasService _personasService;

        public VehiculosController(IVehiculosService vehiculosService, ICatDictionary catDictionary,
            IPersonasService personasService
         )
        {
            _vehiculosService = vehiculosService;
            _catDictionary = catDictionary;
            _personasService = personasService;
        }


        public IActionResult Index()
        {
            VehiculoBusquedaModel vehiculoBusquedaModel = new VehiculoBusquedaModel();
            vehiculoBusquedaModel.Vehiculo = new VehiculoModel();
            vehiculoBusquedaModel.Vehiculo.PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel();
            vehiculoBusquedaModel.Vehiculo.PersonaMoralBusquedaModel.PersonasMorales = new List<PersonaModel>();
            return View(vehiculoBusquedaModel);
        }

        public JsonResult Entidades_Read()
        {
            var catEntidades = _catDictionary.GetCatalog("CatEntidades", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult TipoServicios_Read()
        {
            var catEntidades = _catDictionary.GetCatalog("CatTipoServicio", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult Colores_Read()
        {
            var catEntidades = _catDictionary.GetCatalog("CatColores", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult Marcas_Read()
        {
            var catEntidades = _catDictionary.GetCatalog("CatMarcasVehiculos", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult SubMarcas_Read()
        {
            var catEntidades = _catDictionary.GetCatalog("SubMarcas_Read", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult TiposVehiculo_Read()
        {
            var catEntidades = _catDictionary.GetCatalog("CatTiposVehiculo", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        [HttpPost]
        public ActionResult ajax_BuscarVehiculo(VehiculoBusquedaModel model)
        {
            var vehiculosModel = _vehiculosService.GetVehiculoToAnexo(model);
            vehiculosModel.PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel();
            vehiculosModel.PersonaMoralBusquedaModel.PersonasMorales = new List<PersonaModel>();
            return PartialView("_Create", vehiculosModel);
        }

        [HttpPost]
        public ActionResult ajax_BuscarPersonaMoral(PersonaMoralBusquedaModel PersonaMoralBusquedaModel)
        {
            PersonaMoralBusquedaModel.IdTipoPersona = (int)TipoPersona.Moral;
            var personasMoralesModel = _personasService.GetAllPersonasMorales(PersonaMoralBusquedaModel);
            return PartialView("_ListPersonasMorales", personasMoralesModel);
        }


    }
}
