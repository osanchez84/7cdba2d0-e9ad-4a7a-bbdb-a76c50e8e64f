﻿using System;

namespace GuanajuatoAdminUsuarios.Models
{
    public class InfraccionesModel
    {
        public int? IdInfraccion { get; set; }
        public string FolioInfraccion { get; set; }
        public string Placas { get; set; }

        public int IdOficial { get; set; }
        public int IdDependencian { get; set; }
        public int IdDelegacion { get; set; }
        public string Oficial { get; set; }
        public string Municipio { get; set; }

        public DateTime? FechaInfraccion { get; set; }
        public string Carretera { get; set; }
        public string Tramo { get; set; }
        public string KmCarretera { get; set; }
        public int IdVehiculo { get; set; }
        public int IdConductor { get; set; }

        public string Conductor { get; set; }
        public string Propietario { get; set; }
        public int IdAplicacion { get; set; }
        public int InfraccionCortesia { get; set; }
        public int IdGarantia { get; set; }
        public int EstatusProceso { get; set; }

        public DateTime? FechaActualizacion { get; set; }
        public int ActualizadoPor { get; set; }
        public int Estatus { get; set; }

    }
}
