/*
 * Descripción:
 * Proyecto: Sistema de Infracciones y Accidentes
 * Fecha de creación: Monday, February 19th 2024 4:09:18 pm
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Mon Feb 19 2024
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
using Microsoft.Extensions.Options;

namespace GuanajuatoAdminUsuarios.Services
{
    public class VehiculoBaseService : IVehiculoBaseService
    {
        private readonly IRepuveService _repuveService;
        private readonly AppSettings _appSettings;
        public VehiculoBaseService(IRepuveService repuveService, IOptions<AppSettings> appSettings)
        {
            _repuveService = repuveService;
            _appSettings = (AppSettings)appSettings;
        }

        /*public VehiculoModel ajax_BuscarVehiculo(VehiculoBusquedaModel model)
        {

            var vehiculoModel = new VehiculoModel();
            RepuveConsgralRequestModel repuveGralModel = new RepuveConsgralRequestModel(model.PlacasBusqueda, model.SerieBusqueda);


            //Variable que indica si se debe realizar la busqueda en REPUVE, es una configuración del sistema configurada en appSettings
            var buscarEnRepuve = _appSettings.AllowWebServicesRepuve;

            //Se busca en la base de datos riag si existe algún vehiculo con los parámetros de búsqueda
            vehiculoModel = _vehiculosService.GetVehiculoToAnexo(model);
            vehiculoModel.idSubmarcaUpdated = vehiculosModel.idSubmarca;
            vehiculoModel.PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel();
            vehiculoModel.PersonaMoralBusquedaModel.PersonasMorales = new List<PersonaModel>();

                        //Se valida si un vehiculo tiene reporte de robo
            vehiculoModel.tieneReporteRobo = ValidarRobo(repuveGralModel);

            if (vehiculosModel.idVehiculo > 0)
            {
                return PartialView("_Create", vehiculosModel);

            }

            if (buscarEnRepuve && !string.IsNullOrEmpty(model.PlacasBusqueda))
            {
                CotejarDatosRequestModel cotejarDatosRequestModel = new CotejarDatosRequestModel();
                cotejarDatosRequestModel.Tp_folio = "4";
                cotejarDatosRequestModel.Folio = model.PlacasBusqueda;
                cotejarDatosRequestModel.tp_consulta = "3";
                var endPointName = "CotejarDatosEndPoint";
                var result = _cotejarDocumentosClientService.CotejarDatos(cotejarDatosRequestModel, endPointName);
                if (result.MT_CotejarDatos_res != null && result.MT_CotejarDatos_res.Es_mensaje != null && result.MT_CotejarDatos_res.Es_mensaje.TpMens.ToString().Equals("I", StringComparison.OrdinalIgnoreCase))
                {
                    vehiculosModel = GetVEiculoModelFromFinanzas(result);
                    return PartialView("_Create", vehiculosModel);
                }
            }

            if (allowSistem)
            {
                var repuveConsGralResponse = _repuveService.ConsultaGeneral(repuveGralModel)?.FirstOrDefault() ?? new RepuveConsgralResponseModel();

                var vehiculoEncontrado = new VehiculoModel
                {
                    placas = repuveConsGralResponse.placa,
                    serie = repuveConsGralResponse.niv_padron,
                    //tarjeta = repuveConsGralResponse.ta,
                    motor = repuveConsGralResponse.motor,
                    //otros = repuveConsGralResponse.
                    color = repuveConsGralResponse.color,
                    // Entidad = repuveConsGralResponse.entidad_expide,
                    //MarcaVehiculo = repuveConsGralResponse.marca_padron,
                    //idSubmarca = idSubmarca,
                    submarca = repuveConsGralResponse.submarca,
                    //idTipoVehiculo = idTipo,
                    modelo = repuveConsGralResponse.modelo,
                    //capacidad = repuveConsGralResponse.c,
                    //carga = repuveConsGralResponse.ca,

                    Persona = new PersonaModel(),

                    PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel(),
                };
                return PartialView("_Create", vehiculoEncontrado);

            }

            return PartialView("_Create", vehiculosModel);

        }*/

        /// <summary>
        /// Valida si un vehiculo tiene reporte de robo en REPUVE
        /// </summary>
        /// <param name="repuveGralModel"></param>
        /// <returns></returns>

        public bool ValidarRobo(RepuveConsgralRequestModel repuveGralModel)
        {
            var repuveConsRoboResponse = _repuveService.ConsultaRobo(repuveGralModel)?.FirstOrDefault() ?? new RepuveConsRoboResponseModel();
            return repuveConsRoboResponse.estatus == "1";
        }
    }
}