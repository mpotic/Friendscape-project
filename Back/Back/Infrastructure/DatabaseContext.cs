using Back.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Infrastructure
{
	public class DatabaseContext : DbContext
	{
		public DbSet<Person> People { get; set; }
		public DbSet<Friendship> Friendships { get; set; }
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//Kazemo mu da pronadje sve konfiguracije u Assembliju i da ih primeni nad bazom
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
		}
	}
}
