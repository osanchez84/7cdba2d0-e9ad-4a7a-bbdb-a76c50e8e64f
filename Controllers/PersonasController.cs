using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GuanajuatoAdminUsuarios.Controllers
{
    public class PersonasController : Controller
    {
        private readonly ICatDictionary _catDictionary;
        private readonly IPersonasService _personasService;
        private readonly IHttpClientFactory _httpClientFactory;
        public PersonasController(ICatDictionary catDictionary, IPersonasService personasService, IHttpClientFactory httpClientFactory)
        {
            _catDictionary = catDictionary;
            _personasService = personasService;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            var catTipoPersona = _catDictionary.GetCatalog("CatTipoPersona", "0");

            ViewBag.CatTipoPersona = new SelectList(catTipoPersona.CatalogList, "Id", "Text");
            return View();
        }
        public IActionResult DetallesLicencia()
        {

            return View("_DetallesLicencia");
        }
        public async Task<ActionResult> BuscarPorParametroAsync(PersonaModel model)
        {
            if (!string.IsNullOrEmpty(model.numeroLicenciaBusqueda))
            {
                // Verificar si el número de licencia no está en la base de datos
                bool licenciaNoSITTEG = _personasService.VerificarLicenciaSitteg(model.numeroLicenciaBusqueda);

                if (licenciaNoSITTEG) {
                    try
                    {
                        var url = $"https://virtual.zeitek.net:9094/serviciosinfracciones/getdatoslicencia?userWS=1&claveWS=1&folioLicencia={model.numeroLicenciaBusqueda}";

                        var httpClient = _httpClientFactory.CreateClient();
                        var response = await httpClient.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            
                            List<ResultadoLicenciaModel> licencias = JsonConvert.DeserializeObject<List<ResultadoLicenciaModel>>(content);
                            LicenciaViewModel licenciaViewModel = new LicenciaViewModel
                            {
                                Nombre = licencias[0].Nombre, // Asignar el nombre (suponiendo que es el mismo en todas las licencias)
                                TiposLicencia = licencias.Select(l => l.TipoLicencia).ToList(),
                                Licencias = licencias.Select(l => new LicenciaInfo
                                {
                                    TipoLicencia = l.TipoLicencia,
                                    FechaExpedicion = l.FechaExpedicion,
                                    FechaVigencia = l.FechaVigencia
                                }).ToList()
                            };


                            return View("_DetalleLicencia",licenciaViewModel);
                        }
                        else
                        {
                            // En caso de respuesta no exitosa, manejar el error y devolver una vista de error o redireccionar a otra página.
                            return View("Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        // En caso de errores, manejar el error y devolver una vista de error o redireccionar a otra página.
                        return View("Error");
                    }
                }
            
                else
            {
                    var personasList = _personasService.BusquedaPersona(model);

                    return Json(personasList);
                }
        }else{
            var personasList = _personasService.BusquedaPersona(model);

            return Json(personasList);
        }
    }


        [HttpGet]
        public IActionResult ajax_ModalCrearPersona()
        {
            var catTipoPersona = _catDictionary.GetCatalog("CatTipoPersona", "0");
            var catTipoLicencia = _catDictionary.GetCatalog("CatTipoLicencia", "0");
            var catEntidades = _catDictionary.GetCatalog("CatEntidades", "0");
            var catGeneros = _catDictionary.GetCatalog("CatGeneros", "0");
            var catMunicipios = _catDictionary.GetCatalog("CatMunicipios", "0");

            ViewBag.CatMunicipios = new SelectList(catMunicipios.CatalogList, "Id", "Text");
            ViewBag.CatGeneros = new SelectList(catGeneros.CatalogList, "Id", "Text");
            ViewBag.CatEntidades = new SelectList(catEntidades.CatalogList, "Id", "Text");
            ViewBag.CatTipoPersona = new SelectList(catTipoPersona.CatalogList, "Id", "Text");
            ViewBag.CatTipoLicencia = new SelectList(catTipoLicencia.CatalogList, "Id", "Text");
            return PartialView("_CrearPersona", new PersonaModel());
        }

        [HttpPost]
        public IActionResult ajax_CrearPersona(PersonaModel model)
        {
            //var model = json.ToObject<Gruas2Model>();
            //var errors = ModelState.Values.Select(s => s.Errors);
            //if (ModelState.IsValid)
            //{
            int id = _personasService.CreatePersona(model);
            model.PersonaDireccion.idPersona = id;
            int idDireccion = _personasService.CreatePersonaDireccion(model.PersonaDireccion);

            var modelList = _personasService.GetAllPersonas();
            //var listPadronGruas = _concesionariosService.GetAllConcesionarios();
            return PartialView("_ListadoPersonas", modelList);
            //}
            //return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult ajax_ModalEditarPersona(int id)
        {
            var model = _personasService.GetPersonaById(id);
            var catTipoPersona = _catDictionary.GetCatalog("CatTipoPersona", "0");
            var catTipoLicencia = _catDictionary.GetCatalog("CatTipoLicencia", "0");
            var catEntidades = _catDictionary.GetCatalog("CatEntidades", "0");
            var catGeneros = _catDictionary.GetCatalog("CatGeneros", "0");
            var catMunicipios = _catDictionary.GetCatalog("CatMunicipios", "0");

            ViewBag.CatMunicipios = new SelectList(catMunicipios.CatalogList, "Id", "Text");
            ViewBag.CatGeneros = new SelectList(catGeneros.CatalogList, "Id", "Text");
            ViewBag.CatEntidades = new SelectList(catEntidades.CatalogList, "Id", "Text");
            ViewBag.CatTipoPersona = new SelectList(catTipoPersona.CatalogList, "Id", "Text");
            ViewBag.CatTipoLicencia = new SelectList(catTipoLicencia.CatalogList, "Id", "Text");
            return PartialView("_EditarPersona", model);
        }

        [HttpPost]
        public IActionResult ajax_EditarPersona(PersonaModel model)
        {
            //var model = json.ToObject<Gruas2Model>();
            var errors = ModelState.Values.Select(s => s.Errors);
            if (ModelState.IsValid)
            {
                if (model.PersonaDireccion.idPersona == null || model.PersonaDireccion.idPersona <= 0)
                {
                    model.PersonaDireccion.idPersona = model.idPersona;
                    int idDireccion = _personasService.CreatePersonaDireccion(model.PersonaDireccion);
                }
                else
                {
                    int idDireccion = _personasService.UpdatePersonaDireccion(model.PersonaDireccion);
                }
                int id = _personasService.UpdatePersona(model);
                var modelList = _personasService.GetAllPersonas();
                //var listPadronGruas = _concesionariosService.GetAllConcesionarios();
                return PartialView("_ListadoPersonas", modelList);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]

        public async Task<IActionResult> BusquedaPorLicencia(string numeroLicencia)
        {
            try
            {
                var url = $"https://virtual.zeitek.net:9094/serviciosinfracciones/getdatoslicencia?userWS=1&claveWS=1&folioLicencia={numeroLicencia}";

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    List<ResultadoLicenciaModel> licencias = JsonConvert.DeserializeObject<List<ResultadoLicenciaModel>>(content);

                    foreach (var licenciaInfo in licencias)
                    {
                        string nombreCompleto = licenciaInfo.Nombre;
                        string[] partesNombre = nombreCompleto.Split(' ');

                        string nombre = partesNombre[0]; 
                        string apellidoPaterno = partesNombre.Length > 1 ? partesNombre[1] : string.Empty; 
                        string apellidoMaterno = partesNombre.Length > 2 ? partesNombre[2] : string.Empty; 
                        string tipoLicencia = licenciaInfo.TipoLicencia;
                        DateTime fechaExpedicion = licenciaInfo.FechaExpedicion;
                        DateTime fechaVigencia = licenciaInfo.FechaVigencia;

                        ResultadoLicenciaModel persona = new ResultadoLicenciaModel
                        {
                            NumeroLicencia = numeroLicencia,
                            Nombre = nombre,
                            ApellidoPaterno = apellidoPaterno,
                            ApellidoMaterno = apellidoMaterno,
                            TipoLicencia = tipoLicencia,
                            FechaExpedicion = fechaExpedicion,
                            FechaVigencia = fechaVigencia,
                        };

                        _personasService.InsertarDesdeServicio(persona);
                    }
                    return Ok();

                }
                else
                {
                    // En caso de respuesta no exitosa, manejar el error y devolver una vista de error o redireccionar a otra página.
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                // En caso de errores, manejar el error y devolver una vista de error o redireccionar a otra página.
                return View("Error");
            }
        }
    }
}
