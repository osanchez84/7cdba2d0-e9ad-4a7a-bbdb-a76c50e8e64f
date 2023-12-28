﻿using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IBusquedaAccidentesService
    {
        List<BusquedaAccidentesModel> BusquedaAccidentes(BusquedaAccidentesModel model, int idOficina, int idDependencia);
       List<BusquedaAccidentesModel> GetAllAccidentes(int idOficina, int idDependencia);
        List<BusquedaAccidentesPDFModel> BusquedaAccidentes(BusquedaAccidentesPDFModel model, int idOficina);
        public BusquedaAccidentesPDFModel ObtenerAccidentePorId(int idAccidente);
        List<BusquedaAccidentesModel> ObtenerAccidentes(BusquedaAccidentesModel model);


    }
}
