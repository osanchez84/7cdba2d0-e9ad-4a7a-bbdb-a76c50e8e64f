using System.ComponentModel.DataAnnotations;
using System;

namespace GuanajuatoAdminUsuarios.Models
{
    public class EnvioInfraccionesModel

    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaInicio { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime FechaFin { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string Oficio { get; set; }
        public string IdLugarEnvio { get; set; }
        


    }
}
