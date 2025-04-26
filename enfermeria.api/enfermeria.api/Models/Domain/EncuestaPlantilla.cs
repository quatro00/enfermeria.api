using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class EncuestaPlantilla
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? TipoServicio { get; set; }

    public bool Activo { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual ICollection<EncuestaPlantillaPreguntum> EncuestaPlantillaPregunta { get; set; } = new List<EncuestaPlantillaPreguntum>();
}
