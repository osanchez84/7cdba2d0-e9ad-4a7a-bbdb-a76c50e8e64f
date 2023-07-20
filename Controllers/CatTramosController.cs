using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class CatTramosController : Controller
    {
        private readonly ICatTramosService _catTramosService;
        private readonly ICatCarreterasService _catCarreterasService;


        public CatTramosController(ICatTramosService catTramosService, ICatCarreterasService catCarreterasService)
        {
            _catTramosService = catTramosService;
            _catCarreterasService = catCarreterasService;
        }
        public IActionResult Index()
        {
            int IdModulo = 10;
            // Obtener la cadena JSON de la variable de sesión
            string listaIdsPermitidosJson = HttpContext.Session.GetString("IdsPermitidos");

            // Deserializar la cadena JSON a una lista de enteros
            List<int> listaIdsPermitidos = JsonConvert.DeserializeObject<List<int>>(listaIdsPermitidosJson);

            // Verificar si el IdModulo está contenido en la lista de Ids permitidos
            if (listaIdsPermitidos != null && listaIdsPermitidos.Contains(IdModulo))
            {
                // Si el IdModulo está contenido en la lista de Ids permitidos,
                // continuar con la lógica actual

                var ListTramosModel = _catTramosService.ObtenerTramos();
                return View(ListTramosModel);
            }
            else
            {
                // Si el IdModulo no está contenido en la lista de Ids permitidos,
                // redireccionar al usuario a la vista "Marca"
                return View("Marca");
            }
        }

        public JsonResult Carreteras_Drop()
        {
            var result = new SelectList(_catCarreterasService.ObtenerCarreteras(), "IdCarretera", "Carretera");
            return Json(result);
        }
        [HttpPost]
        public ActionResult MostrarModalAgregarTramo()
        {
            //SetDDLDependencias();
            return PartialView("_Crear");
        }

        public ActionResult EditarTramoModal(int IdTramo)
        {
            var TramosModel = _catTramosService.ObtenerTramoByID(IdTramo);
            return PartialView("_Editar", TramosModel);

        }

        [HttpPost]
        public ActionResult CrearTramoMod(CatTramosModel model)
        {

            var errors = ModelState.Values.Select(s => s.Errors);
            if (ModelState.IsValid)
            {

                _catTramosService.CrearTramo(model);
                var TramosModel = _catTramosService.ObtenerTramos();
                return PartialView("_ListaTramos", TramosModel);
            }

            return PartialView("_Crear");
        }

        public ActionResult EditarTramoBD(CatTramosModel model)
        {
            bool switchTramoss = Request.Form["tramosSwitch"].Contains("true");
            model.Estatus = switchTramoss ? 1 : 0;
            var errors = ModelState.Values.Select(s => s.Errors);
            if (ModelState.IsValid)
            {


                _catTramosService.EditarTramo(model);
                var TramosModel = _catTramosService.ObtenerTramos();
                return PartialView("_ListaTramos", TramosModel);
            }
            return PartialView("_Editar");
        }

        public JsonResult GetTra([DataSourceRequest] DataSourceRequest request)
        {
            var ListtramosModel = _catTramosService.ObtenerTramos();

            return Json(ListtramosModel.ToDataSourceResult(request));
        }
    }
}
