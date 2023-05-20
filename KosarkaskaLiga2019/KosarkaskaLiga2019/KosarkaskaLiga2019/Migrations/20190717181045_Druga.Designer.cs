﻿// <auto-generated />
using System;
using KosarkaskaLiga2019.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KosarkaskaLiga2019.Migrations
{
    [DbContext(typeof(Kosarkaskaliga2019dbContext))]
    [Migration("20190717181045_Druga")]
    partial class Druga
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("KosarkaskaLiga2019.Models.Grad", b =>
                {
                    b.Property<int>("GradId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NazivGrada")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("GradId");

                    b.ToTable("Grad");
                });

            modelBuilder.Entity("KosarkaskaLiga2019.Models.Tim", b =>
                {
                    b.Property<int>("TimId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BrojBodova")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("((0))");

                    b.Property<int?>("GradId");

                    b.Property<string>("Naziv")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<byte[]>("Slika");

                    b.Property<string>("TipSlike")
                        .HasColumnName("TipSLike")
                        .HasMaxLength(30);

                    b.HasKey("TimId");

                    b.HasIndex("GradId");

                    b.ToTable("Tim");
                });

            modelBuilder.Entity("KosarkaskaLiga2019.Models.Utakmica", b =>
                {
                    b.Property<int>("UtakmicaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DomacinId");

                    b.Property<int>("GostId");

                    b.Property<int>("Kolo");

                    b.Property<string>("Rezultat")
                        .HasMaxLength(7)
                        .IsUnicode(false);

                    b.HasKey("UtakmicaId");

                    b.HasIndex("DomacinId");

                    b.HasIndex("GostId");

                    b.ToTable("Utakmica");
                });

            modelBuilder.Entity("KosarkaskaLiga2019.Models.Tim", b =>
                {
                    b.HasOne("KosarkaskaLiga2019.Models.Grad", "Grad")
                        .WithMany("Timovi")
                        .HasForeignKey("GradId")
                        .HasConstraintName("FK__Tim__GradId__2CF2ADDF");
                });

            modelBuilder.Entity("KosarkaskaLiga2019.Models.Utakmica", b =>
                {
                    b.HasOne("KosarkaskaLiga2019.Models.Tim", "Domacin")
                        .WithMany("UtakmicaDomacini")
                        .HasForeignKey("DomacinId")
                        .HasConstraintName("FK__Utakmica__Domaci__2DE6D218");

                    b.HasOne("KosarkaskaLiga2019.Models.Tim", "Gost")
                        .WithMany("UtakmicaGosti")
                        .HasForeignKey("GostId")
                        .HasConstraintName("FK__Utakmica__GostId__2EDAF651");
                });
#pragma warning restore 612, 618
        }
    }
}