using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EcommercePerifericos.Models.DB;

public partial class AleshopBdContext : DbContext
{
    public AleshopBdContext()
    {
    }

    public AleshopBdContext(DbContextOptions<AleshopBdContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CarritoPc> CarritoPcs { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<DetalleVentum> DetalleVenta { get; set; }

    public virtual DbSet<Origen> Origens { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Ventum> Venta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CarritoPc>(entity =>
        {
            entity.ToTable("carrito_pc");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.Preciounitario).HasColumnName("preciounitario");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.CarritoPcs)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK_carrito_pc_producto");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.ToTable("categoria");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<DetalleVentum>(entity =>
        {
            entity.ToTable("detalle_venta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdProducto).HasColumnName("id_producto");
            entity.Property(e => e.IdVenta).HasColumnName("id_venta");
            entity.Property(e => e.Precio).HasColumnName("precio");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK_detalle_venta_producto");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK_detalle_venta_venta");
        });

        modelBuilder.Entity<Origen>(entity =>
        {
            entity.ToTable("origen");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("producto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Activo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("activo");
            entity.Property(e => e.Detalle)
                .IsUnicode(false)
                .HasColumnName("detalle");
            entity.Property(e => e.IdCategoria).HasColumnName("id_categoria");
            entity.Property(e => e.Img)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.Nombre)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.PrecioOferta).HasColumnName("precio_oferta");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategoria)
                .HasConstraintName("FK_producto_categoria");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("rol");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_usuario_rol");
        });

        modelBuilder.Entity<Ventum>(entity =>
        {
            entity.ToTable("venta");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CodigoPostal)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigo_postal");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Fecha)
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdOrigen).HasColumnName("id_origen");
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_cliente");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.IdOrigenNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdOrigen)
                .HasConstraintName("FK_venta_origen");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
