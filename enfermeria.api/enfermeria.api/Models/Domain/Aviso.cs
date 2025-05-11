using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Aviso
{
    public Guid Id { get; set; }

    public DateTime Vigencia { get; set; }

    public string Titulo { get; set; } = null!;

    public string Texto { get; set; } = null!;

    public string Icono { get; set; } = null!;

    public string Color { get; set; } = null!;

    public Guid? ColaboradorId { get; set; }

    public string? Url { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public virtual Colaborador? Colaborador { get; set; }
}
