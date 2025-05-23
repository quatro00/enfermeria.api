﻿using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class CatEstado
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NombreCorto { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual ICollection<CatMunicipio> CatMunicipios { get; set; } = new List<CatMunicipio>();

    public virtual ICollection<RelEstadoColaborador> RelEstadoColaboradors { get; set; } = new List<RelEstadoColaborador>();
}
