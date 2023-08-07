using System;
using System.ComponentModel.DataAnnotations;

namespace GuanajuatoAdminUsuarios.Models
{
    public class RecibosPagoWSRequestModel
    {
        public string UsuarioLog { get; set; }
        public string PasswordLog { get; set; }
        public int ReciboControlInterno { get; set; }
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}")]
        public DateTime FechaReversa { get; set; }
    }
}
