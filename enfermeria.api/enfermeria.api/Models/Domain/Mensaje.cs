using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Mensaje
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public string Nombre { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Mensaje1 { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }
}
