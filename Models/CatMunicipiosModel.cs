using System;

namespace GuanajuatoAdminUsuarios.Models
{
    public class CatMunicipiosModel
    {
        public int IdMunicipio { get; set; }

        public string Municipio { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int? ActualizadoPor { get; set; }

        public int? Estatus { get; set; }

        public string estatusDesc { get; set; }

    }
}
