using System.Collections.Generic;
using GuanajuatoAdminUsuarios.Entity;

namespace GuanajuatoAdminUsuarios.Interfaces
{
    public interface IColores
    {
        List<Colores> Getcoloress();

        Colores GetColorById(int IdColor);

        int SaveColor(Colores color);

        int UpdateColor(Colores color);

        int DeleteColor(int color);

    }
}
