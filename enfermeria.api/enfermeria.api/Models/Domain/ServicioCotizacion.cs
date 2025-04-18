using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class ServicioCotizacion
{
    public Guid Id { get; set; }

    public Guid ServicioFechasId { get; set; }

    public decimal Horas { get; set; }

    public string Horario { get; set; } = null!;

    public decimal PrecioHoraBase { get; set; }

    public decimal PrecioHoraFinal { get; set; }

    public decimal PrecioFinal { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual ServicioFecha ServicioFechas { get; set; } = null!;
}
