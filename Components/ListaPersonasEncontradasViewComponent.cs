/*
 * Descripción:
 * Proyecto: RIAG
 * Fecha de creación: Tuesday, February 27th 2024 12:36:56 pm
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Tue Feb 27 2024
 * Modificado por: Osvaldo S.
 * -----
 * Copyright (c) 2023 - 2024 Accesos Holográficos
 * -----
 * HISTORIAL:
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Components
{
    public class ListaPersonasEncontradasViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(List<PersonaModel> listaPersonas)
        {
            //var modelo = new VehiculoPropietarioBusquedaModel();
            return await Task.FromResult((IViewComponentResult)View("ListaPersonasEncontradas", listaPersonas));
        }
    }
}