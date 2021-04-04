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
    [Migration("20210404013437_AddPercentage")]
    partial class AddPercentage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.BackTestingResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndTo")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExecutedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReportFileUri")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartFrom")
                        .HasColumnType("datetime2");

                    b.Property<int?>("StrategyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StrategyId");

                    b.ToTable("BackTestingResults");
                });

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
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Expression")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("EventGroups");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Indicator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Indicators");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.JoinStrategyEventGroup", b =>
                {
                    b.Property<int>("StrategyId")
                        .HasColumnType("int");

                    b.Property<int>("EventGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Action")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AfterDays")
                        .HasColumnType("int");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("StrategyId", "EventGroupId");

                    b.HasIndex("EventGroupId");

                    b.ToTable("JoinStrategyEventGroup");
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
                        .HasColumnType("nvarchar(1000)")
                        .HasMaxLength(1000);

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

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.BackTestingResult", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.Strategy", null)
                        .WithMany("BackTestingResults")
                        .HasForeignKey("StrategyId");
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Event", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.EventGroup", null)
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
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.JoinStrategyEventGroup", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.EventGroup", "EventGroup")
                        .WithMany("JoinStrategyEventGroups")
                        .HasForeignKey("EventGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StrategyBuilder.Repository.Entities.Strategy", "Strategy")
                        .WithMany("JoinStrategyEventGroups")
                        .HasForeignKey("StrategyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StrategyBuilder.Repository.Entities.Strategy", b =>
                {
                    b.HasOne("StrategyBuilder.Repository.Entities.User", "CreatedBy")
                        .WithMany("Strategies")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
