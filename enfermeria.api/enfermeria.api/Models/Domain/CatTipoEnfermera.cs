using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatTipoEnfermera
{
    public Guid TipoEnfermeraId { get; set; }

    public int No { get; set; }

    public string Descripcion { get; set; } = null!;

    public int Valor { get; set; }

    public decimal CostoHora { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Colaborador> Colaboradors { get; set; } = new List<Colaborador>();

    public virtual ICollection<Solicitud> Solicituds { get; set; } = new List<Solicitud>();
}
