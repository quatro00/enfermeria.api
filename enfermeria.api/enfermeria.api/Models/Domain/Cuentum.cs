using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Cuentum
{
    public Guid Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public Guid? ProveedorId { get; set; }

    public Guid? TipoEnfermeraId { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreadionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual CatTipoEnfermera? TipoEnfermera { get; set; }
}
