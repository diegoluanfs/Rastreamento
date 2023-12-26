﻿// <auto-generated />
using System;
using LocatedAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LocatedAPI.Migrations
{
    [DbContext(typeof(Contexto))]
    [Migration("20231226135955_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LocatedAPI.Models.Moviment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdPerson")
                        .HasColumnType("int");

                    b.Property<int>("IdTarget")
                        .HasColumnType("int");

                    b.Property<double>("LatitudeEnd")
                        .HasColumnType("float");

                    b.Property<double>("LatitudeStart")
                        .HasColumnType("float");

                    b.Property<double>("LongitudeEnd")
                        .HasColumnType("float");

                    b.Property<double>("LongitudeStart")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Moviment");
                });

            modelBuilder.Entity("LocatedAPI.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("LocatedAPI.Models.Target", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdPerson")
                        .HasColumnType("int");

                    b.Property<double>("LatitudeEnd")
                        .HasColumnType("float");

                    b.Property<double>("LatitudeStart")
                        .HasColumnType("float");

                    b.Property<double>("LongitudeEnd")
                        .HasColumnType("float");

                    b.Property<double>("LongitudeStart")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Targets");
                });
#pragma warning restore 612, 618
        }
    }
}
