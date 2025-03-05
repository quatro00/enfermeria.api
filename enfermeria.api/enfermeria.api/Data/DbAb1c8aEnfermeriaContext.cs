using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using enfermeria.api.Models.Domain;

namespace enfermeria.api.Data;

public partial class DbAb1c8aEnfermeriaContext : DbContext
{
    public DbAb1c8aEnfermeriaContext()
    {
    }

    public DbAb1c8aEnfermeriaContext(DbContextOptions<DbAb1c8aEnfermeriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetSystem> AspNetSystems { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<CatEstado> CatEstados { get; set; }

    public virtual DbSet<CatEstatusSolicitud> CatEstatusSolicituds { get; set; }

    public virtual DbSet<CatTipoEnfermera> CatTipoEnfermeras { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<Cuentum> Cuenta { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Solicitud> Solicituds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=SQL1002.site4now.net;Initial Catalog=db_ab1c8a_enfermeria;Persist Security Info=True;User ID=db_ab1c8a_enfermeria_admin;Password=Suikoden2.;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetSystem>(entity =>
        {
            entity.ToTable("AspNetSystem");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Clave).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(500);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<CatEstado>(entity =>
        {
            entity.ToTable("CatEstado");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.NombreCorto).HasMaxLength(50);
        });

        modelBuilder.Entity<CatEstatusSolicitud>(entity =>
        {
            entity.ToTable("CatEstatusSolicitud");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<CatTipoEnfermera>(entity =>
        {
            entity.HasKey(e => e.TipoEnfermeraId);

            entity.ToTable("CatTipoEnfermera");

            entity.Property(e => e.TipoEnfermeraId).ValueGeneratedNever();
            entity.Property(e => e.CostoHora).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.ToTable("Configuracion");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Codigo).HasMaxLength(500);
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Modulo).HasMaxLength(50);
            entity.Property(e => e.ValorDate).HasColumnType("datetime");
            entity.Property(e => e.ValorDecimal).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.ValorString).HasMaxLength(500);
        });

        modelBuilder.Entity<Cuentum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Apellidos).HasMaxLength(500);
            entity.Property(e => e.CorreoElectronico).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
            entity.Property(e => e.Telefono).HasMaxLength(50);

            entity.HasOne(d => d.TipoEnfermera).WithMany(p => p.Cuenta)
                .HasForeignKey(d => d.TipoEnfermeraId)
                .HasConstraintName("FK_Cuenta_CatTipoEnfermera");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("Paciente");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Apellidos).HasMaxLength(500);
            entity.Property(e => e.DescripcionDiscapacidad).HasMaxLength(500);
            entity.Property(e => e.Estatura).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Genero).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.Peso).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.ToTable("Solicitud");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CostoEstimado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostoEstimadoHora).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CuidadosNocturnosDesc).HasMaxLength(500);
            entity.Property(e => e.Descuento).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DispositivosMedicosDesc).HasMaxLength(500);
            entity.Property(e => e.EnfermedadDiagnosticadaDesc).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.PrincipalRazon).HasMaxLength(500);
            entity.Property(e => e.RequiereAtencionNeurologicaDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereAyudaBasica).HasMaxLength(500);
            entity.Property(e => e.RequiereAyudaBasicaDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereCuidadosCriticosDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereCuracionesDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereMonitoreoDesc).HasMaxLength(500);
            entity.Property(e => e.TomaMedicamentoDesc).HasMaxLength(500);
            entity.Property(e => e.TotalHoras).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vigencia).HasColumnType("datetime");

            entity.HasOne(d => d.Estado).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_CatEstado");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Paciente");

            entity.HasOne(d => d.TipoEnfermera).WithMany(p => p.Solicituds)
                .HasForeignKey(d => d.TipoEnfermeraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_CatTipoEnfermera");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
