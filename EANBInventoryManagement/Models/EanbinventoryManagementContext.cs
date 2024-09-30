using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EANBInventoryManagement.Models;

public partial class EanbinventoryManagementContext : DbContext
{
    public EanbinventoryManagementContext()
    {
    }

    public EanbinventoryManagementContext(DbContextOptions<EanbinventoryManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Offer> Offers { get; set; }

    public virtual DbSet<RequestedItem> RequestedItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EANBInventoryManagement;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__2370F72785FD5E6C");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.End)
                .HasColumnType("datetime")
                .HasColumnName("end");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Start)
                .HasColumnType("datetime")
                .HasColumnName("start");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Location).WithMany(p => p.Events)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__location__3D5E1FD2");

            entity.HasOne(d => d.User).WithMany(p => p.Events)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__user_id__3E52440B");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("PK__Location__771831EA9783E623");

            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Offer>(entity =>
        {
            entity.Property(e => e.OfferId).HasColumnName("offerId");
            entity.Property(e => e.Amount)
                .HasMaxLength(50)
                .HasColumnName("amount");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.OfferUserId).HasColumnName("offerUserId");
            entity.Property(e => e.RequestUserId).HasColumnName("RequestUserID");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.State)
                .HasMaxLength(50)
                .HasColumnName("state");

            entity.HasOne(d => d.OfferUser).WithMany(p => p.OfferOfferUsers)
                .HasForeignKey(d => d.OfferUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Offers_Users1");

            entity.HasOne(d => d.RequestUser).WithMany(p => p.OfferRequestUsers)
                .HasForeignKey(d => d.RequestUserId)
                .HasConstraintName("FK_Offers_Users");
        });

        modelBuilder.Entity<RequestedItem>(entity =>
        {
            entity.HasKey(e => e.RequestedItemId).HasName("PK__Requeste__173838553EF9B9FC");

            entity.Property(e => e.RequestedItemId).HasColumnName("requested_item_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.IsFulfilled).HasColumnName("isFulfilled");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("startDate");

            entity.HasOne(d => d.Event).WithMany(p => p.RequestedItems)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Requested__event__3F466844");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370F84199184");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
