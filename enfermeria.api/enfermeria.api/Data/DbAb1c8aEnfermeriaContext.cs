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

    public virtual DbSet<CatBanco> CatBancos { get; set; }

    public virtual DbSet<CatEstado> CatEstados { get; set; }

    public virtual DbSet<CatEstatusPago> CatEstatusPagos { get; set; }

    public virtual DbSet<CatEstatusPagoLote> CatEstatusPagoLotes { get; set; }

    public virtual DbSet<CatEstatusServicio> CatEstatusServicios { get; set; }

    public virtual DbSet<CatEstatusServicioFecha> CatEstatusServicioFechas { get; set; }

    public virtual DbSet<CatHorario> CatHorarios { get; set; }

    public virtual DbSet<CatMunicipio> CatMunicipios { get; set; }

    public virtual DbSet<CatServicioFechasOfertaEstatus> CatServicioFechasOfertaEstatuses { get; set; }

    public virtual DbSet<CatTipoDocumento> CatTipoDocumentos { get; set; }

    public virtual DbSet<CatTipoEnfermera> CatTipoEnfermeras { get; set; }

    public virtual DbSet<CatTipoLugar> CatTipoLugars { get; set; }

    public virtual DbSet<Colaborador> Colaboradors { get; set; }

    public virtual DbSet<ColaboradorDocumento> ColaboradorDocumentos { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<EncuestaPlantilla> EncuestaPlantillas { get; set; }

    public virtual DbSet<EncuestaPlantillaPreguntum> EncuestaPlantillaPregunta { get; set; }

    public virtual DbSet<EstatusColaborador> EstatusColaboradors { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<PagoLote> PagoLotes { get; set; }

    public virtual DbSet<RelEstadoColaborador> RelEstadoColaboradors { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServicioCotizacion> ServicioCotizacions { get; set; }

    public virtual DbSet<ServicioFecha> ServicioFechas { get; set; }

    public virtual DbSet<ServicioFechasOfertum> ServicioFechasOferta { get; set; }

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

        modelBuilder.Entity<CatBanco>(entity =>
        {
            entity.ToTable("CatBanco");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<CatEstado>(entity =>
        {
            entity.ToTable("CatEstado");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.NombreCorto).HasMaxLength(50);
        });

        modelBuilder.Entity<CatEstatusPago>(entity =>
        {
            entity.ToTable("CatEstatusPago");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<CatEstatusPagoLote>(entity =>
        {
            entity.ToTable("CatEstatusPagoLote");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<CatEstatusServicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_CatEstatusSolicitud");

            entity.ToTable("CatEstatusServicio");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<CatEstatusServicioFecha>(entity =>
        {
            entity.ToTable("CatEstatusServicioFecha");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<CatHorario>(entity =>
        {
            entity.ToTable("CatHorario");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
            entity.Property(e => e.PorcentajeTarifa).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<CatMunicipio>(entity =>
        {
            entity.ToTable("CatMunicipio");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.NombreCorto).HasMaxLength(50);

            entity.HasOne(d => d.Estado).WithMany(p => p.CatMunicipios)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CatMunicipio_CatEstado");
        });

        modelBuilder.Entity<CatServicioFechasOfertaEstatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ServicioOfertaEstatus");

            entity.ToTable("CatServicioFechasOfertaEstatus");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<CatTipoDocumento>(entity =>
        {
            entity.ToTable("CatTipoDocumento");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TipoDocumento).HasMaxLength(50);
        });

        modelBuilder.Entity<CatTipoEnfermera>(entity =>
        {
            entity.HasKey(e => e.TipoEnfermeraId);

            entity.ToTable("CatTipoEnfermera");

            entity.Property(e => e.TipoEnfermeraId).ValueGeneratedNever();
            entity.Property(e => e.CostoHora).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<CatTipoLugar>(entity =>
        {
            entity.ToTable("CatTipoLugar");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50);
        });

        modelBuilder.Entity<Colaborador>(entity =>
        {
            entity.ToTable("Colaborador");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Apellidos).HasMaxLength(500);
            entity.Property(e => e.Avatar).HasMaxLength(500);
            entity.Property(e => e.CedulaProfesional).HasMaxLength(500);
            entity.Property(e => e.Clabe).HasMaxLength(50);
            entity.Property(e => e.Colonia).HasMaxLength(500);
            entity.Property(e => e.Comision).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CorreoElectronico).HasMaxLength(500);
            entity.Property(e => e.Cp).HasMaxLength(50);
            entity.Property(e => e.Cuenta).HasMaxLength(50);
            entity.Property(e => e.Curp).HasMaxLength(500);
            entity.Property(e => e.DomicilioCalle).HasMaxLength(500);
            entity.Property(e => e.DomicilioNumero).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.No).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.Rfc).HasMaxLength(500);
            entity.Property(e => e.Telefono).HasMaxLength(500);

            entity.HasOne(d => d.Banco).WithMany(p => p.Colaboradors)
                .HasForeignKey(d => d.BancoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colaborador_CatBanco");

            entity.HasOne(d => d.EstatusColaborador).WithMany(p => p.Colaboradors)
                .HasForeignKey(d => d.EstatusColaboradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colaborador_EstatusColaborador");

            entity.HasOne(d => d.TipoEnfermera).WithMany(p => p.Colaboradors)
                .HasForeignKey(d => d.TipoEnfermeraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Colaborador_CatTipoEnfermera");
        });

        modelBuilder.Entity<ColaboradorDocumento>(entity =>
        {
            entity.ToTable("ColaboradorDocumento");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Descripcion).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.NombreArchivo).HasMaxLength(50);
            entity.Property(e => e.Ruta).HasMaxLength(500);
            entity.Property(e => e.RutaFisica).HasMaxLength(500);

            entity.HasOne(d => d.Colaborador).WithMany(p => p.ColaboradorDocumentos)
                .HasForeignKey(d => d.ColaboradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ColaboradorDocumento_Colaborador");

            entity.HasOne(d => d.TipoDocumento).WithMany(p => p.ColaboradorDocumentos)
                .HasForeignKey(d => d.TipoDocumentoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ColaboradorDocumento_CatTipoDocumento");
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

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.ToTable("Contacto");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CorreoElectronico).HasMaxLength(50);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.Parentezco).HasMaxLength(150);
            entity.Property(e => e.Telefono).HasMaxLength(50);

            entity.HasOne(d => d.Paciente).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contacto_Paciente");
        });

        modelBuilder.Entity<EncuestaPlantilla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Encuesta__C5DEB5EC9B4253A5");

            entity.ToTable("EncuestaPlantilla");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.No).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(200);
            entity.Property(e => e.TipoServicio).HasMaxLength(100);
        });

        modelBuilder.Entity<EncuestaPlantillaPreguntum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Encuesta__EBB2A379383D9F73");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Requerida).HasDefaultValue(true);
            entity.Property(e => e.Texto).HasMaxLength(500);
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.Plantilla).WithMany(p => p.EncuestaPlantillaPregunta)
                .HasForeignKey(d => d.PlantillaId)
                .HasConstraintName("FK__EncuestaP__Plant__5B78929E");
        });

        modelBuilder.Entity<EstatusColaborador>(entity =>
        {
            entity.ToTable("EstatusColaborador");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(150);
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("Paciente");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Apellidos).HasMaxLength(500);
            entity.Property(e => e.CorreoElectronico).HasMaxLength(500);
            entity.Property(e => e.DescripcionDiscapacidad).HasMaxLength(500);
            entity.Property(e => e.Estatura).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Genero).HasMaxLength(50);
            entity.Property(e => e.No).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).HasMaxLength(500);
            entity.Property(e => e.Peso).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Telefono).HasMaxLength(500);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.ToTable("Pago");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Comision).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Comprobante).HasMaxLength(500);
            entity.Property(e => e.CostoOperativo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.ImporteBruto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.No).ValueGeneratedOnAdd();
            entity.Property(e => e.Referencia).HasMaxLength(50);
            entity.Property(e => e.Retencion).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.EstatusPago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.EstatusPagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_CatEstatusPago");

            entity.HasOne(d => d.PagoLote).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PagoLoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_PagoLote");

            entity.HasOne(d => d.ServicioFecha).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.ServicioFechaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_ServicioFechas");
        });

        modelBuilder.Entity<PagoLote>(entity =>
        {
            entity.ToTable("PagoLote");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Csv).HasMaxLength(500);
            entity.Property(e => e.Etiqueta).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.No).ValueGeneratedOnAdd();

            entity.HasOne(d => d.EstatosPagoLote).WithMany(p => p.PagoLotes)
                .HasForeignKey(d => d.EstatosPagoLoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PagoLote_CatEstatusPagoLote");
        });

        modelBuilder.Entity<RelEstadoColaborador>(entity =>
        {
            entity.ToTable("RelEstadoColaborador");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");

            entity.HasOne(d => d.Colaborador).WithMany(p => p.RelEstadoColaboradors)
                .HasForeignKey(d => d.ColaboradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelEstadoColaborador_Colaborador");

            entity.HasOne(d => d.Estado).WithMany(p => p.RelEstadoColaboradors)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RelEstadoColaborador_CatEstado");
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Solicitud");

            entity.ToTable("Servicio");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CostoEstimadoHora).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CuidadosNocturnosDesc).HasMaxLength(500);
            entity.Property(e => e.Descuento).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Direccion).HasMaxLength(500);
            entity.Property(e => e.DispositivosMedicosDesc).HasMaxLength(500);
            entity.Property(e => e.EnfermedadDiagnosticadaDesc).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Lat).HasMaxLength(50);
            entity.Property(e => e.Lon).HasMaxLength(50);
            entity.Property(e => e.No).ValueGeneratedOnAdd();
            entity.Property(e => e.Observaciones).HasMaxLength(500);
            entity.Property(e => e.PrincipalRazon).HasMaxLength(500);
            entity.Property(e => e.ReferenciaPagoStripe).HasMaxLength(500);
            entity.Property(e => e.RequiereAtencionNeurologicaDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereAyudaBasicaDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereCuidadosCriticosDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereCuracionesDesc).HasMaxLength(500);
            entity.Property(e => e.RequiereMonitoreoDesc).HasMaxLength(500);
            entity.Property(e => e.SubTotalPropuesto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TomaMedicamentoDesc).HasMaxLength(500);
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalHoras).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vigencia).HasColumnType("datetime");

            entity.HasOne(d => d.EstatusServicio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.EstatusServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_CatEstatusServicio");

            entity.HasOne(d => d.Municipio).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.MunicipioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_CatMunicipio");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_Paciente");

            entity.HasOne(d => d.TipoEnfermera).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoEnfermeraId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Solicitud_CatTipoEnfermera");

            entity.HasOne(d => d.TipoLugar).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.TipoLugarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_CatTipoLugar");
        });

        modelBuilder.Entity<ServicioCotizacion>(entity =>
        {
            entity.ToTable("ServicioCotizacion");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.Horario).HasMaxLength(50);
            entity.Property(e => e.Horas).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PrecioFinal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PrecioHoraBase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PrecioHoraFinal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ServicioFechas).WithMany(p => p.ServicioCotizacions)
                .HasForeignKey(d => d.ServicioFechasId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioCotizacion_ServicioFechas");
        });

        modelBuilder.Entity<ServicioFecha>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.CantidadHoras).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Comision).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CostosOperativos).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.FechaTermino).HasColumnType("datetime");
            entity.Property(e => e.ImporteBruto).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ImporteSolicitado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Retenciones).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.ColaboradorAsignado).WithMany(p => p.ServicioFechas)
                .HasForeignKey(d => d.ColaboradorAsignadoId)
                .HasConstraintName("FK_ServicioFechas_Colaborador");

            entity.HasOne(d => d.EstatusServicioFecha).WithMany(p => p.ServicioFechas)
                .HasForeignKey(d => d.EstatusServicioFechaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioFechas_CatEstatusServicioFecha");

            entity.HasOne(d => d.Servicio).WithMany(p => p.ServicioFechas)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioFechas_Servicio");
        });

        modelBuilder.Entity<ServicioFechasOfertum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ServicioOferta");

            entity.Property(e => e.Id).HasDefaultValueSql("(newsequentialid())");
            entity.Property(e => e.Comentario).HasMaxLength(500);
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime");
            entity.Property(e => e.FechaModificacion).HasColumnType("datetime");
            entity.Property(e => e.MontoSolicitado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Observaciones).HasMaxLength(500);

            entity.HasOne(d => d.Colaborador).WithMany(p => p.ServicioFechasOferta)
                .HasForeignKey(d => d.ColaboradorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioOferta_Colaborador");

            entity.HasOne(d => d.EstatusOferta).WithMany(p => p.ServicioFechasOferta)
                .HasForeignKey(d => d.EstatusOfertaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioOferta_ServicioOfertaEstatus");

            entity.HasOne(d => d.ServicioFecha).WithMany(p => p.ServicioFechasOferta)
                .HasForeignKey(d => d.ServicioFechaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServicioFechasOferta_ServicioFechas");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
