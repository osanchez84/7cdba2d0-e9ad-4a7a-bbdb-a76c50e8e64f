using System;

namespace GuanajuatoAdminUsuarios.RESTModels
{
    public class AnulacionDocumentoResponseModel
    {
        public AnulacionDocumentoChild MT_AnulacionDocumento_res { get; set; }
    }
    public class AnulacionDocumentoChild
    {
        public resultModel result { get; set; }
    }

    public class resultModel
    {
        public string WTYPE { get; set; }
        public string WMESSAGE { get; set; }
        public string DOCTO_CANCEL { get; set; }
    }
}