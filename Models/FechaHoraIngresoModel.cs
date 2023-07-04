using System;

namespace GuanajuatoAdminUsuarios.Models
{
    public class FechaHoraIngresoModel
    {
        public int IdPersona { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public TimeSpan? HoraIngreso { get; set; }

    }
}
