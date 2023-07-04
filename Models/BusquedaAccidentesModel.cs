namespace GuanajuatoAdminUsuarios.Models
{
    public class BusquedaAccidentesModel
    {
        public int IdAccidente { get; set; }
        public string folioBusqueda { get; set; }
        public string Delegacion { get; set; }   
        public int IdDelegacionBusqueda { get; set; }
        public int IdCarreteraBusqueda { get; set; }
        public int IdTramoBusqueda { get; set; }
        public string placasBusqueda { get; set; }
        public string serieBusqueda { get; set; }
        public string propietarioBusqueda { get; set; }
        public string conductorBusqueda { get; set; }
        public int IdOficialBusqueda { get; set; }
        public int IdEstatusAccidente { get; set; }
        public string estatusAccidente { get; set; }

    }
}

