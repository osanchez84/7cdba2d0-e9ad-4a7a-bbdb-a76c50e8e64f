using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Models
{
    public class BitacoraInfraccionesModel
    {

        public string folio { get; set; }

         public string operacion { get; set; }
         public string fecha { get; set; }
        public string hora { get; set; }
        public string ip { get; set; }
        public string nombre { get; set; }
        public string desc { get { 
            
                var indextest = new Dictionary<string,string>();

                indextest["CREAR"]= @"se registro infraccion con folio " + folio;
                indextest["Editar"]= @"se edito la infraccion con folio " + folio;
                indextest["Pagar"]= @"se pago la infraccion con folio " + folio;

                var t = indextest[this.operacion];

                return t;
            
            
            } }

    }
}
