using MProj6.DataAccess.Entities;

namespace MProj6.DataAccess.Contracts
{
	public interface IIndexPageItemsRepository
	{
		IQueryable<IndexPage> GetIndexPageItems();
		//   IndexPage GetIndexPageItemById(int id);
		void SaveIndexPageItem(IndexPage page);
		void DeleteIndexPageItemById(IndexPage page);
		IndexPage GetById(int id);
		List<int> GetAllIdIndex();
		void Update(IndexPage page);
		IndexPage GetByTag(string tag);
	}
}
