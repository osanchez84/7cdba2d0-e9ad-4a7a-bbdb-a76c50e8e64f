namespace GuanajuatoAdminUsuarios.RESTModels
{
    public class ConsultarDocumentoResponseModel
    {
        public class MTConsultarDocumentoRes
        {
            public Result result { get; set; }
        }

        public class Result
        {
            public string NUM_DOCUMENTO { get; set; }
            public string FOL_MULTA { get; set; }
            public string FECHA_PAGO { get; set; }
            public string OFICINA { get; set; }
            public double IMPORTE { get; set; }
            public string CONCEPTO { get; set; }
            public string WTYPE { get; set; }
            public string WMESSAGE { get; set; }
        }

        public class RootConsultarDocumentoResponse
        {
            public MTConsultarDocumentoRes MT_ConsultarDocumento_res { get; set; }
        }
    }
}
