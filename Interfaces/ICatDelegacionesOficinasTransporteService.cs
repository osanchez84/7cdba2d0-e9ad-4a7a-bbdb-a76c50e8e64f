using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface ICatDelegacionesOficinasTransporteService
    {
        List<CatDelegacionesOficinasTransporteModel> GetDelegacionesOficinas();
        List<CatDelegacionesOficinasTransporteModel> GetDelegacionesOficinasActivos();
        List<CatDelegacionesOficinasTransporteModel> GetDelegacionesOficinasFiltrado(int idDependencia);
        public List<CatDelegacionesOficinasTransporteModel> GetDelegacionesDropDown();


            public string GetDelegacionOficinaById(int idOficina);
        public int EditarDelegacion(CatDelegacionesOficinasTransporteModel model);



    }
}
