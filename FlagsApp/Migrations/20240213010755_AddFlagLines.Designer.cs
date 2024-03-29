﻿// <auto-generated />
using FlagsApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlagsApp.Migrations
{
    [DbContext(typeof(FlagsDBContext))]
    [Migration("20240213010755_AddFlagLines")]
    partial class AddFlagLines
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FlagsApp.Models.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("FlagsApp.Models.Flag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageSrc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Flags");
                });

            modelBuilder.Entity("FlagsApp.Models.FlagColor", b =>
                {
                    b.Property<int>("FlagId")
                        .HasColumnType("int");

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.HasKey("FlagId", "ColorId");

                    b.HasIndex("ColorId");

                    b.ToTable("FlagColors");
                });

            modelBuilder.Entity("FlagsApp.Models.FlagLines", b =>
                {
                    b.Property<int>("FlagId")
                        .HasColumnType("int");

                    b.Property<int>("LinesId")
                        .HasColumnType("int");

                    b.HasKey("FlagId", "LinesId");

                    b.HasIndex("LinesId");

                    b.ToTable("FlagLines");
                });

            modelBuilder.Entity("FlagsApp.Models.Lines", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("FlagsApp.Models.FlagColor", b =>
                {
                    b.HasOne("FlagsApp.Models.Color", null)
                        .WithMany()
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlagsApp.Models.Flag", null)
                        .WithMany()
                        .HasForeignKey("FlagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FlagsApp.Models.FlagLines", b =>
                {
                    b.HasOne("FlagsApp.Models.Flag", null)
                        .WithMany()
                        .HasForeignKey("FlagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlagsApp.Models.Lines", null)
                        .WithMany()
                        .HasForeignKey("LinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
