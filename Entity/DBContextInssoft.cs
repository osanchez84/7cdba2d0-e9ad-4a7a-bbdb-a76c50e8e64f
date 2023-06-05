using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GuanajuatoAdminUsuarios.Entity;

public partial class DBContextInssoft : DbContext
{
    public DBContextInssoft()
    {
    }

    public DBContextInssoft(DbContextOptions<DBContextInssoft> options)
        : base(options)
    {
    }

    public virtual DbSet<Dependencias> Dependencias { get; set; }

    public virtual DbSet<MarcasVehiculo> MarcasVehiculos { get; set; }

    public virtual DbSet<Oficiales> Oficiales { get; set; }

    public virtual DbSet<SubmarcasVehiculo> SubmarcasVehiculos { get; set; }

    public virtual DbSet<Estatus> Estatus { get; set; }

    public virtual DbSet<Delegaciones> Delegaciones { get; set; }

    public virtual DbSet<Colores> Colores{ get; set; }

    public virtual DbSet<TipoVehiculos> TipoVehiculos { get; set; }

    public virtual DbSet<SalariosMinimos> SalariosMinimos { get; set; }

    public virtual DbSet<DiasInhabiles> DiasInhabiles { get; set; }

    public virtual DbSet<Municipios> Municipios { get; set; }

    public virtual DbSet<TiposCarga> TiposCarga { get; set; }

