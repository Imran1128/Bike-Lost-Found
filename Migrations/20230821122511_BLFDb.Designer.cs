﻿// <auto-generated />
using BikeLostAndFound.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BikeLostAndFound.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20230821122511_BLFDb")]
    partial class BLFDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BikeLostAndFound.Models.LostAndFoundBikeInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BikeName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BikePhoto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BikeRegNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BikeSN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FoundOrLost")
                        .HasColumnType("int");

                    b.Property<string>("OwnerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlaceWhereLost")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LostAndFoundBikeInformation");
                });
#pragma warning restore 612, 618
        }
    }
}
