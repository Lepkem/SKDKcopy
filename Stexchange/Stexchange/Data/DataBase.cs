﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stexchange.Data.Models;

namespace Stexchange.Data
{
	public class Database : DbContext
	{
		private IConfiguration Config { get; }

		

		public Database(DbContextOptions<Database> options, IConfiguration config) : base(options) {
			Config = config.GetSection("DbSettings");
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseMySQL(new MySqlConnectionStringBuilder()
			{
				Server = Config["Server"],
				Port = Config.GetValue<uint>("Port"),
				Database = Config["Database"],
				UserID = Config["Username"],
				Password = Config["Password"]
			}.ConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserVerification>(entity =>
			{
				// Put a unique constraint on the Guid column
				entity.HasIndex(uv => uv.Guid)
					.IsUnique();
			});

			modelBuilder.Entity<UserVerification>()
				.HasOne(uv => uv.User)
				.WithOne(u => u.Verification)
				.HasForeignKey<UserVerification>(uv => uv.Id);

			modelBuilder.Entity<User>(entity =>
			{
				// Put a unique constraint on the Email column
				entity.HasAlternateKey(u => u.Email);
				// Put a unique constraint on the Username column
				entity.HasAlternateKey(u => u.Username);

				entity.Property(u => u.Postal_Code).IsRequired();
				entity.Property(u => u.Password).IsRequired();
				entity.Property(u => u.IsVerified).HasDefaultValue(0);
			});

			modelBuilder.Entity<User>()
				.HasOne(u => u.Verification)
				.WithOne(uv => uv.User)
				.HasForeignKey<UserVerification>(uv => uv.Id);

			modelBuilder.Entity<Listing>(entity =>
            {
				entity.Property(l => l.Title).IsRequired();
				entity.Property(l => l.Description).IsRequired();
				entity.Property(l => l.NameNl).IsRequired();
				entity.Property(l => l.NameLatin).IsRequired();
				entity.Property(l => l.Visible).HasDefaultValue(1);
				entity.Property(l => l.Renewed).HasDefaultValue(0);
			});

			modelBuilder.Entity<Listing>()
				.HasOne(l => l.Owner)
				.WithMany(u => u.Listings)
				.HasForeignKey(l => l.UserId);

			modelBuilder.Entity<ImageData>()
				.HasOne(id => id.Listing)
				.WithMany(l => l.Pictures)
				.HasForeignKey(id => id.ListingId);

			modelBuilder.Entity<FilterListing>()
				.HasKey(fl => new { fl.ListingId, fl.Value });

			modelBuilder.Entity<FilterListing>()
				.HasOne(fl => fl.Listing)
				.WithMany()
				.HasForeignKey(fl => fl.ListingId);

			modelBuilder.Entity<FilterListing>()
				.HasOne(fl => fl.Filter)
				.WithMany()
				.HasForeignKey(fl => fl.Value)
				.HasPrincipalKey(f => f.Value);

			modelBuilder.Entity<Chat>()
				.HasAlternateKey(c => new { c.AdId, c.ResponderId });

			modelBuilder.Entity<Chat>()
				.HasOne(c => c.Listing)
				.WithMany()
				.HasForeignKey(c => c.AdId)
				.HasPrincipalKey(l => l.Id);

			modelBuilder.Entity<Chat>()
				.HasOne(c => c.Responder)
				.WithMany(u => u.ChatInbox)
				.HasForeignKey(c => c.ResponderId)
				.HasPrincipalKey(u => u.Id);

			modelBuilder.Entity<Chat>()
				.HasMany(c => c.Messages)
				.WithOne()
				.HasForeignKey(m => m.ChatId)
				.HasForeignKey(c => c.Id);

			modelBuilder.Entity<Message>(entity =>
			{
				entity.Property(m => m.Content).IsRequired();
			});

			modelBuilder.Entity<Message>()
				.HasOne(m => m.Sender)
				.WithMany()
				.HasForeignKey(m => m.SenderId)
				.HasPrincipalKey(u => u.Id);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<UserVerification> UserVerifications { get; set; }
		public DbSet<Listing> Listings { get; set; }
		public DbSet<ImageData> Images { get; set; }
		public DbSet<Filter> Filters { get; set; }
		public DbSet<FilterListing> FilterListings { get; set; }
		public DbSet<Chat> Chats { get; set; }
		public DbSet<Message> Messages { get; set; }
	}
}
