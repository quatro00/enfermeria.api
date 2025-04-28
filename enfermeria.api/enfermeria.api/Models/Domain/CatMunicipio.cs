using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatMunicipio
{
    public Guid Id { get; set; }

    public Guid EstadoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string NombreCorto { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual CatEstado Estado { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
