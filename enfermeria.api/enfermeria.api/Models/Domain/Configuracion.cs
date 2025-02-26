using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Configuracion
{
    public int Id { get; set; }

    public string? Descripcion { get; set; }

    public string? Modulo { get; set; }

    public string? Codigo { get; set; }

    public int? ValorEntero { get; set; }

    public decimal? ValorDecimal { get; set; }

    public string? ValorString { get; set; }

    public DateTime? ValorDate { get; set; }
}
