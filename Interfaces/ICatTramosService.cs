using GuanajuatoAdminUsuarios.Models;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface ICatTramosService

    {

        List<CatTramosModel> ObtenerTamosPorCarretera(int carreteraDDValue);


    }
}
