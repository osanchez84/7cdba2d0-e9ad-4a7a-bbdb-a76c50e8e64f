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

namespace GuanajuatoAdminUsuarios.Controllers
{

    public class RegistroReciboPagoController : Controller
    {

        private readonly IRegistroReciboPagoService _registroReciboPagoService;
      
    public  RegistroReciboPagoController(IRegistroReciboPagoService registroReciboPagoService)
        {
            _registroReciboPagoService= registroReciboPagoService;
        }

    public IActionResult Index()
        {
            int IdModulo = 705;
            string listaIdsPermitidosJson = HttpContext.Session.GetString("IdsPermitidos");
            List<int> listaIdsPermitidos = JsonConvert.DeserializeObject<List<int>>(listaIdsPermitidosJson);
            if (listaIdsPermitidos != null && listaIdsPermitidos.Contains(IdModulo))
            {
                return View("RegistroReciboDePago") ;
        }
            else
            {
                TempData["ErrorMessage"] = "Este usuario no tiene acceso a esta sección.";
                return RedirectToAction("Principal", "Inicio", new { area = "" });
            }
        }


        [HttpPost]
        public ActionResult ObtenerInfracciones(RegistroReciboPagoModel model,string FolioInfraccion)
        {
            var ListInfraccionesModel = _registroReciboPagoService.ObtInfracciones(FolioInfraccion);
            return PartialView("_ListadoBusquedaInfraccion", ListInfraccionesModel);

        }

        public ActionResult Detalleinfraccion(RegistroReciboPagoModel model, int Id)
        {
            var ListInfraccionesModel = _registroReciboPagoService.ObtenerDetallePorId(Id);
            return PartialView("_DetalleRegistroDePago", ListInfraccionesModel);

        }
        public ActionResult GuardarReciboPago(string ReciboPago, float Monto, DateTime FechaPago, string LugarPago, int IdInfraccion)
        {
            var datosGuardados = _registroReciboPagoService.GuardarRecibo(ReciboPago, Monto,FechaPago, LugarPago,IdInfraccion);
            return PartialView("RegistroReciboDePago");

        }
        
    }
   
}
