/*
 * Descripción:
 * Proyecto: Models
 * Fecha de creación: Sunday, February 18th 2024 10:44:54 am
 * Autor: Osvaldo S. (osvaldo.sanchez@zeitek.net)
 * -----
 * Última modificación: Sun Feb 18 2024
 * Modificado por: Osvaldo S.
 * -----
 * Copyright (c) 2023 - 2024 Accesos Holográficos
 * -----
 * HISTORIAL:
 */


using System;
using System.ComponentModel.DataAnnotations;

namespace GuanajuatoAdminUsuarios.Models{
public class SolicitudDepositoOtraDependenciaModel
    {

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime fechaSolicitud { get; set; }
        public TimeSpan horaSolicitud { get; set; }

        public int idDependenciaEnvia { get; set; }
    }
}