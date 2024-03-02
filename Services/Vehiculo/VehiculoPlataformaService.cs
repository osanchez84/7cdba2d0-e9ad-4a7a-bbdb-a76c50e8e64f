/*
 * Descripción: Servicio de Vehiculo relacionado con algun servicio de REPUVE o Finanzas
 * Proyecto: RIAG
 * Fecha de creación: Wednesday, February 21st 2024 9:56:46 am
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Fri Mar 01 2024
 * Modificado por: Osvaldo S.
 * -----
 * Copyright (c) 2023 - 2024 Accesos Holográficos
 * -----
 * HISTORIAL:
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.RESTModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using static GuanajuatoAdminUsuarios.RESTModels.CotejarDatosResponseModel;

namespace GuanajuatoAdminUsuarios.Services
{
    public partial class VehiculoPlataformaService : IVehiculoPlataformaService
    {
        #region Variables
        private readonly IRepuveService _repuveService;
        private readonly ICatMunicipiosService _municipioService;
        private readonly ICatEntidadesService _entidadService;
        private readonly IColores _colorService;
        private readonly ICatMarcasVehiculosService _marcaVehiculoService;
        private readonly ICatSubmarcasVehiculosService _submarcaVehiculoService;
        private readonly ICatDictionary _catDictionary;
        private readonly IVehiculosService _vehiculoService;
        private readonly ICotejarDocumentosClientService _cotejarDocumentosClientService;
        private readonly AppSettings _appSettings;
        [GeneratedRegex("[0-9-]")]
        private static partial Regex MyRegex();
        #endregion

        public VehiculoPlataformaService(IRepuveService repuveService, ICatEntidadesService entidadService,ICatMunicipiosService municipioService, IColores colorService,
        ICatMarcasVehiculosService marcaVehiculoService, ICatSubmarcasVehiculosService submarcaVehiculoService, ICatDictionary catDictionary,IOptions<AppSettings> appSettings,IVehiculosService vehiculoService,ICotejarDocumentosClientService cotejarDocumentosClientService)
        {
            _repuveService = repuveService;
            _entidadService = entidadService;
            _municipioService = municipioService;
            _colorService = colorService;
            _marcaVehiculoService = marcaVehiculoService;
            _submarcaVehiculoService = submarcaVehiculoService;
            _catDictionary = catDictionary;
            _appSettings = appSettings.Value;
            _vehiculoService = vehiculoService;
            _cotejarDocumentosClientService =cotejarDocumentosClientService;

        }

         public VehiculoModel BuscarVehiculoEnPlataformas(VehiculoPropietarioBusquedaModel model)
        {
            try
            {
                List<VehiculoModel> listaVehiculos = new();
                var request = JsonConvert.SerializeObject(model);
               // Logger.Debug("Infracciones - ajax_BuscarVehiculo - Request:" + request);
                var vehiculoModel = new VehiculoModel();

                //Se realiza la consulta para validar si el vehiculo tiene reporte de robo
                RepuveConsgralRequestModel repuveGralModel = new(model.PlacaBusqueda, model.SerieBusqueda);
               // Logger.Debug("Infracciones - ajax_BuscarVehiculo - ValidarRobo ");
                RepuveRoboModel repuveRoboModel = new();

                try
                {
                    repuveRoboModel = ValidarRoboRepuve(repuveGralModel);
                }
                catch (Exception e)
                {
                   // Logger.Error("Ocurrió un error al consultar robados en REPUVE:" + e);
                }

VehiculoBusquedaModel busquedaModel = new()
            {
                IdEntidadBusqueda = model.IdEntidadBusqueda,
                PlacasBusqueda = model.PlacaBusqueda,
                SerieBusqueda = model.SerieBusqueda
            };

                vehiculoModel.RepuveRobo = repuveRoboModel;
                vehiculoModel.ReporteRobo = repuveRoboModel.EsRobado;

                var buscarEnServicios = _appSettings.AllowWebServicesRepuve;
                //Logger.Debug("Infracciones - ajax_BuscarVehiculo - GetVehiculoToAnexo");
                
                listaVehiculos = _vehiculoService.GetVehiculoPropietario(busquedaModel);
                
                if(listaVehiculos.Count>0){
                    return listaVehiculos;
                }
                
                vehiculoModel.idSubmarcaUpdated = vehiculoModel.idSubmarca;
                vehiculoModel.PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel();
                vehiculoModel.PersonaMoralBusquedaModel.PersonasMorales = new List<PersonaModel>();

                
                if (buscarEnServicios && !string.IsNullOrEmpty(busquedaModel.PlacasBusqueda))
                {
                    CotejarDatosRequestModel cotejarDatosRequestModel = new CotejarDatosRequestModel
                    {
                        Tp_folio = "4",
                        Folio = model.PlacaBusqueda,
                        tp_consulta = "3"
                    };
                    var endPointName = "CotejarDatosEndPoint";
                   // Logger.Debug("Infracciones - ajax_BuscarVehiculo - CotejarDatos");
                    var result = _cotejarDocumentosClientService.CotejarDatos(cotejarDatosRequestModel, endPointName);
                    if (result.MT_CotejarDatos_res != null && result.MT_CotejarDatos_res.Es_mensaje != null && result.MT_CotejarDatos_res.Es_mensaje.TpMens.ToString().Equals("I", StringComparison.OrdinalIgnoreCase))
                    {
                       // Logger.Debug("Infracciones - ajax_BuscarVehiculo - GetVEiculoModelFromFinanzas - Response - " + JsonConvert.SerializeObject(result));
                        vehiculoModel = GetVehiculoModelFromFinanzas(result);

                         //Se asigna el objeto de la consulta de robado a repuve
                    vehiculoModel.RepuveRobo = repuveRoboModel;
                    vehiculoModel.ReporteRobo = repuveRoboModel.EsRobado;

                        //Se establece el origen de datos
                        vehiculoModel.origenDatos = "Padrón Estatal";
                        listaVehiculos.Add(vehiculoModel);

                        return listaVehiculos;
                    }
                }

                if (buscarEnServicios)
                {
                    //Logger.Debug("Infracciones - ajax_BuscarVehiculo - ConsultaGeneral - REPUVE");
                    RepuveConsgralResponseModel repuveConsGralResponse = new();
                    try
                    {
                        repuveConsGralResponse = _repuveService.ConsultaGeneral(repuveGralModel).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {
                        //Logger.Error("Ocurrió un error al consultar vehículo en REPUVE:" + ex);
                    }
                    //Logger.Debug(" - Response - " + JsonConvert.SerializeObject(repuveConsGralResponse));
                    var idEntidad = !string.IsNullOrEmpty(repuveConsGralResponse.entidad_expide)
                          ? ObtenerIdEntidadRepuve(repuveConsGralResponse.entidad_expide)
                          : 0;
                    var idColor = !string.IsNullOrEmpty(repuveConsGralResponse.color)
                        ? ObtenerIdColor(repuveConsGralResponse.color)
                        : 0;

                    var idMarca = !string.IsNullOrEmpty(repuveConsGralResponse.marca_padron)
                        ? ObtenerIdMarcaRepuve(repuveConsGralResponse.marca_padron)
                        : 0;

                    var idSubmarca = !string.IsNullOrEmpty(repuveConsGralResponse.submarca_padron)
                        ? ObtenerIdSubmarcaRepuve(repuveConsGralResponse.submarca_padron)
                        : 0;
                    var submarcaLimpio = !string.IsNullOrEmpty(repuveConsGralResponse.submarca_padron)
                        ? ObtenerIdSubmarcaRepuve(repuveConsGralResponse.submarca_padron)
                        : 0;

                    var idTipo = !string.IsNullOrEmpty(repuveConsGralResponse.tipo_vehiculo_padron)
                     ? ObtenerIdTipoVehiculo(repuveConsGralResponse.tipo_vehiculo_padron)
                     : 0;
                    var idTipoServicio = !string.IsNullOrEmpty(repuveConsGralResponse.tipo_uso_padron)
                    ? ObtenerIdTipoServicioRepuve(repuveConsGralResponse.tipo_uso_padron)
                    : 0;

                    var vehiculoEncontrado = new VehiculoModel
                    {
                        placas = repuveConsGralResponse.placa.IsNullOrEmpty() ? repuveRoboModel.Placa : repuveConsGralResponse.placa,
                        serie = repuveConsGralResponse.niv_padron.IsNullOrEmpty() ? repuveRoboModel.Niv : repuveConsGralResponse.niv_padron,
                        // numeroEconomico = repuveConsGralResponse.tnia,
                        motor = repuveConsGralResponse.motor,
                        //otros = repuveConsGralResponse.
                        idColor = idColor,
                        idEntidad = idEntidad,
                        idMarcaVehiculo = idMarca,
                        idSubmarca = idSubmarca,
                        //submarca = submarcaLimpio,
                        idTipoVehiculo = idTipo,
                        modelo = repuveConsGralResponse.modelo,
                        idCatTipoServicio = idTipoServicio,
                        Persona = new PersonaModel(),
                        PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel(),
                        RepuveRobo = repuveRoboModel,
                        ReporteRobo = repuveRoboModel.EsRobado,
                        ErrorRepube = !repuveConsGralResponse.estatus.IsCaseInsensitiveEqual(RepuveConsgralResponseModel.CONSULTA_CORRECTA) ? "No" : ""
                    };

                    //Se establece el origen de datos
                    vehiculoEncontrado.origenDatos = string.IsNullOrEmpty(vehiculoEncontrado.placas) ? null : "REPUVE";





                    vehiculoEncontrado.placas = model.PlacasBusqueda ?? vehiculoEncontrado.placas;
                    vehiculoEncontrado.serie = model.SerieBusqueda ?? vehiculoEncontrado.serie;

                    return vehiculoEncontrado;

                }
                else
                {
                    Logger.Debug("Infracciones - ajax_BuscarVehiculo - ConsultaGeneral - REPUVE (BANDERA DESACTIVADA)");
                }


                return vehiculosModel;
            }
            catch (Exception ex)
            {
                Logger.Error("Infracciones - ajax_BuscarVehiculo: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Valida si un vehiculo tiene reporte de robo en REPUVE
        /// </summary>
        /// <param name="repuveGralModel"></param>
        /// <returns></returns>

        public RepuveRoboModel ValidarRoboRepuve(RepuveConsgralRequestModel repuveGralModel)
        {
            var repuveConsRoboResponse = _repuveService.ConsultaRobo(repuveGralModel)?.FirstOrDefault() ?? new RepuveRoboModel();
            return repuveConsRoboResponse;
        }
        #region Catalogos y Generacion de Objeto Vehiculo Finanzas

        /// <summary>
        /// Regresa objeto vehiculo a partir de un objeto de finanzas
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public VehiculoModel GetVehiculoModelFromFinanzas(RootCotejarDatosRes result)
        {
            var vehiculoEncontradoData = result.MT_CotejarDatos_res.tb_vehiculo[0];
            var vehiculoDireccionData = result.MT_CotejarDatos_res.tb_direccion[0];
            var vehiculoInterlocutorData = result.MT_CotejarDatos_res;
            var idMunicipio = !string.IsNullOrEmpty(vehiculoDireccionData.municipio)
                  ? _municipioService.obtenerIdPorNombre(vehiculoDireccionData.municipio)
                  : 0;

            var idEntidad = !string.IsNullOrEmpty(vehiculoDireccionData.entidadreg)
                ? _entidadService.obtenerIdPorEntidad(vehiculoDireccionData.entidadreg)
                : 0;

            var idColor = !string.IsNullOrEmpty(vehiculoEncontradoData.color)
                ? ObtenerIdColor(vehiculoEncontradoData.color)
                : 0;

            var idMarca = !string.IsNullOrEmpty(vehiculoEncontradoData.marca)
                ? ObtenerIdMarca(vehiculoEncontradoData.marca)
                : 0;

            var idSubmarca = !string.IsNullOrEmpty(vehiculoEncontradoData.linea)
                ? ObtenerIdSubmarca(vehiculoEncontradoData.linea)
                : 0;
            var submarcaLimpio = !string.IsNullOrEmpty(vehiculoEncontradoData.linea)
                ? ObtenerSubmarca(vehiculoEncontradoData.linea)
                : "NA";
            var telefonoValido = !string.IsNullOrEmpty(vehiculoDireccionData.telefono)
                ? LimpiarValorTelefono(vehiculoDireccionData.telefono)
                : 0;
            var cargaBool = ConvertirBool(vehiculoEncontradoData.carga);
            var generoBool = vehiculoInterlocutorData.es_per_fisica?.sexo == "2";

            var idTipo = !string.IsNullOrEmpty(vehiculoEncontradoData.categoria)
             ? ObtenerIdTipoVehiculo(vehiculoEncontradoData.categoria)
             : 0;

            var vehiculoEncontrado = new VehiculoModel
            {
                placas = vehiculoEncontradoData.no_placa,
                serie = vehiculoEncontradoData.no_serie,
                tarjeta = vehiculoEncontradoData.no_tarjeta,
                motor = vehiculoEncontradoData.no_motor,
                otros = vehiculoEncontradoData.otros,
                idColor = idColor,
                idEntidad = idEntidad,
                idMarcaVehiculo = idMarca,
                idSubmarca = idSubmarca,
                submarca = submarcaLimpio,
                idTipoVehiculo = idTipo,
                modelo = vehiculoEncontradoData.modelo,
                capacidad = vehiculoEncontradoData.numpersona,
                carga = cargaBool,

                Persona = new PersonaModel
                {
                    RFC = vehiculoInterlocutorData.Nro_rfc,
                    RFCFisico = vehiculoInterlocutorData.Nro_rfc,
                    CURPFisico = vehiculoInterlocutorData.es_per_fisica?.Nro_curp,
                    nombreFisico = vehiculoInterlocutorData.es_per_fisica?.Nombre,
                    apellidoPaternoFisico = vehiculoInterlocutorData.es_per_fisica?.Ape_paterno,
                    apellidoMaternoFisico = vehiculoInterlocutorData.es_per_fisica?.Ape_materno,
                    fechaNacimiento = vehiculoInterlocutorData.es_per_fisica?.Fecha_nacimiento,
                    generoBool = generoBool,

                    nombre = vehiculoInterlocutorData.es_per_moral?.name_org1,
                    PersonaDireccion = new PersonaDireccionModel
                    {
                        telefonoFisico = telefonoValido.ToString(),
                        telefono = telefonoValido.ToString(),
                        colonia = vehiculoDireccionData.colonia,
                        coloniaFisico = vehiculoDireccionData.colonia,
                        calleFisico = vehiculoDireccionData.calle,
                        calle = vehiculoDireccionData.calle,
                        numero = vehiculoDireccionData.nro_exterior,
                        numeroFisico = vehiculoDireccionData.nro_exterior,
                        idMunicipioFisico = idMunicipio,
                        idMunicipio = idMunicipio,
                    }
                },

                PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel
                {
                    PersonasMorales = new List<PersonaModel>()
                }
            };

            return vehiculoEncontrado;

        }


        private static string ObtenerSubmarca(string submarca)
        {
            string[] partes = submarca.Split(new[] { '-' }, 2);

            if (partes.Length > 1)
            {
                string submarcaLimpio = partes[1].Trim();

                return submarcaLimpio;
            }

            return "NA"; // Valor predeterminado en caso de no encontrar el guión
        }

        private int ObtenerIdColor(string color)
        {
            string colorLimpio = MyRegex().Replace(color, "").Trim();
            var idColor = _colorService.obtenerIdPorColor(colorLimpio);
            return idColor;
        }
        private int ObtenerIdMarca(string marca)
        {
            string[] partes = marca.Split(new[] { '-' }, 2);

            if (partes.Length > 1)
            {
                string marcaLimpio = partes[1].Trim();

                var idMarca = _marcaVehiculoService.obtenerIdPorMarca(marcaLimpio);
                return idMarca;
            }

            return 0; // Valor predeterminado en caso de no encontrar el guión
        }

        private int ObtenerIdSubmarca(string submarca)
        {
            string[] partes = submarca.Split(new[] { '-' }, 2);

            if (partes.Length > 1)
            {
                string submarcaLimpio = partes[1].Trim();

                var idSubmarca = _submarcaVehiculoService.obtenerIdPorSubmarca(submarcaLimpio);
                return idSubmarca;
            }

            return 0; // Valor predeterminado en caso de no encontrar el guión
        }
        private int ObtenerIdTipoVehiculo(string categoria)
        {

            int idTipo = 0;
            var tipoVehiculo = _catDictionary.GetCatalog("CatTiposVehiculo", "0");
            idTipo = tipoVehiculo.CatalogList.Where(w => categoria.ToLower().Contains(w.Text.ToLower())).Select(s => s.Id).FirstOrDefault();
            return idTipo;

        }

        private static bool ConvertirBool(string carga)
        {
            bool cargaBool = false;

            if (carga.Trim() == "1.00")
            {
                cargaBool = true;
            }
            else if (carga.Trim() == "0.00")
            {
                cargaBool = false;
            }
            return cargaBool;
        }

        private static long LimpiarValorTelefono(string telefono)
        {
            telefono = telefono.Replace(" ", "");
            if (long.TryParse(telefono, out long telefonoValido))
            {
                return telefonoValido;
            }
            else
            {
                return 0; // O algún otro valor que indique que no es válido
            }
        }
        #endregion
    }
}