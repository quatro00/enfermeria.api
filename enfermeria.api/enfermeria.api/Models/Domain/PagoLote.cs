using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class PagoLote
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public string Etiqueta { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public string Csv { get; set; } = null!;

    public int EstatosPagoLoteId { get; set; }

    public bool Activo { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual CatEstatusPagoLote EstatosPagoLote { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
