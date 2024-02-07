using MProj6.DataAccess.Entities;

namespace MProj6.DataAccess.Contracts
{
	public interface ITitleStoriesRepository
	{
		void DeleteTitleStories(TitleStories title);
		void SaveTitleStories(TitleStories title);
		IQueryable<TitleStories> GetAllTitleStories();

		void UpdateTitleStories(TitleStories title);
		TitleStories GetByIdTitleStories(int id);

	}
}
