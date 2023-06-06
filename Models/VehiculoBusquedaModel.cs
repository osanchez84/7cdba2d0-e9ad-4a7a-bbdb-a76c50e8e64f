using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Models
{
    public class VehiculoBusquedaModel
    {
        public int? IdEntidadBusqueda { get; set; }
        public string PlacasBusqueda { get; set; } = null!;
        public string SerieBusqueda { get; set; } = null!;
        public VehiculoModel Vehiculo { get; set; }
    }
}
