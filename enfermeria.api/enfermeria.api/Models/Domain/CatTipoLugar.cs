using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatTipoLugar
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
