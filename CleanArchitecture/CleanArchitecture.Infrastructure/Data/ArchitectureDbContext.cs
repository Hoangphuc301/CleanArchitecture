using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Domain.Entities;

public partial class ArchitectureDbContext : DbContext
{
    public ArchitectureDbContext()
    {
    }

    public ArchitectureDbContext(DbContextOptions<ArchitectureDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Menus> Menus { get; set; }

    public virtual DbSet<MenuNews> MenuNews { get; set; }

    public virtual DbSet<News> News { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-6OJC3FAO;Database=Architecture;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menus>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Menu__C99ED250ACB4B43F");

            entity.ToTable("Menu");

            entity.HasIndex(e => e.Slug, "UQ__Menu__BC7B5FB6AE4B5342").IsUnique();

            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MenuName).HasMaxLength(100);
            entity.Property(e => e.Slug)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MenuNews>(entity =>
        {
            entity.HasKey(e => new { e.MenuId, e.NewsId }).HasName("PK__MenuNews__E0CA398D4AD64580");

            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Note).HasMaxLength(255);

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuNews)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuNews__MenuID__45F365D3");

            entity.HasOne(d => d.News).WithMany(p => p.MenuNews)
                .HasForeignKey(d => d.NewsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MenuNews__NewsID__46E78A0C");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDD3CD6DA9E8");

            entity.HasIndex(e => e.Slug, "UQ__News__BC7B5FB6287851DD").IsUnique();

            entity.Property(e => e.NewsId).HasColumnName("NewsID");
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsHot).HasDefaultValue(false);
            entity.Property(e => e.PublishDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Slug)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue(true);
            entity.Property(e => e.Summary).HasMaxLength(500);
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.ViewCount).HasDefaultValue(0);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
