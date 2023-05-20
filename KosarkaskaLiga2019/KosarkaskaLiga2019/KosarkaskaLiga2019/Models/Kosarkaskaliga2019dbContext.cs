using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KosarkaskaLiga2019.Models
{
    public partial class Kosarkaskaliga2019dbContext : DbContext
    {
        public Kosarkaskaliga2019dbContext()
        {
        }

        public Kosarkaskaliga2019dbContext(DbContextOptions<Kosarkaskaliga2019dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Grad> Gradovi { get; set; }
        public virtual DbSet<Komentar> Komentari { get; set; }
        public virtual DbSet<Tim> Timovi { get; set; }
        public virtual DbSet<Utakmica> Utakmice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Tim>(entity =>
            {
                entity.Property(e => e.BrojBodova).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Grad)
                    .WithMany(p => p.Timovi)
                    .HasForeignKey(d => d.GradId)
                    .HasConstraintName("FK__Tim__GradId__2CF2ADDF");
            });

            modelBuilder.Entity<Utakmica>(entity =>
            {
                entity.Property(e => e.Rezultat).IsUnicode(false);

                entity.HasOne(d => d.Domacin)
                    .WithMany(p => p.UtakmicaDomacini)
                    .HasForeignKey(d => d.DomacinId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Utakmica__Domaci__2DE6D218");

                entity.HasOne(d => d.Gost)
                    .WithMany(p => p.UtakmicaGosti)
                    .HasForeignKey(d => d.GostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Utakmica__GostId__2EDAF651");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}