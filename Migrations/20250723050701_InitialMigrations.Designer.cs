﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RimPoc.Data;

#nullable disable

namespace rim_poc.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250723050701_InitialMigrations")]
    partial class InitialMigrations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationProduct", b =>
                {
                    b.Property<int>("ApplicationId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.HasKey("ApplicationId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ApplicationProducts", (string)null);
                });

            modelBuilder.Entity("RimPoc.Data.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AppNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("CountryId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("RiskId")
                        .HasColumnType("integer");

                    b.Property<string>("SerialNum")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("StatusDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AppNumber");

                    b.HasIndex("CountryId");

                    b.HasIndex("RiskId");

                    b.HasIndex("SerialNum")
                        .IsUnique();

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("RimPoc.Data.ControlledVocabulary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("ParentId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("Category");

                    b.HasIndex("ParentId");

                    b.HasIndex("Category", "Code")
                        .IsUnique();

                    b.ToTable("ControlledVocabularies");
                });

            modelBuilder.Entity("RimPoc.Data.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("RimPoc.Data.DefaultTemplates", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("SubmissionTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("IsActive");

                    b.HasIndex("SubmissionTypeId");

                    b.HasIndex("Country", "SubmissionTypeId", "Name")
                        .IsUnique();

                    b.ToTable("DefaultTemplates");
                });

            modelBuilder.Entity("RimPoc.Data.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CodeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int?>("EnergySourceId")
                        .HasColumnType("integer");

                    b.Property<int?>("FunctionsId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int?>("MedicalSpecialtyId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("ProductFamilyId")
                        .HasColumnType("integer");

                    b.Property<bool>("RadiationEmitting")
                        .HasColumnType("boolean");

                    b.Property<int?>("RadiationTypeId")
                        .HasColumnType("integer");

                    b.Property<int?>("SubtypeId")
                        .HasColumnType("integer");

                    b.Property<int?>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EnergySourceId");

                    b.HasIndex("FunctionsId");

                    b.HasIndex("MedicalSpecialtyId");

                    b.HasIndex("ProductFamilyId");

                    b.HasIndex("RadiationTypeId");

                    b.HasIndex("SubtypeId");

                    b.HasIndex("TypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("RimPoc.Data.ProductFamily", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("ModifiedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("ProductFamilies");
                });

            modelBuilder.Entity("RimPoc.Data.Submission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ApplicationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("SequenceNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime?>("StatusDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("SubmissionActivityId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("SubmissionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SubmissionNumber")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("Status");

                    b.HasIndex("SubmissionActivityId");

                    b.HasIndex("SubmissionNumber");

                    b.HasIndex("ApplicationId", "SequenceNumber")
                        .IsUnique();

                    b.ToTable("Submissions");
                });

            modelBuilder.Entity("ApplicationProduct", b =>
                {
                    b.HasOne("RimPoc.Data.Application", null)
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RimPoc.Data.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RimPoc.Data.Application", b =>
                {
                    b.HasOne("RimPoc.Data.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RimPoc.Data.ControlledVocabulary", "RiskVocabulary")
                        .WithMany("ApplicationsAsRisk")
                        .HasForeignKey("RiskId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Country");

                    b.Navigation("RiskVocabulary");
                });

            modelBuilder.Entity("RimPoc.Data.ControlledVocabulary", b =>
                {
                    b.HasOne("RimPoc.Data.ControlledVocabulary", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("RimPoc.Data.DefaultTemplates", b =>
                {
                    b.HasOne("RimPoc.Data.ControlledVocabulary", "SubmissionType")
                        .WithMany()
                        .HasForeignKey("SubmissionTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SubmissionType");
                });

            modelBuilder.Entity("RimPoc.Data.Product", b =>
                {
                    b.HasOne("RimPoc.Data.ControlledVocabulary", "EnergySource")
                        .WithMany("ProductsAsEnergySource")
                        .HasForeignKey("EnergySourceId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RimPoc.Data.ControlledVocabulary", "Functions")
                        .WithMany("ProductsAsFunctions")
                        .HasForeignKey("FunctionsId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RimPoc.Data.ControlledVocabulary", "MedicalSpecialty")
                        .WithMany("ProductsAsMedicalSpecialty")
                        .HasForeignKey("MedicalSpecialtyId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RimPoc.Data.ProductFamily", "ProductFamily")
                        .WithMany("Products")
                        .HasForeignKey("ProductFamilyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RimPoc.Data.ControlledVocabulary", "RadiationType")
                        .WithMany("ProductsAsRadiationType")
                        .HasForeignKey("RadiationTypeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RimPoc.Data.ControlledVocabulary", "ProductSubtype")
                        .WithMany("ProductsAsSubtype")
                        .HasForeignKey("SubtypeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RimPoc.Data.ControlledVocabulary", "ProductType")
                        .WithMany("ProductsAsType")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("EnergySource");

                    b.Navigation("Functions");

                    b.Navigation("MedicalSpecialty");

                    b.Navigation("ProductFamily");

                    b.Navigation("ProductSubtype");

                    b.Navigation("ProductType");

                    b.Navigation("RadiationType");
                });

            modelBuilder.Entity("RimPoc.Data.Submission", b =>
                {
                    b.HasOne("RimPoc.Data.Application", "Application")
                        .WithMany("Submissions")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RimPoc.Data.ControlledVocabulary", "SubmissionActivity")
                        .WithMany()
                        .HasForeignKey("SubmissionActivityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("SubmissionActivity");
                });

            modelBuilder.Entity("RimPoc.Data.Application", b =>
                {
                    b.Navigation("Submissions");
                });

            modelBuilder.Entity("RimPoc.Data.ControlledVocabulary", b =>
                {
                    b.Navigation("ApplicationsAsRisk");

                    b.Navigation("Children");

                    b.Navigation("ProductsAsEnergySource");

                    b.Navigation("ProductsAsFunctions");

                    b.Navigation("ProductsAsMedicalSpecialty");

                    b.Navigation("ProductsAsRadiationType");

                    b.Navigation("ProductsAsSubtype");

                    b.Navigation("ProductsAsType");
                });

            modelBuilder.Entity("RimPoc.Data.ProductFamily", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
