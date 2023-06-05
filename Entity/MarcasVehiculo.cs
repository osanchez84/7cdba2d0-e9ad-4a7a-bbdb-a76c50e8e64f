using System;
using System.Collections.Generic;

namespace GuanajuatoAdminUsuarios.Entity;

public partial class MarcasVehiculo
{
    public int IdMarcaVehiculo { get; set; }

    public string MarcaVehiculo { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public int? ModificadoPor { get; set; }

    public int? Estatus { get; set; }

    public virtual ICollection<SubmarcasVehiculo> SubmarcasVehiculos { get; } = new List<SubmarcasVehiculo>();

}
