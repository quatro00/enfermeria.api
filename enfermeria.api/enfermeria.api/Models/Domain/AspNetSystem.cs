using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class AspNetSystem
{
    public int Id { get; set; }

    public string Clave { get; set; } = null!;

    public string Nombre { get; set; } = null!;
}
