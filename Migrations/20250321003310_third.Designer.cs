﻿// <auto-generated />
using System;
using IoTSolution.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IoTSolution.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20250321003310_third")]
    partial class third
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IoTSolution.Models.DispositivosModel", b =>
                {
                    b.Property<int>("IdDispositivo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDispositivo"));

                    b.Property<DateTime>("DataHoraCadastro")
                        .HasColumnType("datetime2");

                    b.HasKey("IdDispositivo");

                    b.ToTable("Dispositivos");
                });

            modelBuilder.Entity("IoTSolution.Models.LeiturasModel", b =>
                {
                    b.Property<int>("IdLeitura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdLeitura"));

                    b.Property<DateTime>("DataHoraLeitura")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdDispositivo")
                        .HasColumnType("int");

                    b.Property<int>("IdSensor")
                        .HasColumnType("int");

                    b.Property<string>("Temperatura")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdLeitura");

                    b.ToTable("Leituras");
                });

            modelBuilder.Entity("IoTSolution.Models.SensorsModel", b =>
                {
                    b.Property<int>("IdSensor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSensor"));

                    b.Property<DateTime>("DataHoraCadastro")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DispositivosIdDispositivo")
                        .HasColumnType("int");

                    b.HasKey("IdSensor");

                    b.HasIndex("DispositivosIdDispositivo");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("IoTSolution.Models.SensorsModel", b =>
                {
                    b.HasOne("IoTSolution.Models.DispositivosModel", "Dispositivos")
                        .WithMany()
                        .HasForeignKey("DispositivosIdDispositivo");

                    b.Navigation("Dispositivos");
                });
#pragma warning restore 612, 618
        }
    }
}
