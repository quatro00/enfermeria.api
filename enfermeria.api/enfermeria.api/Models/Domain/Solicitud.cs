using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Solicitud
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public Guid PacienteId { get; set; }

    public DateTime Vigencia { get; set; }

    public int EstatusSolicitudId { get; set; }

    public string PrincipalRazon { get; set; } = null!;

    public string RequiereAyudaBasica { get; set; } = null!;

    public string RequiereAyudaBasicaDesc { get; set; } = null!;

    public bool EnfermedadDiagnosticiada { get; set; }

    public string EnfermedadDiagnosticadaDesc { get; set; } = null!;

    public bool TomaMedicamento { get; set; }

    public string TomaMedicamentoDesc { get; set; } = null!;

    public bool DispositivosMedicos { get; set; }

    public string DispositivosMedicosDesc { get; set; } = null!;

    public bool RequiereCuraciones { get; set; }

    public string RequiereCuracionesDesc { get; set; } = null!;

    public bool RequiereMonitoreo { get; set; }

    public string RequiereMonitoreoDesc { get; set; } = null!;

    public bool RequiereAtencionNeurologica { get; set; }

    public string RequiereAtencionNeurologicaDesc { get; set; } = null!;

    public bool CuidadosNocturnos { get; set; }

    public string CuidadosNocturnosDesc { get; set; } = null!;

    public bool RequiereCuidadosCriticos { get; set; }

    public string RequiereCuidadosCriticosDesc { get; set; } = null!;

    public decimal TotalHoras { get; set; }

    public decimal CostoEstimadoHora { get; set; }

    public decimal Descuento { get; set; }

    public decimal CostoEstimado { get; set; }

    public Guid TipoEnfermeraId { get; set; }

    public Guid? UsuarioId { get; set; }

    public Guid EstadoId { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual CatEstado Estado { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual CatTipoEnfermera TipoEnfermera { get; set; } = null!;
}
