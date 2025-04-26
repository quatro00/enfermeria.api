using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatEstatusPagoLote
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<PagoLote> PagoLotes { get; set; } = new List<PagoLote>();
}
