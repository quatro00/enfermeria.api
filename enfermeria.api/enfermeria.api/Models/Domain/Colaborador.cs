using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Colaborador
{
    public Guid Id { get; set; }

    public string? UserId { get; set; }

    public int No { get; set; }

    public string Avatar { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string Rfc { get; set; } = null!;

    public string Curp { get; set; } = null!;

    public string CedulaProfesional { get; set; } = null!;

    public string DomicilioCalle { get; set; } = null!;

    public string DomicilioNumero { get; set; } = null!;

    public string Cp { get; set; } = null!;

    public string Colonia { get; set; } = null!;

    public int BancoId { get; set; }

    public string Clabe { get; set; } = null!;

    public string Cuenta { get; set; } = null!;

    public int EstatusColaboradorId { get; set; }

    public Guid TipoEnfermeraId { get; set; }

    public bool CuentaCreada { get; set; }

    public decimal Comision { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacionId { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacionId { get; set; }

    public virtual CatBanco Banco { get; set; } = null!;

    public virtual ICollection<ColaboradorDocumento> ColaboradorDocumentos { get; set; } = new List<ColaboradorDocumento>();

    public virtual EstatusColaborador EstatusColaborador { get; set; } = null!;

    public virtual ICollection<RelEstadoColaborador> RelEstadoColaboradors { get; set; } = new List<RelEstadoColaborador>();

    public virtual ICollection<ServicioFecha> ServicioFechas { get; set; } = new List<ServicioFecha>();

    public virtual ICollection<ServicioFechasOfertum> ServicioFechasOferta { get; set; } = new List<ServicioFechasOfertum>();

    public virtual CatTipoEnfermera TipoEnfermera { get; set; } = null!;

    public virtual AspNetUser? User { get; set; }
}
