﻿using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface ICatTiposVehiculosService
    {
        List<TiposVehiculosModel> GetTiposVehiculos();
        public int obtenerIdPorTipo(string categoria);
        public TiposVehiculosModel GetTipoVehiculoByID(int IdTipoVehiculo);
        public int CreateTipoVehiculo(TiposVehiculosModel model);
        public int UpdateTipoVehiculo(TiposVehiculosModel model);


    }
}
