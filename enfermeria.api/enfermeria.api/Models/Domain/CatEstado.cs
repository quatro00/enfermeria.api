using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatEstado
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NombreCorto { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
