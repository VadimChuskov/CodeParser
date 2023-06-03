﻿// <auto-generated />
using System;
using CodeParser.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CodeParser.Database.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230417205333_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("CodeParser.Database.Models.Enums.ClassType", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ClassType");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "None"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Service"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Model"
                        });
                });

            modelBuilder.Entity("CodeParser.Database.Models.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NamespaceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("NamespaceId");

                    b.ToTable("File");
                });

            modelBuilder.Entity("CodeParser.Database.Models.Namespace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Namespace");
                });

            modelBuilder.Entity("CodeParser.Database.Models.File", b =>
                {
                    b.HasOne("CodeParser.Database.Models.Namespace", "Namespace")
                        .WithMany()
                        .HasForeignKey("NamespaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Namespace");
                });
#pragma warning restore 612, 618
        }
    }
}
