using System.ComponentModel.DataAnnotations;

namespace MProj6.DataAccess.Entities
{
	public class IndexPage
	{
		public int Id { get; set; }
		[Display(Name = "Имя")]
		public string? Name { get; set; }
		[Display(Name = "Главное фото")]
		public string? photoTitle { get; set; }
		[Display(Name = "Выберите титульное фото")]
		public string? photoOnPage { get; set; }
		[Display(Name = "Количество фото объекта")]
		public int? pageCount { get; set; }
		[Display(Name = "Тег категории объектов(tag1/tag2/tag3/tag4/tag5/tag6)")] //изминить нейминг тегов под свои нужды
		public string? Tag { get; set; }
		[Display(Name = "Страница на которой будет отображаться объект")]
		public int? PageNumber { get; set; }
	}
}
