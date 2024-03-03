/*
 * Descripción:
 * Proyecto: RIAG
 * Fecha de creación: Tuesday, February 27th 2024 9:57:35 am
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
using GuanajuatoAdminUsuarios.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuanajuatoAdminUsuarios.Components
{
    public class BusquedaPersonaFisicaViewComponent : ViewComponent
    {
        /// <summary>
        /// Componente de busqueda de persona modal
        /// </summary>
        /// <param name="isModal"></param>
        /// <param name="persona"></param>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(bool isModal, PersonaModel persona)
        {
            List<PersonaModel> otrasPersonas = new();
            if (persona != null && persona.idCatTipoPersona ==1)
                otrasPersonas.Add(persona);
            persona.PersonaDireccion ??= new PersonaDireccionModel();
            var modelo = new BusquedaPersonaModel
            {
                IsModal = isModal,
                ListadoPersonasOtras = otrasPersonas
            };
            return await Task.FromResult((IViewComponentResult)View("BusquedaPersonaFisica", modelo));
        }
    }
}