using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Servicio
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public Guid PacienteId { get; set; }

    public DateTime Vigencia { get; set; }

    public int EstatusServicioId { get; set; }

    public Guid MunicipioId { get; set; }

    public string Direccion { get; set; } = null!;

    public string Lat { get; set; } = null!;

    public string Lon { get; set; } = null!;

    public string PrincipalRazon { get; set; } = null!;

    public bool RequiereAyudaBasica { get; set; }

    public string RequiereAyudaBasicaDesc { get; set; } = null!;

    public bool EnfermedadDiagnosticada { get; set; }

    public string EnfermedadDiagnosticadaDesc { get; set; } = null!;

    public bool TomaMedicamento { get; set; }

    public string TomaMedicamentoDesc { get; set; } = null!;

    public bool RequiereCuraciones { get; set; }

    public string RequiereCuracionesDesc { get; set; } = null!;

    public bool DispositivosMedicos { get; set; }

    public string DispositivosMedicosDesc { get; set; } = null!;

    public bool RequiereMonitoreo { get; set; }

    public string RequiereMonitoreoDesc { get; set; } = null!;

    public bool CuidadosNocturnos { get; set; }

    public string CuidadosNocturnosDesc { get; set; } = null!;

    public bool RequiereAtencionNeurologica { get; set; }

    public string RequiereAtencionNeurologicaDesc { get; set; } = null!;

    public bool RequiereCuidadosCriticos { get; set; }

    public string RequiereCuidadosCriticosDesc { get; set; } = null!;

    public decimal TotalHoras { get; set; }

    public decimal SubTotalPropuesto { get; set; }

    public decimal Impuestos { get; set; }

    public decimal Descuento { get; set; }

    public decimal CostoEstimadoHora { get; set; }

    public decimal Total { get; set; }

    public Guid TipoEnfermeraId { get; set; }

    public int TipoLugarId { get; set; }

    public string Observaciones { get; set; } = null!;

    public Guid? UsuarioId { get; set; }

    public string? ReferenciaPagoStripe { get; set; }

    public string? Transferencia { get; set; }

    public string? ReferenciaTransferencia { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual CatEstatusServicio EstatusServicio { get; set; } = null!;

    public virtual CatMunicipio Municipio { get; set; } = null!;

    public virtual Paciente Paciente { get; set; } = null!;

    public virtual ICollection<ServicioFecha> ServicioFechas { get; set; } = new List<ServicioFecha>();

    public virtual CatTipoEnfermera TipoEnfermera { get; set; } = null!;

    public virtual CatTipoLugar TipoLugar { get; set; } = null!;
}
