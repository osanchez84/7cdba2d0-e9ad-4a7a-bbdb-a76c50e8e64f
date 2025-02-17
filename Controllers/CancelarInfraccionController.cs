﻿using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using Kendo.Mvc.Extensions;
using GuanajuatoAdminUsuarios.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using static GuanajuatoAdminUsuarios.RESTModels.AnulacionDocumentoRequestModel;
using GuanajuatoAdminUsuarios.RESTModels;
using GuanajuatoAdminUsuarios.Services;
using Microsoft.AspNetCore.Authorization;

namespace GuanajuatoAdminUsuarios.Controllers
{


    [Authorize]
    public class CancelarInfraccionController : BaseController
    {

        private readonly ICancelarInfraccionService _cancelarInfraccionService;
        private readonly IAnulacionDocumentoService _anulacionDocumentoService;
		private readonly IBitacoraService _bitacoraServices;

		public CancelarInfraccionController(ICancelarInfraccionService cancelarInfraccionService, IAnulacionDocumentoService anulacionDocumentoService, IBitacoraService bitacoraService)
        {
            _cancelarInfraccionService = cancelarInfraccionService;
            _anulacionDocumentoService = anulacionDocumentoService;
            _bitacoraServices = bitacoraService;
        }

        public IActionResult Index(CancelarInfraccionModel cancelarInfraccionService)
        {
        
                return View("CancelarInfraccion");
            }



        [HttpPost]
        public ActionResult ObtenerInfracciones(CancelarInfraccionModel model, string FolioInfraccion)
        {

            var ListInfraccionesModel = _cancelarInfraccionService.ObtenerInfraccionPorFolio(FolioInfraccion);

            if (ListInfraccionesModel == null || ListInfraccionesModel.Count == 0)
            {
                TempData["ErrorNoCoinciencia"] = "No se encontraron infracciones con el folio especificado.";
                return PartialView("_ListadoCancelarInfraccion"); 
            }

            return PartialView("_ListadoCancelarInfraccion", ListInfraccionesModel);
        }


        public ActionResult MostrarDetalle(CancelarInfraccionModel model, int Id)
        {
            var ListInfraccionesModel = _cancelarInfraccionService.ObtenerDetalleInfraccion(Id);
            return PartialView("_DetalleInfraccion",ListInfraccionesModel);

        }

        [HttpPost]
        public IActionResult IniciarCancelacion(CancelarInfraccionModel model, int IdInfraccion, string OficioRevocacion)
        {

            var ListInfraccionesModel = _cancelarInfraccionService.CancelarInfraccionBD(IdInfraccion, OficioRevocacion);

			var ip = HttpContext.Connection.RemoteIpAddress.ToString();
			var user = Convert.ToDecimal(User.FindFirst(CustomClaims.IdUsuario).Value);

			_bitacoraServices.insertBitacora(IdInfraccion, ip, "CancelarInfraccion", "Cancelar", "delete", user);


			return View("CancelarInfraccion");
        }

        public IActionResult AnulacionDocumento(string folio_infraccion, int idOficina)
        {
            string prefijo = (idOficina == 1) ? "TTO-PEC" : (idOficina == 2) ? "TTE-M" : "";
            RootAnulacionDocumentoRequest rootRequest = new RootAnulacionDocumentoRequest();

            MT_Consulta_documento mTConsultaDocumento = new MT_Consulta_documento(); 
            mTConsultaDocumento.DOCUMENTO = prefijo+folio_infraccion;
            mTConsultaDocumento.USUARIO = "INNSJACOB";
            mTConsultaDocumento.PASSWORD = "123456";

            rootRequest.MT_Consulta_documento = mTConsultaDocumento;
             
            var result = _anulacionDocumentoService.CancelarMultasTransitoFinanzas(rootRequest);

			var ip = HttpContext.Connection.RemoteIpAddress.ToString();
			var user = Convert.ToDecimal(User.FindFirst(CustomClaims.IdUsuario).Value);




			ViewBag.Pension = result;
            return Json(result);
        }

    }

}
