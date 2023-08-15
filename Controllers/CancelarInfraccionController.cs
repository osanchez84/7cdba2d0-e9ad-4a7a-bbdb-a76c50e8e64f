using GuanajuatoAdminUsuarios.Entity;
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
using GuanajuatoAdminUsuarios.RESTModels;
using GuanajuatoAdminUsuarios.Services;

namespace GuanajuatoAdminUsuarios.Controllers
{

    public class CancelarInfraccionController : Controller
    {

        private readonly ICancelarInfraccionService _cancelarInfraccionService;
        private readonly IAnularDocumentoClientService _anularDocumentoCientService;


        public CancelarInfraccionController(ICancelarInfraccionService cancelarInfraccionService, IAnularDocumentoClientService anularDocumentoCientService)
        {
            _cancelarInfraccionService = cancelarInfraccionService;
            _anularDocumentoCientService = anularDocumentoCientService;
        }

        public IActionResult Index(CancelarInfraccionModel cancelarInfraccionService)
        {
            int IdModulo = 704;
            string listaIdsPermitidosJson = HttpContext.Session.GetString("IdsPermitidos");
            List<int> listaIdsPermitidos = JsonConvert.DeserializeObject<List<int>>(listaIdsPermitidosJson);
            if (listaIdsPermitidos != null && listaIdsPermitidos.Contains(IdModulo))
            {
                return View("CancelarInfraccion");
            }
            else
            {
                TempData["ErrorMessage"] = "Este usuario no tiene acceso a esta sección.";
                return RedirectToAction("Principal", "Inicio", new { area = "" });
            }
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
        public IActionResult IniciarCancelacion(CancelarInfraccionModel model, int IdInfraccion, string OficioRevocacion, string folioInfraccion)
        {
           
//var ListInfraccionesModel = _cancelarInfraccionService.CancelarInfraccionBD(IdInfraccion, OficioRevocacion);
          
                AnulacionDocumentoRequestModel requestModel = new AnulacionDocumentoRequestModel();
                 requestModel.DOCUMENTO = "TTO-PEC9999";
                 requestModel.USUARIO = "INNSJACOB";
                 requestModel.PASSWORD = "123456";

                var result = _anularDocumentoCientService.AnularDocumento(requestModel);
            return View("CancelarInfraccion");

        }
    }

}
