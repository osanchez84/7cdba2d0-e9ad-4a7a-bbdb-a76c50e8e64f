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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class DepositosOtraDependenciaController : BaseController
{
    private readonly ICatDependenciaEnviaService _catDependenciaEnviaService;

    public DepositosOtraDependenciaController(ICatDependenciaEnviaService catDependenciaEnviaService)
    {
        _catDependenciaEnviaService = catDependenciaEnviaService;

    }
    public IActionResult Depositos(int? Isol)
    {
        return View("Depositos");
    }

    public JsonResult DependenciaEnvia_Drop()
    {
        var result = new SelectList(_catDependenciaEnviaService.ObtenerDependenciasEnviaActivas(),"id","nombre");
        return Json(result);
    }

}