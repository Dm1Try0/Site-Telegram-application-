using MProj6.DataAccess.Contracts;
using MProj6.DataAccess.Entities;

namespace MProj6.DataAccess.Internals
{
	public class TitleStoriesRepository : ITitleStoriesRepository
	{
		private readonly Context context;

		public TitleStoriesRepository(Context context)
		{
			this.context = context;
		}
		public IQueryable<TitleStories> GetAllTitleStories()
		{
			return context.TitleStoriesData;
		}
		public void SaveTitleStories(TitleStories title)
		{
			context.TitleStoriesData.Add(title);
			context.SaveChanges();

		}
		public void DeleteTitleStories(TitleStories title)
		{
			context.TitleStoriesData.Remove(title);
			context.SaveChanges();
		}
		public void UpdateTitleStories(TitleStories title)
		{
			context.TitleStoriesData.Update(title);
			context.SaveChanges();
		}
		public TitleStories GetByIdTitleStories(int id)
		{
			return context.TitleStoriesData.FirstOrDefault(u => u.Id == id);
		}
	}
}
