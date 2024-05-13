using Honey.Models.Entities;
using Honey.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Honey.Models.Contexts
{
	public class DataBaseContext : DbContext, IDataBaseContext
	{
		public DataBaseContext(DbContextOptions options) : base(options)
		{

		}
		public DbSet<User> Users { get; set; }
		public DbSet<Experience> Experiences { get; set; }
		public DbSet<Education> Educations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			

			modelBuilder.Entity<User>().HasIndex(p => p.Email).IsUnique();
		


			modelBuilder.Entity<User>()
				.HasMany(u => u.Educations)
				.WithOne(e => e.User)
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.Cascade);


			modelBuilder.Entity<User>()
				.HasMany(u => u.Experiences)
				.WithOne(e => e.User)
				.HasForeignKey(e => e.UserId)
				.OnDelete(DeleteBehavior.NoAction);


		}


	}
}
