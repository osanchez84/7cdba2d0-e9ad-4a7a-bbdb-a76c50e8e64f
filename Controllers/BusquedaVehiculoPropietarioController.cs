/*
 * Descripción:
 * Proyecto: RIAG
 * Fecha de creación: Tuesday, February 20th 2024 5:06:14 pm
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Tue Feb 20 2024
 * Modificado por: Osvaldo S.
 * -----
 * Copyright (c) 2023 - 2024 Accesos Holográficos
 * -----
 * HISTORIAL:
 */

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.RESTModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static GuanajuatoAdminUsuarios.RESTModels.CotejarDatosResponseModel;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class BusquedaVehiculoPropietarioController : BaseController
    {
         #region Variables
        #endregion
         public BusquedaVehiculoPropietarioController()
        {
        }

        [HttpPost]
        public ActionResult BuscarVehiculo([FromServices]IOptions<AppSettings> appSettings,
        [FromServices] IRepuveService repuveService,[FromServices] IVehiculosService vehiculoService,
        [FromServices] ICotejarDocumentosClientService cotejarDocumentosService,VehiculoBusquedaModel model)
        {
               var vehiculoModel = new VehiculoModel();

            RepuveConsgralRequestModel repuveGralModel = new(model.PlacasBusqueda, model.SerieBusqueda);

            ViewBag.ReporteRobo = vehiculoService.ValidarRobo(repuveGralModel);

            var buscarEnRepuve = appSettings.Value.AllowWebServicesRepuve;
             vehiculoModel = vehiculoService.GetVehiculoToAnexo(model);
            vehiculoModel.idSubmarcaUpdated = vehiculoModel.idSubmarca;
            vehiculoModel.PersonaMoralBusquedaModel = new PersonaMoralBusquedaModel
            {
                PersonasMorales = new List<PersonaModel>()
            };

            if (vehiculoModel.idVehiculo > 0)
            {
                return PartialView("_VehiculoPropietario", vehiculoModel);

            }

            if (buscarEnRepuve && !string.IsNullOrEmpty(model.PlacasBusqueda))
            {
                CotejarDatosRequestModel cotejarDatosRequestModel = new CotejarDatosRequestModel();
                cotejarDatosRequestModel.Tp_folio = "4";
                cotejarDatosRequestModel.Folio = model.PlacasBusqueda;
                cotejarDatosRequestModel.tp_consulta = "3";
                var endPointName = "CotejarDatosEndPoint";
                var result = cotejarDocumentosService.CotejarDatos(cotejarDatosRequestModel, endPointName);
                if (result.MT_CotejarDatos_res != null && result.MT_CotejarDatos_res.Es_mensaje != null && result.MT_CotejarDatos_res.Es_mensaje.TpMens.ToString().Equals("I", StringComparison.OrdinalIgnoreCase))
                {
                    vehiculoModel = GetVehiculoModelDeFinanzas(result);
                    return PartialView("_VehiculoPropietario", vehiculoModel);
                }
            }

            if (buscarEnRepuve)
            {
                var repuveConsGralResponse = repuveService.ConsultaGeneral(repuveGralModel)?.FirstOrDefault()?? new RepuveConsgralResponseModel();

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
                return PartialView("_VehiculoPropietario", vehiculoEncontrado);

            }
            
            return PartialView("_VehiculoPropietario",vehiculoModel);
        }

         public ActionResult MostrarBusquedaVehiculo(){
            return PartialView("_BusquedaVehiculoPropietario");
         }

         #region Catalogos y Vehicul Finanzas

         /// <summary>
/// Regresa objeto vehiculo a partir de un objeto de finanzas
/// </summary>
/// <param name="result"></param>
/// <returns></returns>
        public VehiculoModel GetVehiculoModelDeFinanzas(RootCotejarDatosRes result)
        {
            var vehiculoEncontradoData = result.MT_CotejarDatos_res.tb_vehiculo[0];
            var vehiculoDireccionData = result.MT_CotejarDatos_res.tb_direccion[0];
            var vehiculoInterlocutorData = result.MT_CotejarDatos_res;
            var idMunicipio = !string.IsNullOrEmpty(vehiculoDireccionData.municipio)
                  ? ObtenerIdMunicipioDesdeBD(vehiculoDireccionData.municipio)
                  : 0;

            var idEntidad = !string.IsNullOrEmpty(vehiculoDireccionData.entidadreg)
                ? ObtenerIdEntidadDesdeBD(vehiculoDireccionData.entidadreg)
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
            var generoBool = ConvertirGeneroBool(vehiculoInterlocutorData.es_per_fisica?.sexo);

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
         private int ObtenerIdMunicipioDesdeBD([FromServices]ICatMunicipiosService municipioService,string municipio)
        {
            var idMunicipio = municipioService.obtenerIdPorNombre(municipio);
            return idMunicipio;
        }
        private int ObtenerIdEntidadDesdeBD([FromServices]ICatEntidadesService entidadService,string entidad)
        {
            var idEntidad = entidadService.obtenerIdPorEntidad(entidad);
            return idEntidad;
        }
        private string ObtenerSubmarca(string submarca)
        {
            string[] partes = submarca.Split(new[] { '-' }, 2);

            if (partes.Length > 1)
            {
                string submarcaLimpio = partes[1].Trim();

                return submarcaLimpio;
            }

            return "NA"; // Valor predeterminado en caso de no encontrar el guión
        }

        private int ObtenerIdColor([FromServices]IColores colorService,string color)
        {
            string colorLimpio = Regex.Replace(color, "[0-9-]", "").Trim();
            var idColor = colorService.obtenerIdPorColor(colorLimpio);
            return (idColor);
        }
        private int ObtenerIdMarca([FromServices] ICatMarcasVehiculosService marcaVehiculoService,string marca)
        {
            string[] partes = marca.Split(new[] { '-' }, 2);

            if (partes.Length > 1)
            {
                string marcaLimpio = partes[1].Trim();

                var idMarca = marcaVehiculoService.obtenerIdPorMarca(marcaLimpio);
                return idMarca;
            }

            return 0; // Valor predeterminado en caso de no encontrar el guión
        }

        private int ObtenerIdSubmarca([FromServices] ICatSubmarcasVehiculosService submarcaVehiculoService,string submarca)
        {
            string[] partes = submarca.Split(new[] { '-' }, 2);

            if (partes.Length > 1)
            {
                string submarcaLimpio = partes[1].Trim();

                var idSubmarca = submarcaVehiculoService.obtenerIdPorSubmarca(submarcaLimpio);
                return idSubmarca;
            }

            return 0; // Valor predeterminado en caso de no encontrar el guión
        }
        private int ObtenerIdTipoVehiculo([FromServices]ICatDictionary catDictionary,string categoria)
        {

            int idTipo = 0;

            var tipoVehiculo = catDictionary.GetCatalog("CatTiposVehiculo", "0");

            idTipo = tipoVehiculo.CatalogList.Where(w => categoria.ToLower().Contains(w.Text.ToLower())).Select(s => s.Id).FirstOrDefault();

            return (idTipo);

        }

        private bool ConvertirBool(string carga)
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

        private bool ConvertirGeneroBool(string sexo)
        {
            if (sexo == "2")
            {
                return true;
            }
            else if (sexo == "1")
            {
                return false;
            }
            else
            {
                return false;
            }

        }
        private long LimpiarValorTelefono(string telefono)
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
