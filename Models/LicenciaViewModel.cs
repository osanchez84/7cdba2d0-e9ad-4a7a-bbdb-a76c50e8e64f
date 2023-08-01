using System.Collections.Generic;
using System;

public class LicenciaViewModel
{
    public string Nombre { get; set; }
    public List<string> TiposLicencia { get; set; }
    public List<LicenciaInfo> Licencias { get; set; }
}

public class LicenciaInfo
{
    public string TipoLicencia { get; set; }
    public DateTime FechaExpedicion { get; set; }
    public DateTime FechaVigencia { get; set; }
}
