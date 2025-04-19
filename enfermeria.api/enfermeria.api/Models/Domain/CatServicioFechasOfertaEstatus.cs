using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatServicioFechasOfertaEstatus
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<ServicioFechasOfertum> ServicioFechasOferta { get; set; } = new List<ServicioFechasOfertum>();
}
