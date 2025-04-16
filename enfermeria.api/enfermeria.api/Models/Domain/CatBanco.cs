using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatBanco
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Colaborador> Colaboradors { get; set; } = new List<Colaborador>();
}
