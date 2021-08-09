using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SS13Stats.Frontend.Database
{
    public partial class SS13StatsDBContext : DbContext
    {
        public SS13StatsDBContext()
        {
        }

        public SS13StatsDBContext(DbContextOptions<SS13StatsDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Heartbeat> Heartbeats { get; set; }
        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<Snapshot> Snapshots { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<Heartbeat>(entity =>
            {
                entity.HasKey(e => e.Type)
                    .HasName("PRIMARY");

                entity.ToTable("heartbeats");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .HasColumnName("type");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.ToTable("servers");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.FirstSeen)
                    .HasColumnType("datetime")
                    .HasColumnName("first_seen");

                entity.Property(e => e.LastSeen)
                    .HasColumnType("datetime")
                    .HasColumnName("last_seen");

                entity.Property(e => e.ServerName)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("server_name")
                    .HasDefaultValueSql("''");
            });

            modelBuilder.Entity<Snapshot>(entity =>
            {
                entity.ToTable("snapshots");

                entity.HasIndex(e => e.ServerId, "server_id_fk");

                entity.Property(e => e.SnapshotId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("snapshot_id");

                entity.Property(e => e.PlayerCount)
                    .HasColumnType("int(11)")
                    .HasColumnName("player_count");

                entity.Property(e => e.ServerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("server_id");

                entity.Property(e => e.SnapshotTime)
                    .HasColumnType("datetime")
                    .HasColumnName("snapshot_time");

                entity.HasOne(d => d.Server)
                    .WithMany(p => p.Snapshots)
                    .HasForeignKey(d => d.ServerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("server_id_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
