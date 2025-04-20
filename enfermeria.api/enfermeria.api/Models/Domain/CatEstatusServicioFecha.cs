using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatEstatusServicioFecha
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<ServicioFecha> ServicioFechas { get; set; } = new List<ServicioFecha>();
}
