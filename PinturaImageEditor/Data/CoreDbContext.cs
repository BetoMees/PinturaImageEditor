using Microsoft.EntityFrameworkCore;

namespace PinturaImageEditor.Data
{
	public class CoreDbContext : DbContext
	{
		public CoreDbContext()
		{
			Database.EnsureCreated();
		}

		public DbSet<ArchiveModel> Archive { get; set; }
	}
}
