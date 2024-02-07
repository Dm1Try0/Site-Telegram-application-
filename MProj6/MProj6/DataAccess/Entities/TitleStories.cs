using System.ComponentModel.DataAnnotations;

namespace MProj6.DataAccess.Entities
{
	public class TitleStories
	{
		public int Id { get; set; }
		[Display(Name = "Имя")]
		public string? Name { get; set; }
		[Display(Name = "Фото для истории")]
		public string? Photo { get; set; }
		[Display(Name = "Введите Id объекта")]
		public string? UrlId { get; set; }

	}
}
