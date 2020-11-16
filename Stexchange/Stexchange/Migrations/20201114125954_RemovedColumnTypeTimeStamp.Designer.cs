﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Stexchange.Data;

namespace Stexchange.Migrations
{
    [DbContext(typeof(Database))]
    [Migration("20201114125954_RemovedColumnTypeTimeStamp")]
    partial class RemovedColumnTypeTimeStamp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Stexchange.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Created_At")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasColumnType("VARCHAR(254)")
                        .HasMaxLength(254);

                    b.Property<string>("Password")
                        .HasColumnType("VARCHAR(64)")
                        .HasMaxLength(64);

                    b.Property<string>("Postal_Code")
                        .HasColumnType("CHAR(6)")
                        .HasMaxLength(6);

                    b.Property<string>("Username")
                        .HasColumnType("VARCHAR(15)")
                        .HasMaxLength(15);

                    b.Property<int?>("VerificationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("VerificationId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Stexchange.Data.Models.UserVerification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<byte[]>("Guid")
                        .IsRequired()
                        .HasColumnType("varbinary(16)");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.ToTable("UserVerifications");
                });

            modelBuilder.Entity("Stexchange.Data.Models.User", b =>
                {
                    b.HasOne("Stexchange.Data.Models.UserVerification", "Verification")
                        .WithMany()
                        .HasForeignKey("VerificationId");
                });
#pragma warning restore 612, 618
        }
    }
}
