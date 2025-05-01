using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Pago
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public Guid PagoLoteId { get; set; }

    public Guid ServicioFechaId { get; set; }

    public decimal ImporteBruto { get; set; }

    public decimal Comision { get; set; }

    public decimal Retencion { get; set; }

    public decimal CostoOperativo { get; set; }

    public decimal Descuento { get; set; }

    public decimal Total { get; set; }

    public int EstatusPagoId { get; set; }

    public DateTime? FechaPago { get; set; }

    public string? Comprobante { get; set; }

    public string? Referencia { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual CatEstatusPago EstatusPago { get; set; } = null!;

    public virtual PagoLote PagoLote { get; set; } = null!;

    public virtual ServicioFecha ServicioFecha { get; set; } = null!;
}
