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
    [Migration("20201118150147_filters")]
    partial class filters
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Stexchange.Data.Models.Filter", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnName("value")
                        .HasColumnType("varchar(20)");

                    b.HasKey("Value");

                    b.ToTable("Filters");
                });

            modelBuilder.Entity("Stexchange.Data.Models.FilterListing", b =>
                {
                    b.Property<long>("ListingId")
                        .HasColumnName("listing_id")
                        .HasColumnType("bigint(20) unsigned");

                    b.Property<string>("Value")
                        .HasColumnName("filter_value")
                        .HasColumnType("varchar(20)");

                    b.HasKey("ListingId", "Value");

                    b.HasIndex("Value");

                    b.ToTable("FilterListings");
                });

            modelBuilder.Entity("Stexchange.Data.Models.ImageData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("serial");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnName("image")
                        .HasColumnType("LONGBLOB");

                    b.Property<long>("ListingId")
                        .HasColumnName("listing_id")
                        .HasColumnType("bigint(20) unsigned");

                    b.HasKey("Id");

                    b.HasIndex("ListingId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Stexchange.Data.Models.Listing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("serial");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasColumnType("timestamp");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<string>("NameLatin")
                        .IsRequired()
                        .HasColumnName("name_lt")
                        .HasColumnType("varchar(30)");

                    b.Property<string>("NameNl")
                        .IsRequired()
                        .HasColumnName("name_nl")
                        .HasColumnType("varchar(30)");

                    b.Property<int>("Quantity")
                        .HasColumnName("quantity")
                        .HasColumnType("int");

                    b.Property<bool>("Renewed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("renewed")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("varchar(80)");

                    b.Property<long>("UserId")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint(20) unsigned");

                    b.Property<bool>("Visible")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("visible")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("Stexchange.Data.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("serial");

                    b.Property<DateTime>("Created_At")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created_at")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("varchar(254)");

                    b.Property<bool>("IsVerified")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("verified")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("varbinary(64)");

                    b.Property<string>("Postal_Code")
                        .IsRequired()
                        .HasColumnName("postal_code")
                        .HasColumnType("char(6)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnName("username")
                        .HasColumnType("varchar(15)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Email");

                    b.HasAlternateKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Stexchange.Data.Models.UserVerification", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnName("user_id")
                        .HasColumnType("bigint(20) unsigned");

                    b.Property<byte[]>("Guid")
                        .IsRequired()
                        .HasColumnName("verification_code")
                        .HasColumnType("varbinary(16)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Guid");

                    b.ToTable("UserVerifications");
                });

            modelBuilder.Entity("Stexchange.Data.Models.FilterListing", b =>
                {
                    b.HasOne("Stexchange.Data.Models.Listing", "Listing")
                        .WithMany("Categories")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Stexchange.Data.Models.Filter", "Filter")
                        .WithMany()
                        .HasForeignKey("Value")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Stexchange.Data.Models.ImageData", b =>
                {
                    b.HasOne("Stexchange.Data.Models.Listing", "Listing")
                        .WithMany("Pictures")
                        .HasForeignKey("ListingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Stexchange.Data.Models.Listing", b =>
                {
                    b.HasOne("Stexchange.Data.Models.User", "Owner")
                        .WithMany("Listings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Stexchange.Data.Models.UserVerification", b =>
                {
                    b.HasOne("Stexchange.Data.Models.User", "User")
                        .WithOne("Verification")
                        .HasForeignKey("Stexchange.Data.Models.UserVerification", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
