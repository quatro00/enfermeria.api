using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class ServicioFechasOfertum
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public Guid ServicioFechaId { get; set; }

    public Guid ColaboradorId { get; set; }

    public decimal MontoSolicitado { get; set; }

    public string Observaciones { get; set; } = null!;

    public int EstatusOfertaId { get; set; }

    public string Comentario { get; set; } = null!;

    public bool Activo { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public virtual Colaborador Colaborador { get; set; } = null!;

    public virtual CatServicioFechasOfertaEstatus EstatusOferta { get; set; } = null!;

    public virtual ServicioFecha ServicioFecha { get; set; } = null!;
}
