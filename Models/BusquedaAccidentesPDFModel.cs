using System;
using System.ComponentModel.DataAnnotations;

namespace GuanajuatoAdminUsuarios.Models
{
    public class BusquedaAccidentesPDFModel
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaInicio { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime  FechaFin { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime fechaCompleta { get; set; }

        public int? idPropietario { get; set; }

        public string folioBusqueda { get; set; }
        public string Delegacion { get; set; }
        public int? IdDelegacionBusqueda { get; set; }
        public int? IdCarreteraBusqueda { get; set; }
        public int? IdTramoBusqueda { get; set; }
        public string placasBusqueda { get; set; }
        public string serieBusqueda { get; set; }
        public string propietarioBusqueda { get; set; }
        public string conductorBusqueda { get; set; }
        public int? IdOficialBusqueda { get; set; }
    }
}
