using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ShoeAccounting.Models;

public partial class ShoesDbContext : DbContext
{
    public ShoesDbContext()
    {
    }

    public ShoesDbContext(DbContextOptions<ShoesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PickUpPoint> PickUpPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=shoes_db", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.44-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryTitle)
                .HasMaxLength(45)
                .HasColumnName("category_title");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("PRIMARY");

            entity.ToTable("manufacturers");

            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.ManufacturerTitle)
                .HasMaxLength(100)
                .HasColumnName("manufacturer_title");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.PickUpPointId, "o_pick_up_id_fk_idx");

            entity.HasIndex(e => e.ProductArticle, "o_product_article_fk_idx");

            entity.HasIndex(e => e.UserId, "o_user_id_idx");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderDateMake)
                .HasDefaultValueSql("curdate()")
                .HasColumnName("order_date_make");
            entity.Property(e => e.OrderDateReceipt).HasColumnName("order_date_receipt");
            entity.Property(e => e.OrderReceiptCode)
                .HasMaxLength(10)
                .HasColumnName("order_receipt_code");
            entity.Property(e => e.OrderStatus)
                .HasDefaultValueSql("'Новый'")
                .HasColumnType("enum('Новый','Завершен')")
                .HasColumnName("order_status");
            entity.Property(e => e.PickUpPointId).HasColumnName("pick_up_point_id");
            entity.Property(e => e.ProductArticle)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("product_article");
            entity.Property(e => e.ProductQuantity).HasColumnName("product_quantity");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.PickUpPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickUpPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("o_pick_up_id_fk");

            entity.HasOne(d => d.ProductArticleNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ProductArticle)
                .HasConstraintName("o_product_article_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("o_user_id");
        });

        modelBuilder.Entity<PickUpPoint>(entity =>
        {
            entity.HasKey(e => e.PickUpPointId).HasName("PRIMARY");

            entity.ToTable("pick_up_points");

            entity.Property(e => e.PickUpPointId).HasColumnName("pick_up_point_id");
            entity.Property(e => e.PickUpPointCity)
                .HasMaxLength(100)
                .HasColumnName("pick_up_point_city");
            entity.Property(e => e.PickUpPointHome)
                .HasMaxLength(10)
                .HasColumnName("pick_up_point_home");
            entity.Property(e => e.PickUpPointPostalCode)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("pick_up_point_postal_code");
            entity.Property(e => e.PickUpPointStreet)
                .HasMaxLength(100)
                .HasColumnName("pick_up_point_street");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticle).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.CategoryId, "p_category_id_fk");

            entity.HasIndex(e => e.ProviderId, "p_category_id_fk_idx");

            entity.HasIndex(e => e.ManufacturerId, "p_manufacturer_id_fk_idx");

            entity.Property(e => e.ProductArticle)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasColumnName("product_article");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.ProductDescription)
                .HasColumnType("text")
                .HasColumnName("product_description");
            entity.Property(e => e.ProductDiscount)
                .HasDefaultValueSql("'0'")
                .HasColumnName("product_discount");
            entity.Property(e => e.ProductPhoto)
                .HasColumnType("mediumblob")
                .HasColumnName("product_photo");
            entity.Property(e => e.ProductPrice)
                .HasPrecision(12, 2)
                .HasColumnName("product_price");
            entity.Property(e => e.ProductQuantityInStock).HasColumnName("product_quantity_in_stock");
            entity.Property(e => e.ProductTitle)
                .HasMaxLength(100)
                .HasColumnName("product_title");
            entity.Property(e => e.ProductUnit)
                .HasMaxLength(10)
                .HasColumnName("product_unit");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("p_category_id_fk");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ManufacturerId)
                .HasConstraintName("p_manufacturer_id_fk");

            entity.HasOne(d => d.Provider).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("p_provierd_id_fk");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.ProviderId).HasName("PRIMARY");

            entity.ToTable("providers");

            entity.Property(e => e.ProviderId).HasColumnName("provider_id");
            entity.Property(e => e.ProviderTitle)
                .HasMaxLength(100)
                .HasColumnName("provider_title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserFirstname)
                .HasMaxLength(60)
                .HasColumnName("user_firstname");
            entity.Property(e => e.UserLastname)
                .HasMaxLength(60)
                .HasColumnName("user_lastname");
            entity.Property(e => e.UserLogin)
                .HasMaxLength(255)
                .HasColumnName("user_login");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(25)
                .HasColumnName("user_password");
            entity.Property(e => e.UserPatronymic)
                .HasMaxLength(60)
                .HasColumnName("user_patronymic");
            entity.Property(e => e.UserRole)
                .HasDefaultValueSql("'Авторизированный клиент'")
                .HasColumnType("enum('Авторизированный клиент','Менеджер','Администратор')")
                .HasColumnName("user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
