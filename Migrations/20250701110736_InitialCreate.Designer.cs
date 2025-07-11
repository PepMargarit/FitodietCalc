﻿// <auto-generated />
using System;
using FitodietCalc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FitodietCalc.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250701110736_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.6");

            modelBuilder.Entity("FitodietCalc.Models.Evaluacion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("AlturaCm")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<double>("GrasaCorporal")
                        .HasColumnType("REAL");

                    b.Property<double>("MasaMuscular")
                        .HasColumnType("REAL");

                    b.Property<int>("PacienteId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("PesoKg")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("PacienteId");

                    b.ToTable("Evaluaciones");
                });

            modelBuilder.Entity("FitodietCalc.Models.Paciente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Apellido1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Apellido2")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("FitodietCalc.Models.Evaluacion", b =>
                {
                    b.HasOne("FitodietCalc.Models.Paciente", "Paciente")
                        .WithMany("Evaluaciones")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("FitodietCalc.Models.Paciente", b =>
                {
                    b.Navigation("Evaluaciones");
                });
#pragma warning restore 612, 618
        }
    }
}
