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
    [Migration("20201101225917_AddStrategyTable")]
    partial class AddStrategyTable
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

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("StrategyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("StrategyId");

                    b.ToTable("EventGroups");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Strategy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Strategies");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EncryptedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("SecretKey")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Event", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.EventGroup", "EventGroup")
                        .WithMany("Events")
                        .HasForeignKey("EventGroupId");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.EventGroup", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.User", "CreatedBy")
                        .WithMany("EventGroups")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StrategyBuilder.Repository.Entities.Strategy", null)
                        .WithMany("EventGroups")
                        .HasForeignKey("StrategyId");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Strategy", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
