using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IBusquedaEspecialAccidentesService
    {

        bool UpdateFolio(string id, string folio);
        List<BusquedaEspecialAccidentesModel> BusquedaAccidentes(BusquedaEspecialAccidentesModel model, int idOficina,int idDependencia);
        List<BusquedaAccidentesPDFModel> BusquedaAccidentes(BusquedaAccidentesPDFModel model, int idOficina, int idDependencia);
        public BusquedaAccidentesPDFModel ObtenerAccidentePorId(int idAccidente);
        List<BusquedaEspecialAccidentesModel> ObtenerAccidentes(BusquedaEspecialAccidentesModel model);
        public int EliminarSeleccionado(int idAccidente);
        List<BusquedaEspecialAccidentesModel> ObtenerTodosAccidentes(int idOficina, int idDependencia);
        
    }
}
