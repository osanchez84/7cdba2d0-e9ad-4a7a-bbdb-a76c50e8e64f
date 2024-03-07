/*
 * Descripción:
 * Proyecto: RIAG
 * Fecha de creación: Tuesday, February 27th 2024 12:36:56 pm
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Sat Mar 02 2024
 * Modificado por: Osvaldo S.
 * -----
 * Copyright (c) 2023 - 2024 Accesos Holográficos
 * -----
 * HISTORIAL:
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using GuanajuatoAdminUsuarios.Interfaces;
using GuanajuatoAdminUsuarios.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Components
{
    public class ListaPersonasEncontradasViewComponent : ViewComponent
    {
        private readonly IPersonasService _personasService;
        public ListaPersonasEncontradasViewComponent(IPersonasService personasService){
            _personasService = personasService;
        }
        public async Task<IViewComponentResult> InvokeAsync(BusquedaPersonaModel model)
        {
            List<PersonaModel> listaPersonas = model.ListadoPersonas;
            if (model.ListadoPersonas==null || model.ListadoPersonas.Count==0)
             listaPersonas = _personasService.BuscarPersonasWithPagination(model, model.Pagination);

            return await Task.FromResult((IViewComponentResult)View("ListaPersonasEncontradas", listaPersonas ));
        }
    }
}