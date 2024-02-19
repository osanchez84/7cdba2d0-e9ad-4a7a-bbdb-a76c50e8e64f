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

[Authorize]
public class DepositosOtraDependenciaController : BaseController
{
    private readonly ICatDependenciaEnviaService _catDependenciaEnviaService;
    private readonly ICatTipoMotivoIngresoService _catTipoMotivoIngresoService;
    private readonly ICatMunicipiosService _catMunicipiosService;
    private readonly ICatCarreterasService _catCarreterasService;
    private readonly ICatTramosService _catTramosService;

    public DepositosOtraDependenciaController(ICatDependenciaEnviaService catDependenciaEnviaService, ICatTipoMotivoIngresoService catTipoMotivoIngresoService, ICatMunicipiosService catMunicipiosService, ICatCarreterasService catCarreterasService, ICatTramosService catTramosService)
    {
        _catDependenciaEnviaService = catDependenciaEnviaService;
        _catTipoMotivoIngresoService = catTipoMotivoIngresoService;
        _catMunicipiosService = catMunicipiosService;
        _catCarreterasService = catCarreterasService;
        _catTramosService = catTramosService;

    }
    public IActionResult Depositos(int? Isol)
    {
        return View("Depositos");
    }

    public JsonResult DependenciaEnvia_Drop()
    {
        var result = new SelectList(_catDependenciaEnviaService.ObtenerDependenciasEnviaActivas(), "id", "nombre");
        return Json(result);
    }

    public JsonResult TipoMotivoIngreso_Drop()
    {
        var result = new SelectList(_catTipoMotivoIngresoService.ObtenerTiposMotivoIngresoActivos(), "id", "nombre");
        return Json(result);
    }

    public JsonResult Municipios_Drop()
    {
        var result = new SelectList(_catMunicipiosService.GetMunicipiosPorEntidad(CatEntidadesModel.GUANAJUATO), "IdMunicipio", "Municipio");
        return Json(result);
    }
    public JsonResult Carreteras_Drop()
    {
        var result = new SelectList(_catCarreterasService.ObtenerCarreteras(), "IdCarretera", "Carretera");
        return Json(result);
    }

    public JsonResult Tramos_Drop(int carreteraDDValue)
    {
        var result = new SelectList(_catTramosService.ObtenerTamosPorCarretera(carreteraDDValue), "IdTramo", "Tramo");
        return Json(result);
    }
}