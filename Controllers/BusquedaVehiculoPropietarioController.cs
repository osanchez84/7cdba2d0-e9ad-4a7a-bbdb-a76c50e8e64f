/*
 * Descripción:
 * Proyecto: RIAG
 * Fecha de creación: Tuesday, February 20th 2024 5:06:14 pm
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Thu Feb 22 2024
 * Modificado por: Osvaldo S.
 * -----
 * Copyright (c) 2023 - 2024 Accesos Holográficos
 * -----
 * HISTORIAL:
 */


using System.Collections.Generic;
using System.Linq;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.RESTModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using static GuanajuatoAdminUsuarios.Utils.CatalogosEnums;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class BusquedaVehiculoPropietarioController : BaseController
    {
        #region Variables
        private readonly ICatDictionary _catDictionary;
        #endregion
        #region Constructor
        public BusquedaVehiculoPropietarioController(ICatDictionary catDictionary)
        {
            _catDictionary = catDictionary;
        }
        #endregion

        #region MostrarBusqueda
        public ActionResult MostrarBusquedaVehiculo()
        {
            VehiculoPropietarioBusquedaModel model = new()
            {
                Vehiculo = new VehiculoModel
                {
                    Persona = new PersonaModel()
                },
                IdEntidadBusqueda = CatEntidadesModel.GUANAJUATO
            };
            return PartialView("_BusquedaVehiculoPropietario", model);
        }
        #endregion

        #region Vehiculo
        [HttpPost]
        public ActionResult BuscarVehiculoEnPlataformas([FromServices] IOptions<AppSettings> appSettings, [FromServices] IRepuveService repuveService,
        [FromServices] IVehiculoPlataformaService vehiculoPlataformaService, [FromServices] IVehiculosService vehiculoService, [FromServices] ICotejarDocumentosClientService cotejarDocumentosService, VehiculoPropietarioBusquedaModel model)
        {

            RepuveConsgralRequestModel repuveGralModel = new(model.PlacaBusqueda, model.SerieBusqueda);

            ViewBag.ReporteRobo = vehiculoPlataformaService.ValidarRoboRepuve(repuveGralModel);

            var buscarEnServicios = appSettings.Value.AllowWebServicesRepuve;
            VehiculoBusquedaModel busquedaModel = new()
            {
                IdEntidadBusqueda = model.IdEntidadBusqueda,
                PlacasBusqueda = model.PlacaBusqueda,
                SerieBusqueda = model.SerieBusqueda
            };

            VehiculoModel vehiculoModel = vehiculoService.GetVehiculoToAnexo(busquedaModel);
            vehiculoModel.idSubmarcaUpdated = vehiculoModel.idSubmarca;
            vehiculoModel.PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel
            {
                PersonasMorales = new List<PersonaModel>()
            };

            if (vehiculoModel.idVehiculo > 0)
            {
                return PartialView("_Vehiculo", vehiculoModel);

            }

            if (buscarEnServicios && !string.IsNullOrEmpty(busquedaModel.PlacasBusqueda))
            {
                CotejarDatosRequestModel cotejarDatosRequestModel = new()
                {
                    Tp_folio = "4",
                    Folio = busquedaModel.PlacasBusqueda,
                    tp_consulta = "3"
                };
                var endPointName = "CotejarDatosEndPoint";
                var result = cotejarDocumentosService.CotejarDatos(cotejarDatosRequestModel, endPointName);
                if (result.MT_CotejarDatos_res != null && result.MT_CotejarDatos_res.Es_mensaje != null && result.MT_CotejarDatos_res.Es_mensaje.TpMens.ToString().Equals("I", StringComparison.OrdinalIgnoreCase))
                {
                    vehiculoModel = vehiculoPlataformaService.GetVehiculoModelFromFinanzas(result);
                    vehiculoModel.ErrorRepube = string.IsNullOrEmpty(vehiculoModel.placas) ? "No" : "";
                    //Se establece el origen de datos
                    vehiculoModel.origenDatos = "Padrón Estatal";
                    return PartialView("_Vehiculo", vehiculoModel);
                }
            }

            if (buscarEnServicios)
            {
                var repuveConsGralResponse = repuveService.ConsultaGeneral(repuveGralModel)?.FirstOrDefault() ?? new RepuveConsgralResponseModel();

                var vehiculoEncontrado = new VehiculoModel
                {
                    placas = repuveConsGralResponse.placa,
                    serie = repuveConsGralResponse.niv_padron,
                    motor = repuveConsGralResponse.motor,
                    color = repuveConsGralResponse.color,
                    submarca = repuveConsGralResponse.submarca,
                    modelo = repuveConsGralResponse.modelo,

                    Persona = new PersonaModel(),

                    PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel(),
                };
                vehiculoEncontrado.ErrorRepube = string.IsNullOrEmpty(vehiculoEncontrado.placas) ? "No" : "";

                //Se establece el origen de datos
                vehiculoEncontrado.origenDatos = string.IsNullOrEmpty(vehiculoEncontrado.placas) ? null : "REPUVE";
                return PartialView("_Vehiculo", vehiculoEncontrado);

            }

            return PartialView("_Vehiculo", vehiculoModel);
        }
        #endregion

        #region Propietarios
        /// <summary>
        /// Muestra vista para crear persona fisica
        /// </summary>
        /// <returns></returns>
        public ActionResult MostrarPersonaFisica()
        {
            var model = new PersonaModel
            {
                PersonaDireccion = new PersonaDireccionModel()
            };
            return PartialView("_PersonaFisica", model);
        }
        /// <summary>
        /// Muestra vista para crear persona moral
        /// </summary>
        /// <returns></returns>
        public ActionResult MostrarPersonaMoral()
        {
            var model = new PersonaModel
            {
                PersonaDireccion = new PersonaDireccionModel()
            };
            return PartialView("_PersonaMoral", model);
        }

        /// <summary>
        /// Busca todas las personas morales
        /// </summary>
        /// <param name="personasService"></param>
        /// <param name="PersonaMoralBusquedaModel"></param>
        /// <returns></returns>
        public ActionResult BuscarPersonaMoral([FromServices] IPersonasService personasService, PersonaMoralBusquedaModel PersonaMoralBusquedaModel)
        {
            PersonaMoralBusquedaModel.IdTipoPersona = (int)TipoPersona.Moral;
            var personasMoralesModel = personasService.GetAllPersonasMorales(PersonaMoralBusquedaModel);
            return PartialView("_ListPersonasMorales", personasMoralesModel);
        }

        /// <summary>
        /// Busca todas las personas fisicas
        /// </summary>
        /// <param name="personasService"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BuscarPersonasFiscas([FromServices] IPersonasService personasService)
        {
            var personasFisicas = personasService.GetAllPersonas();
            return PartialView("_PersonasFisicas", personasFisicas);
        }
        /// <summary>
        /// Crea un nuevo registro en la bd de una persona fisica
        /// </summary>
        /// <param name="personasService"></param>
        /// <param name="Persona"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ActionResult CrearPersonaFisica([FromServices] IPersonasService personasService, PersonaModel Persona)
        {
            Persona.nombre = Persona.nombreFisico;
            Persona.apellidoMaterno = Persona.apellidoMaternoFisico;
            Persona.apellidoPaterno = Persona.apellidoPaternoFisico;
            Persona.CURP = Persona.CURPFisico;
            Persona.RFC = Persona.RFCFisico;
            Persona.numeroLicencia = Persona.numeroLicenciaFisico;
            Persona.idTipoLicencia = Persona.idTipoLicencia;
            Persona.vigenciaLicencia = Persona.vigenciaLicencia;
            Persona.PersonaDireccion.idEntidad = Persona.PersonaDireccion.idEntidadFisico;
            Persona.PersonaDireccion.idMunicipio = Persona.PersonaDireccion.idMunicipioFisico;
            Persona.PersonaDireccion.correo = Persona.PersonaDireccion.correoFisico;
            Persona.PersonaDireccion.telefono = Persona.PersonaDireccion.telefonoFisico;
            Persona.PersonaDireccion.colonia = Persona.PersonaDireccion.coloniaFisico;
            Persona.PersonaDireccion.calle = Persona.PersonaDireccion.calleFisico;
            Persona.PersonaDireccion.numero = Persona.PersonaDireccion.numeroFisico;
            Persona.idCatTipoPersona = (int)TipoPersona.Fisica;
            var IdPersonaFisica = personasService.CreatePersona(Persona);
            if (IdPersonaFisica == 0)
            {
                throw new Exception("Ocurrio un error al dar de alta la persona");
            }
            var modelList = personasService.ObterPersonaPorIDList(IdPersonaFisica); ;
            return PartialView("_PersonasFisicas", modelList);
        }
        /// <summary>
        /// Crea un nuevo registro en la bd de una persona moral
        /// </summary>
        /// <param name="personasService"></param>
        /// <param name="Persona"></param>
        /// <returns></returns>
        public ActionResult CrearPersonaMoral([FromServices] IPersonasService personasService, PersonaModel Persona)
        {
            Persona.idCatTipoPersona = (int)TipoPersona.Moral;
            Persona.PersonaDireccion.telefono = System.String.IsNullOrEmpty(Persona.telefono) ? null : Persona.telefono;
            var IdPersonaMoral = personasService.CreatePersonaMoral(Persona);
            if (IdPersonaMoral == 0)
                return Json(new { success = false, message = "Ocurrió un error al procesar su solicitud." });
            else
            {
                var modelList = personasService.ObterPersonaPorIDList(IdPersonaMoral);
                return PartialView("_ListPersonasMorales", modelList);
            }


            //var personasMoralesModel = _personasService.GetAllPersonasMorales();

        }

        #endregion
        #region Catalogos
        public JsonResult GetEntidades_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatEntidades", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }
        public JsonResult SubTipoServicios_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatSubtipoServicio", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult GetSubtipoPorTipo_Drop([FromServices] ICatSubtipoServicio subtipoServicio, int idTipoServicio)
        {
            var result = new SelectList(subtipoServicio.GetSubtipoPorTipo(idTipoServicio), "idSubTipoServicio", "subTipoServicio");
            return Json(result);
        }
        public JsonResult Entidades_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatEntidades", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult TipoServicios_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatTipoServicio", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }
        public JsonResult Municipios_Drop([FromServices] ICatMunicipiosService _catMunicipiosService, int entidadDDlValue)
        {
            var result = new SelectList(_catMunicipiosService.GetMunicipiosPorEntidad(entidadDDlValue), "IdMunicipio", "Municipio");
            return Json(result);
        }
        public JsonResult Colores_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatColores", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }

        public JsonResult Marcas_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatMarcasVehiculos", "0");
            var orderedList = catEntidades.CatalogList.OrderBy(item => item.Text);
            var result = new SelectList(orderedList, "Id", "Text"); return Json(result);
        }

        public JsonResult SubMarcas_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatSubmarcasVehiculos", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            //var selected = result.Where(x => x.Value == Convert.ToString(idSubmarca)).First();
            //selected.Selected = true;
            return Json(result);
        }

        public JsonResult TiposVehiculo_Drop()
        {
            var catEntidades = _catDictionary.GetCatalog("CatTiposVehiculo", "0");
            var result = new SelectList(catEntidades.CatalogList, "Id", "Text");
            return Json(result);
        }
        public JsonResult TipoLicencias_Drop([FromServices] ICatTipoLicenciasService catTipoLicenciasService)
        {
            var result = new SelectList(catTipoLicenciasService.ObtenerTiposLicencia(), "idTipoLicencia", "tipoLicencia");
            return Json(result);
        }

        #endregion

    }
}
