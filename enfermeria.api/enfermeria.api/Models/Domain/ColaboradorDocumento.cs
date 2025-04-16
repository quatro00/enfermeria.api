using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class ColaboradorDocumento
{
    public Guid Id { get; set; }

    public Guid ColaboradorId { get; set; }

    public int TipoDocumentoId { get; set; }

    public string Descripcion { get; set; } = null!;

    public string Ruta { get; set; } = null!;

    public string RutaFisica { get; set; } = null!;

    public string NombreArchivo { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual Colaborador Colaborador { get; set; } = null!;

    public virtual CatTipoDocumento TipoDocumento { get; set; } = null!;
}
