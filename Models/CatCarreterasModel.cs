using System;

namespace GuanajuatoAdminUsuarios.Models
{
    public class CatCarreterasModel
    {
        public int IdCarretera { get; set; }

        public string Carretera { get; set; }

        public int IdDelegacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int? ActualizadoPor { get; set; }

        public int? Estatus { get; set; }

        public string estatusDesc { get; set; }
    }
}
