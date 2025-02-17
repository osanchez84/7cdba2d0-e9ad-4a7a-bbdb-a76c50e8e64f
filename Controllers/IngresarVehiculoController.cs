﻿using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class IngresarVehiculoController : BaseController
    {
        private readonly IIngresarVehiculosService _ingresarVehiculosService;
        private readonly ICatMarcasVehiculosService _catMarcasVehiculosService;
        private readonly IPlacaServices _placaServices;
        private readonly ICatMunicipiosService _catMunicipiosService;
        private readonly ICatDescripcionesEventoService _descripcionesEventoService;


        public IngresarVehiculoController(IIngresarVehiculosService ingresarVehiculosService, ICatMarcasVehiculosService catMarcasVehiculosService,
            IPlacaServices placaServices, ICatMunicipiosService catMunicipiosService, ICatDescripcionesEventoService descripcionesEventoService)
        {
            _ingresarVehiculosService = ingresarVehiculosService;
            _catMarcasVehiculosService = catMarcasVehiculosService;
            _placaServices = placaServices;
            _catMunicipiosService = catMunicipiosService;
            _descripcionesEventoService = descripcionesEventoService;
        }
        public IActionResult Index()
        {
                return View();
        }

        
        public JsonResult Marcas_Drop()
        {
            var result = new SelectList(_catMarcasVehiculosService.ObtenerMarcas(), "IdMarcaVehiculo", "MarcaVehiculo");
            return Json(result);
        }
        public JsonResult Placas_Read()
        {
            int idOficina = HttpContext.Session.GetInt32("IdOficina") ?? 0;
            int idPension = HttpContext.Session.GetInt32("IdPension") ?? 0;

            var result = new SelectList(_placaServices.GetPlacasByDelegacionId(idPension), "IdDepositos", "Placa");
            return Json(result);
        }
        public JsonResult Municipios_Drop()
        {
            var result = new SelectList(_catMunicipiosService.GetMunicipios(), "IdMunicipio", "Municipio");
            return Json(result);
        }
        public JsonResult Descripcion_Drop()
        {
            var result = new SelectList(_descripcionesEventoService.ObtenerDescripciones(), "idDescripcion", "descripcionEvento");
            return Json(result);
        }
        [HttpPost]
        public IActionResult DirigirPorDependencia(string tipoIngreso)
        {
            if (tipoIngreso == "TransitoTransporte")
            {
                return Json(new { redirectTo = Url.Action("IngresoTransitoTransporte") });
            }
            else if (tipoIngreso == "OtraDependencia")
            {
                return Json(new { redirectTo = Url.Action("IngresoOtraDependencia") });
            }
            else
            {
                return Json(new { error = "Es necesario seleccionar una opción" });
            }
        }
        public IActionResult IngresoTransitoTransporte()
        {
            return View("IngresoTransitoTransporte");
        }
        public IActionResult IngresoOtraDependencia()
        {
            return View("IngresoOtraDependencia");
        }


        public IActionResult ajax_BusquedaDepositos(IngresoVehiculosModel model)
        {
           
                int idPension = HttpContext.Session.GetInt32("IdPension") ?? 0;

                var listaDepositos = _ingresarVehiculosService.ObtenerDepositos(model, idPension);
                return Json(listaDepositos);
            }
      
            public IActionResult GuardarRegistroSeleccionado(int idDeposito)
        {
            var infoDeposito = _ingresarVehiculosService.DetallesDeposito(idDeposito);
            return Json(infoDeposito);
        }
       /* [HttpPost]
        public IActionResult ajax_GuardarDatos(DatosIngresoModel datos)
        {
         var depositoModificado = _ingresarVehiculosService.GuardarFechaIngreso(datos);
            return Ok();

        }
       */

        [HttpPost]
        public ActionResult ajax_GuardarDatos(IFormFile AnexarImagen1, string data)
        {
            int result = 0;
            try
            {

                var model = JsonConvert.DeserializeObject<DatosIngresoModel>(data);
                if (AnexarImagen1 != null)
                {
                    using (var ms1 = new MemoryStream())
                    {
                        AnexarImagen1.CopyTo(ms1);
                        model.AnexarImagen1 = ms1.ToArray();
                    }
                }

                ////Prueba de que allmacena bien la imagen  
                ////var imgByte = model.AcreditacionPropiedad;
                ////return new FileContentResult(imgByte, "image/jpeg");
                result = _ingresarVehiculosService.GuardarFechaIngreso(model);

            }
            catch (Exception ex)
            {

            }
            if (result > 0)
            {
               // List<LiberacionVehiculoModel> ListProuctModel = _liberacionVehiculoService.GetAllTopDepositos();
                return Ok();
            }
            else
            {
                return Json(null);
            }
        }


    }
}
