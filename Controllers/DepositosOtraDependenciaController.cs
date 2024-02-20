/*
 * Descripción:
 * Proyecto: Controllers
 * Fecha de creación: Sunday, February 18th 2024 9:40:13 am
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Sun Feb 18 2024
 * Modificado por: Osvaldo S.
 * -----
 * Copyright (c) 2023 - 2024 Accesos Holográficos
 * -----
 * HISTORIAL:
 */

using GuanajuatoAdminUsuarios.Controllers;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GuanajuatoAdminUsuarios.Controllers
{
    [Authorize]
    public class DepositosOtraDependenciaController : BaseController
    {
        #region Variables

        #endregion

        #region Constructor
        public DepositosOtraDependenciaController()
        {
        }
        #endregion
        public IActionResult Depositos(int? Isol)
        {
            return View("Depositos");
        }

        #region BusquedaCatalogos
        public JsonResult DependenciaEnvia_Drop([FromServices] ICatDependenciaEnviaService catDependenciaEnviaService)
        {
            var result = new SelectList(catDependenciaEnviaService.ObtenerDependenciasEnviaActivas(), "id", "nombre");
            return Json(result);
        }

        public JsonResult TipoMotivoIngreso_Drop([FromServices] ICatTipoMotivoIngresoService catTipoMotivoIngresoService)
        {
            var result = new SelectList(catTipoMotivoIngresoService.ObtenerTiposMotivoIngresoActivos(), "id", "nombre");
            return Json(result);
        }

        public JsonResult Municipios_Drop([FromServices] ICatMunicipiosService catMunicipiosService)
        {
            var result = new SelectList(catMunicipiosService.GetMunicipiosPorEntidad(CatEntidadesModel.GUANAJUATO), "IdMunicipio", "Municipio");
            return Json(result);
        }
        public JsonResult Carreteras_Drop([FromServices] ICatCarreterasService catCarreterasService)
        {
            var result = new SelectList(catCarreterasService.ObtenerCarreteras(), "IdCarretera", "Carretera");
            return Json(result);
        }

        public JsonResult Tramos_Drop([FromServices] ICatTramosService catTramosService, int carreteraDDValue)
        {
            var result = new SelectList(catTramosService.ObtenerTamosPorCarretera(carreteraDDValue), "IdTramo", "Tramo");
            return Json(result);
        }
        #endregion
    }
}