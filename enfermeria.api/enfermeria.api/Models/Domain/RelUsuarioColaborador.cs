using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class RelUsuarioColaborador
{
    public Guid Id { get; set; }

    public Guid ColaboradorId { get; set; }

    public string UsuarioId { get; set; } = null!;

    public virtual Colaborador Colaborador { get; set; } = null!;

    public virtual AspNetUser Usuario { get; set; } = null!;
}
