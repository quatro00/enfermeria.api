using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class ServicioFecha
{
    public Guid Id { get; set; }

    public Guid ServicioId { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaTermino { get; set; }

    public decimal CantidadHoras { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? UsuarioModificacion { get; set; }

    public virtual Servicio Servicio { get; set; } = null!;
}
