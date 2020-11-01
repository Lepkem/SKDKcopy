using Microsoft.EntityFrameworkCore;
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

		public Database(IConfiguration config) : base()
		{
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

			modelBuilder.Entity<User>(entity =>
			{
				// Put a unique constraint on the Email column
				entity.HasIndex(u => u.Email)
					.IsUnique();

			});
		}

		public DbSet<User> Users { get; set; }
		public DbSet<UserVerification> UserVerifications { get; set; }
	}
}
