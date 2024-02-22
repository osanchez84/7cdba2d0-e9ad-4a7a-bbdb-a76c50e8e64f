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
                }
            };
            return PartialView("_BusquedaVehiculoPropietario", model);
        }
        #endregion

        #region BuscarVehiculoEnPlataformas
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

        public JsonResult GetSubtipoPorTipo_Drop([FromServices]ICatSubtipoServicio subtipoServicio,int idTipoServicio)
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
        public JsonResult Municipios_Drop([FromServices]ICatMunicipiosService _catMunicipiosService,int entidadDDlValue)
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
        public JsonResult TipoLicencias_Drop([FromServices]ICatTipoLicenciasService catTipoLicenciasService)
        {
            var result = new SelectList(catTipoLicenciasService.ObtenerTiposLicencia(), "idTipoLicencia", "tipoLicencia");
            return Json(result);
        }

        #endregion

    }
}
