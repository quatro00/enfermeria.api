using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatHorario
{
    public int Id { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraTermino { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal PorcentajeTarifa { get; set; }
}
