﻿

using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IOficiales

    {
        List<CatOficialesModel> GetOficiales();
        List<CatOficialesModel> GetOficialesActivos(); 
        List<CatOficialesModel> GetOficialesFiltrados(int idOficina, int idDependencia);
        CatOficialesModel GetOficialById(int IdOficial);

        int SaveOficial(CatOficialesModel oficial);

        int UpdateOficial(CatOficialesModel oficial);

        int DeleteOficial(int oficial);

    }
}