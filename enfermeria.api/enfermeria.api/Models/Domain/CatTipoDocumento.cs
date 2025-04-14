using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatTipoDocumento
{
    public int Id { get; set; }

    public string TipoDocumento { get; set; } = null!;

    public virtual ICollection<ColaboradorDocumento> ColaboradorDocumentos { get; set; } = new List<ColaboradorDocumento>();
}
