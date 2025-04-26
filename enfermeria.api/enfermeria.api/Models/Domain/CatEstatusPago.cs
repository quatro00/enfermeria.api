using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatEstatusPago
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
