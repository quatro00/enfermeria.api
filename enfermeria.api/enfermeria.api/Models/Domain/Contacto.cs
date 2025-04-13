using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Contacto
{
    public Guid Id { get; set; }

    public Guid PacienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Parentezco { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;
}
