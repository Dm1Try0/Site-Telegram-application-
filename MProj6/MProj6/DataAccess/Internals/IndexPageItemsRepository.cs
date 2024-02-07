using MProj6.DataAccess.Contracts;
using MProj6.DataAccess.Entities;

namespace MProj6.DataAccess.Internals
{
	public class IndexPageItemsRepository : IIndexPageItemsRepository
	{
		private readonly Context context;

		public IndexPageItemsRepository(Context context)
		{
			this.context = context;
		}

		public IQueryable<IndexPage> GetIndexPageItems()
		{
			return context.IndexPageData;
		}

		/* public IndexPage GetIndexPageItemById(int id)
		 {
			 return context.IndexPageData.FirstOrDefault(x => x.Id == id);
		 }*/
		public void SaveIndexPageItem(IndexPage page)
		{
			context.IndexPageData.Add(page);
			context.SaveChanges();

		}
		public void Update(IndexPage page)
		{
			context.IndexPageData.Update(page);
			context.SaveChanges();
		}
		public void DeleteIndexPageItemById(IndexPage page)
		{
			context.IndexPageData.Remove(page);
			context.SaveChanges();
		}
		public IndexPage GetById(int id)
		{
			return context.IndexPageData.FirstOrDefault(u => u.Id == id);
		}

		public List<int> GetAllIdIndex()
		{
			var result = context.IndexPageData.Select(x => x.Id).ToList();

			return result;
		}
		public IndexPage GetByTag(string tag)
		{
			return context.IndexPageData.FirstOrDefault(u => u.Tag == tag);
		}
	}
}
