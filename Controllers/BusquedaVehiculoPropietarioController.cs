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

using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using GuanajuatoAdminUsuarios.RESTModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class BusquedaVehiculoPropietarioController : BaseController
    {
         #region Variables
         private readonly AppSettings _appSettings;
        private readonly IRepuveService _repuveService;
        #endregion
         public BusquedaVehiculoPropietarioController(IOptions<AppSettings> appSettings,IRepuveService repuveService)
        {
        }
        [HttpPost]
        public ActionResult Ajax_BuscarVehiculo(VehiculoBusquedaModel model)
        {
               var vehiculosModel = new VehiculoModel();

            RepuveConsgralRequestModel repuveGralModel = new RepuveConsgralRequestModel(model.PlacasBusqueda, model.SerieBusqueda);

            ViewBag.ReporteRobo = ValidarRobo(repuveGralModel);

            var allowSistem = _appSettings.AllowWebServicesRepuve;
            return PartialView("_VehiculoPropietario");
        }
  
    }
}
