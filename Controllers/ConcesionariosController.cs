using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using static GuanajuatoAdminUsuarios.Framework.Catalogs.CatEnumerator;


namespace GuanajuatoAdminUsuarios.Controllers
{

    [Authorize]
    public class ConcesionariosController : BaseController
    {
        private readonly IConcesionariosService _concesionariosService;
        private readonly ICatDictionary _catDictionary;

        public ConcesionariosController(IConcesionariosService concesionariosService, ICatDictionary catDictionary)
        {
            _concesionariosService = concesionariosService;
            _catDictionary = catDictionary;
        }
        public IActionResult Index()
        {
           
                var catMunicipios = _catDictionary.GetCatalog("CatMunicipios", "0");
                var catDelegaciones = _catDictionary.GetCatalog("CatDelegaciones", "0");
                var catConcesionario = _catDictionary.GetCatalog("CatConcesionarios", "0");

                ViewBag.CatMunicipios = new SelectList(catMunicipios.CatalogList, "Id", "Text");
                ViewBag.CatDelegaciones = new SelectList(catDelegaciones.CatalogList, "Id", "Text");
                ViewBag.CatConcesionario = new SelectList(catConcesionario.CatalogList, "Id", "Text");
               // var modelList = _concesionariosService.GetAllConcesionarios(idOficina);
                return View();
            
        }

        [HttpGet]
        public ActionResult ajax_BuscarConcesionario(int? idMunicipio, int? idDelegacion, int? idConcesionario)
        {
            int IdModulo = 291;
            string listaPermisosJson = HttpContext.Session.GetString("Autorizaciones");
            List<int> listaPermisos = JsonConvert.DeserializeObject<List<int>>(listaPermisosJson);
            if (listaPermisos != null && listaPermisos.Contains(IdModulo))
            {
                int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
                var listPadronGruas = _concesionariosService.GetConcecionariosBusqueda(idMunicipio, idOficina, idDelegacion, idConcesionario);

                return PartialView("_ListadoConcesionarios", listPadronGruas);
            }
            else
            {
                TempData["ErrorMessage"] = "El usuario no tiene permisos suficientes para esta acción.";
                return PartialView("ErrorPartial");
            }
        }


            [HttpGet]
        public IActionResult ajax_ModalCrearConcesionario()
        {
            int IdModulo = 293;
            string listaPermisosJson = HttpContext.Session.GetString("Autorizaciones");
            List<int> listaPermisos = JsonConvert.DeserializeObject<List<int>>(listaPermisosJson);
            if (listaPermisos != null && listaPermisos.Contains(IdModulo))
            {
                var catDelegaciones = _catDictionary.GetCatalog("CatDelegaciones", "0");
                var catMunicipios = _catDictionary.GetCatalog("CatMunicipios", "0");
                ViewBag.CatDelegaciones = new SelectList(catDelegaciones.CatalogList, "Id", "Text");
                ViewBag.CatMunicipios = new SelectList(catMunicipios.CatalogList, "Id", "Text");
                return PartialView("_CrearConcesionario", new Concesionarios2Model());
            }
        else
            {
                TempData["ErrorMessage"] = "El usuario no tiene permisos suficientes para esta acción.";
                return PartialView("ErrorPartial");
            }
        }

            [HttpPost]
            public IActionResult ajax_CrearConcesionario(Concesionarios2Model model)
            {
                //var model = json.ToObject<Gruas2Model>();
                var errors = ModelState.Values.Select(s => s.Errors);
                if (ModelState.IsValid)
                {
                    int index = _concesionariosService.CrearConcesionario(model);
                int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
                var listPadronGruas = _concesionariosService.GetAllConcesionarios(idOficina);
                    return PartialView("_ListadoConcesionarios", listPadronGruas);
                }
                return RedirectToAction("Index");
            }

            [HttpGet]
            public IActionResult ajax_ModalEditarConcesionario(int idConcesionario)
            {
            int IdModulo = 295;
            string listaPermisosJson = HttpContext.Session.GetString("Autorizaciones");
            List<int> listaPermisos = JsonConvert.DeserializeObject<List<int>>(listaPermisosJson);
            if (listaPermisos != null && listaPermisos.Contains(IdModulo))
            {
                var model = _concesionariosService.GetConcesionarioById(idConcesionario);
                    var catDelegaciones = _catDictionary.GetCatalog("CatDelegaciones", "0");
                    var catMunicipios = _catDictionary.GetCatalog("CatMunicipios", "0");
                    ViewBag.CatDelegaciones = new SelectList(catDelegaciones.CatalogList, "Id", "Text");
                    ViewBag.CatMunicipios = new SelectList(catMunicipios.CatalogList, "Id", "Text");
                    return PartialView("_EditarConcesionario", model);
                }
                     else
            {
                TempData["ErrorMessage"] = "El usuario no tiene permisos suficientes para esta acción.";
                return PartialView("ErrorPartial");
            }
        }

            [HttpPost]
            public IActionResult ajax_EditarConcesionario(Concesionarios2Model model)
            {
                var errors = ModelState.Values.Select(s => s.Errors);
                if (ModelState.IsValid)
                {
                    int index = _concesionariosService.EditarConcesionario(model);
                int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
                var listPadronGruas = _concesionariosService.GetAllConcesionarios(idOficina);
                    return PartialView("_ListadoConcesionarios", listPadronGruas);
                }
                return RedirectToAction("Index");
            }
        }
    }

