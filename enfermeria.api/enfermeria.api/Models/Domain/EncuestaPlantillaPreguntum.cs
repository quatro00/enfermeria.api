using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class EncuestaPlantillaPreguntum
{
    public Guid Id { get; set; }

    public Guid PlantillaId { get; set; }

    public int Orden { get; set; }

    public string Texto { get; set; } = null!;

    public string? Tipo { get; set; }

    public bool Requerida { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual EncuestaPlantilla Plantilla { get; set; } = null!;
}