    public virtual DbSet<MotivosInfraccion> MotivosInfraccion { get; set; }





    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=54.39.102.101;Database=SITTEG_DEV;User Id=sitteg_admin_dev;Password=s5t3b_d3v;Trusted_Connection=False;TrustServerCertificate=True");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dependencias>(entity =>
        {
            entity.HasKey(e => e.IdDependencia).HasName("PK__dependen__A67AC7BE849A3403");

            entity.ToTable("dependencias");

            entity.Property(e => e.IdDependencia).HasColumnName("idDependencia");
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaActualizacion");
            entity.Property(e => e.NombreDependencia)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombreDependencia");
        });

        modelBuilder.Entity<MarcasVehiculo>(entity =>
        {
            entity.HasKey(e => e.IdMarcaVehiculo).HasName("PK__marcasVe__33AE0F9E9DCD9C8C");

            entity.ToTable("marcasVehiculos");

            entity.Property(e => e.IdMarcaVehiculo).HasColumnName("IdMarcaVehiculo");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaActualizacion");
            entity.Property(e => e.MarcaVehiculo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("marcaVehiculo");
            entity.Property(e => e.ModificadoPor).HasColumnName("modificadoPor");
        });

        modelBuilder.Entity<Oficiales>(entity =>
        {
            entity.HasKey(e => e.IdOficial).HasName("PK__oficiale__7BFD0DB1E280E5C4");

            entity.ToTable("oficiales");

            entity.Property(e => e.IdOficial).HasColumnName("idOficial");

            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoMaterno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellidoPaterno");
            entity.Property(e => e.Estatus).HasColumnName("estatus");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaActualizacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rango)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rango");
            entity.Property(e => e.IdDelegacion).HasColumnName("IdDelegacion");

        });

        modelBuilder.Entity<SubmarcasVehiculo>(entity =>
        {
            entity.HasKey(e => e.IdSubmarca);

            entity.ToTable("submarcasVehiculos");

            entity.Property(e => e.IdSubmarca).HasColumnName("IdSubmarca");
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.estatus).HasColumnName("estatus");
            entity.Property(e => e.FechaActualizacion)
              .HasColumnType("datetime")
              .HasColumnName("fechaActualizacion");
            entity.Property(e => e.IdMarcaVehiculo).HasColumnName("IdMarcaVehiculo");
            entity.Property(e => e.NombreSubmarca)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreSubmarca");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.estatus);

            entity.ToTable("estatus");

            entity.Property(e => e.estatus).HasColumnName("estatus");
            entity.Property(e => e.estatusDesc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("estatusDesc");
        });

        modelBuilder.Entity<Delegaciones>(entity =>
        { 
         entity.HasKey(e => e.IdDelegacion);

        entity.ToTable("delegaciones");
        entity.Property(e => e.IdDelegacion).HasColumnName("IdDelegacion");

        entity.Property(e => e.Delegacion).HasColumnName("Delegacion")
            .HasMaxLength(100)
            .IsUnicode(false)
            .HasColumnName("Delegacion");
        entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.FechaActualizacion)
             .HasColumnType("datetime")
             .HasColumnName("fechaActualizacion");
        entity.Property(e => e.Estatus).HasColumnName("Estatus");
       
    });

        modelBuilder.Entity<Colores>(entity =>
        {
            entity.HasKey(e => e.IdColor);

            entity.ToTable("colores");
            entity.Property(e => e.IdColor).HasColumnName("IdColor");

            entity.Property(e => e.color).HasColumnName("color")
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.FechaActualizacion)
             .HasColumnType("datetime")
             .HasColumnName("fechaActualizacion");
            entity.Property(e => e.Estatus).HasColumnName("Estatus");

        });
        modelBuilder.Entity<TipoVehiculos>(entity =>
        {
            entity.HasKey(e => e.IdTipoVehiculo);

            entity.ToTable("tiposVehiculo");



            entity.Property(e => e.IdTipoVehiculo).HasColumnName("IdTipoVehiculo");

            entity.Property(e => e.TipoVehiculo).HasColumnName("TipoVehiculo")
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.FechaActualizacion)
             .HasColumnType("datetime")
             .HasColumnName("fechaActualizacion");
            entity.Property(e => e.Estatus).HasColumnName("Estatus");

        });

        modelBuilder.Entity<SalariosMinimos>(entity =>
        {
            entity.HasKey(e => e.IdSalario);

            entity.ToTable("salariosMinimos");



            entity.Property(e => e.IdSalario).HasColumnName("IdSalario");

            entity.Property(e => e.Area).HasColumnName("Area")
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Salario)
                .HasColumnType("float")
                .HasColumnName("Salario");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("Fecha");
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaActualizacion");
            entity.Property(e => e.Estatus).HasColumnName("Estatus");

        });

        modelBuilder.Entity<DiasInhabiles>(entity =>
        {
            entity.HasKey(e => e.idDiaInhabil);

            entity.ToTable("diasInhabiles");



            entity.Property(e => e.idDiaInhabil).HasColumnName("idDiaInhabil");

            entity.Property(e => e.fecha).HasColumnName("fecha");
              
            entity.Property(e => e.idMunicipio).HasColumnName("idMunicipio");
            entity.Property(e => e.todosMunicipiosBool).HasColumnName("todosMunicipiosBool");
            entity.Property(e => e.todosMunicipiosDesc).HasColumnName("todosMunicipiosDesc")
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.FechaActualizacion)
                .HasColumnName("fechaActualizacion");
            entity.Property(e => e.Estatus).HasColumnName("Estatus");

        });

        modelBuilder.Entity<Municipios>(entity =>
        {
            entity.HasKey(e => e.idMunicipio).HasName("IdMunicipio");

            entity.ToTable("municipios");

            entity.Property(e => e.idMunicipio).HasColumnName("IdMunicipio");           
            entity.Property(e => e.Municipio)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("municipio");
        });
        modelBuilder.Entity<TiposCarga>(entity =>
        {
            entity.HasKey(e => e.IdTipoCarga);

            entity.ToTable("tiposCarga");
            entity.Property(e => e.IdTipoCarga).HasColumnName("IdTipoCarga");

            entity.Property(e => e.TipoCarga).HasColumnName("TipoCarga")
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.FechaActualizacion)
             .HasColumnType("datetime")
             .HasColumnName("fechaActualizacion");
            entity.Property(e => e.Estatus).HasColumnName("Estatus");

        });

        modelBuilder.Entity<MotivosInfraccion>(entity =>
        {
            entity.HasKey(e => e.IdMotivoInfraccion);

            entity.ToTable("motivosInfraccion");
            entity.Property(e => e.IdMotivoInfraccion).HasColumnName("IdMotivoInfraccion");

            entity.Property(e => e.Nombre).HasColumnName("Nombre")
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Fundamento).HasColumnName("Fundamento")
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CalificacionMinima).HasColumnName("CalificacionMinima");
            entity.Property(e => e.CalificacionMaxima).HasColumnName("CalificacionMaxima");
            entity.Property(e => e.ActualizadoPor).HasColumnName("actualizadoPor");
            entity.Property(e => e.FechaActualizacion)
             .HasColumnType("datetime")
             .HasColumnName("fechaActualizacion");
            entity.Property(e => e.Estatus).HasColumnName("Estatus");

        });

        OnModelCreatingPartial(modelBuilder);
    }



    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
