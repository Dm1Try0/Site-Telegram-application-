using Microsoft.EntityFrameworkCore;
using MProj6.DataAccess.Entities;

namespace MProj6.DataAccess
{
	public class Context : DbContext
	{
		public Context(DbContextOptions<Context> options) : base(options)
		{
			Database.EnsureCreated();
		}
		public DbSet<IndexPage> IndexPageData { get; set; }
		public DbSet<TitleStories> TitleStoriesData { get; set; }
	}
}
