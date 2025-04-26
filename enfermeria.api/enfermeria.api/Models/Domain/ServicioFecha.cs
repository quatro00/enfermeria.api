using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class ServicioFecha
{
    public Guid Id { get; set; }

    public Guid ServicioId { get; set; }

    public Guid? ColaboradorAsignadoId { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaTermino { get; set; }

    public int EstatusServicioFechaId { get; set; }

    public decimal CantidadHoras { get; set; }

    public decimal ImporteBruto { get; set; }

    public decimal ImporteSolicitado { get; set; }

    public decimal Comision { get; set; }

    public decimal CostosOperativos { get; set; }

    public decimal Retenciones { get; set; }

    public decimal Total { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public string? UsuarioModificacion { get; set; }

    public virtual Colaborador? ColaboradorAsignado { get; set; }

    public virtual CatEstatusServicioFecha EstatusServicioFecha { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Servicio Servicio { get; set; } = null!;

    public virtual ICollection<ServicioCotizacion> ServicioCotizacions { get; set; } = new List<ServicioCotizacion>();

    public virtual ICollection<ServicioFechasOfertum> ServicioFechasOferta { get; set; } = new List<ServicioFechasOfertum>();
}
