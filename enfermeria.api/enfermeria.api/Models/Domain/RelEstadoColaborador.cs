using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class RelEstadoColaborador
{
    public Guid Id { get; set; }

    public Guid ColaboradorId { get; set; }

    public Guid EstadoId { get; set; }

    public virtual Colaborador Colaborador { get; set; } = null!;

    public virtual CatEstado Estado { get; set; } = null!;
}
