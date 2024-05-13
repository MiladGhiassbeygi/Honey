using Honey.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace Honey.Models.Services
{
	public interface IDataBaseContext
	{
		 DbSet<User> Users { get; set; }
		 DbSet<Experience> Experiences { get; set; }
		 DbSet<Education> Educations { get; set; }
		

		int SaveChanges();
		int SaveChanges(bool acceptAllChangesOnSuccess);
		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

	}
}
