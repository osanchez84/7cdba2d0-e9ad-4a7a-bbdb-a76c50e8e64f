

using GuanajuatoAdminUsuarios.Entity;
using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IOficiales

    {
        List<Oficiales> GetOficiales();
        List<OficialesModel> GetOficialesActivos();

        Oficiales GetOficialById(int IdOficial);

        int SaveOficial(Oficiales oficial);

        int UpdateOficial(Oficiales oficial);

        int DeleteOficial(int oficial);

    }
}