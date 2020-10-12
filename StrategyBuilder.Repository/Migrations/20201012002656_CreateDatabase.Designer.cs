﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StrategyBuilder.Repository;

namespace StrategyBuilder.Repository.Migrations
{
    [DbContext(typeof(StrategyBuilderContext))]
    [Migration("20201012002656_CreateDatabase")]
    partial class CreateDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EventGroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Occurrence")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EventGroupId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.EventGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("EventGroups");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Event", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.EventGroup", null)
                        .WithMany("Events")
                        .HasForeignKey("EventGroupId");
                });
#pragma warning restore 612, 618
        }
    }
}
