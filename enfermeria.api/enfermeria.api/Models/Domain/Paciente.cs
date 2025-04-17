using System;
using System.Collections.Generic;

namespace enfermeria.api.Models.Domain;

public partial class Paciente
{
    public Guid Id { get; set; }

    public int No { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string Genero { get; set; } = null!;

    public decimal Peso { get; set; }

    public decimal Estatura { get; set; }

    public bool Discapacidad { get; set; }

    public string DescripcionDiscapacidad { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public Guid UsuarioCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public Guid? UsuarioModificacion { get; set; }

    public virtual ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
