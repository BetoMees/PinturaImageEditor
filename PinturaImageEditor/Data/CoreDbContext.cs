using Microsoft.EntityFrameworkCore;

namespace PinturaImageEditor.Data
{
	public class CoreDbContext : DbContext
	{
		public CoreDbContext(DbContextOptions options) : base (options)
		{
			
			Database.EnsureCreated();
		}

		public DbSet<ArchiveModel> Archive { get; set; }
	}
}
