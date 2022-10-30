using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TransportApi.Models
{
    public partial class TransportContext : DbContext
    {
        public TransportContext()
        {
        }

        public TransportContext(DbContextOptions<TransportContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminInfo> AdminInfos { get; set; } = null!;
        public virtual DbSet<EmployeeInfo> EmployeeInfos { get; set; } = null!;
        public virtual DbSet<RouteInfo> RouteInfos { get; set; } = null!;
        public virtual DbSet<StopInfo> StopInfos { get; set; } = null!;
        public virtual DbSet<VehicleInfo> VehicleInfos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-LHUHC4U\\SQLEXPRESS;Database=Transport;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("AdminInfo");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EmployeeInfo>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__Employee__7AD04F1155FE1BD6");

                entity.ToTable("EmployeeInfo");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleNum)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Stop)
                    .WithMany(p => p.EmployeeInfos)
                    .HasForeignKey(d => d.StopId)
                    .HasConstraintName("FK__EmployeeI__StopI__403A8C7D");

                entity.HasOne(d => d.VehicleNumNavigation)
                    .WithMany(p => p.EmployeeInfos)
                    .HasForeignKey(d => d.VehicleNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeI__Vehic__3F466844");
            });

            modelBuilder.Entity<RouteInfo>(entity =>
            {
                entity.HasKey(e => e.RouteNum)
                    .HasName("PK__RouteInf__71AD28E18A3EA2FE");

                entity.ToTable("RouteInfo");

                entity.Property(e => e.RouteName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.VehicleNum)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.VehicleNumNavigation)
                    .WithMany(p => p.RouteInfos)
                    .HasForeignKey(d => d.VehicleNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__RouteInfo__Vehic__398D8EEE");
            });

            modelBuilder.Entity<StopInfo>(entity =>
            {
                entity.HasKey(e => e.StopId)
                    .HasName("PK__StopInfo__EB6A38F4B637A2BE");

                entity.ToTable("StopInfo");

                entity.Property(e => e.StopName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.RouteNumNavigation)
                    .WithMany(p => p.StopInfos)
                    .HasForeignKey(d => d.RouteNum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StopInfo__RouteN__3C69FB99");
            });

            modelBuilder.Entity<VehicleInfo>(entity =>
            {
                entity.HasKey(e => e.VehicleNum)
                    .HasName("PK__VehicleI__1EC1D099584631D5");

                entity.ToTable("VehicleInfo");

                entity.Property(e => e.VehicleNum)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
