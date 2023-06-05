using System;
using System.ComponentModel.DataAnnotations;

namespace GuanajuatoAdminUsuarios.Models
{
    public class ColoresModel
    {
        public int IdColor { get; set; }

        [Required(ErrorMessage = "El nombre del color es obligatorio.")]
        public string color { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int? ActualizadoPor { get; set; }

        public int? Estatus { get; set; }

        public string estatusDesc { get; set; }

         public bool ValorEstatusColores { get; set; }

    }
}
