using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatEstatusSolicitud
{
    public int Id { get; set; }

    public string Descripcion { get; set; } = null!;
}
