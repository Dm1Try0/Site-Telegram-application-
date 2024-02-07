using Microsoft.AspNetCore.Mvc;
using MProj6.DataAccess.Contracts;
using MProj6.DataAccess.Entities;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static MProj6.DataAccess.Entities.WebHook;

namespace MProj6.Controllers
{
	public class AdminController : Controller
	{
		private static IServiceProvider _serviceProvider;
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IIndexPageItemsRepository repository;
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly ITitleStoriesRepository titleStoriesRepository;

		public AdminController(ILogger<HomeController> logger, IConfiguration configuration, IServiceProvider serviceProvider, IWebHostEnvironment hostingEnvironment, IIndexPageItemsRepository repository, ITitleStoriesRepository titleStoriesRepository)
		{
			_logger = logger;

			_configuration = configuration;

			_serviceProvider = serviceProvider;

			this.repository = repository;

			this.hostingEnvironment = hostingEnvironment;

			this.titleStoriesRepository = titleStoriesRepository;

		}
		public IActionResult Add()
		{
			return View();
		}
		public IActionResult TitleEdit()
		{
			try
			{
				List<TitleStories> titleStories = new();
				foreach (var item in titleStoriesRepository.GetAllTitleStories())
				{
					titleStories.Add(item);
				}
				return View(titleStories);
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}

		}
		public IActionResult Panel()
		{
			try
			{
				List<string> storyinfo = new List<string>();
				foreach (var item in titleStoriesRepository.GetAllTitleStories())
				{
					storyinfo.Add($"{item.Name}" + ":" + $"{item.UrlId}" + ":" + $"{item.Id}");
				}
				ViewBag.titleinfo = storyinfo;
				List<IndexPage> sortDtos = new List<IndexPage>();
				foreach (var item in repository.GetIndexPageItems())
				{
					sortDtos.Add(item);
				}
				return View(sortDtos);
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}

		}
		public IActionResult AddStory()
		{
			try
			{
				return View();
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}

		}
		public IActionResult Edit(IndexPage page, IFormFileCollection photo, IFormFileCollection photoblur, IFormFileCollection videos, IFormFileCollection vidblur, IFormFileCollection titlevid, IFormFileCollection titlevidblur)
		{
			try
			{
				if (ModelState.IsValid)
				{
					//просто фото
					foreach (var uploadedFile in photo)
					{
						string fdsf = uploadedFile.FileName;
						using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", uploadedFile.FileName), FileMode.Create))
						{
							uploadedFile.CopyTo(stream);
						}

						var pag = repository.GetById(page.Id);
						pag.photoOnPage = pag.photoOnPage + "/photo:" + fdsf;
						pag.pageCount++;
						repository.Update(pag);
					}
					//фото с блюром
					foreach (var uploadedFile in photoblur)
					{
						string fdsf = uploadedFile.FileName;
						using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", uploadedFile.FileName), FileMode.Create))
						{
							uploadedFile.CopyTo(stream);
						}

						var pag = repository.GetById(page.Id);
						pag.photoOnPage = pag.photoOnPage + "/blur:" + fdsf;
						pag.pageCount++;
						repository.Update(pag);
					}
					//видео c титульной фоткой
					foreach (var uplvideo in videos)
					{
						foreach (var uplTitlephfVid in titlevid)
						{
							string filenametitlevid = uplTitlephfVid.FileName;

							string filenamevideo = uplvideo.FileName;
							using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", uplvideo.FileName), FileMode.Create))
							{
								uplvideo.CopyTo(stream);
							}
							using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", uplTitlephfVid.FileName), FileMode.Create))
							{
								uplvideo.CopyTo(stream);
							}
							var pag = repository.GetById(page.Id);
							pag.photoOnPage = pag.photoOnPage + "/vid:" + filenamevideo + ":" + filenametitlevid;
							pag.pageCount++;
							repository.Update(pag);
						}
					}
					//видео с блюром и фоткой
					foreach (var uplblurvideo in vidblur)
					{
						foreach (var uplTitlephfVid in titlevidblur)
						{
							string filenametitlevid = uplTitlephfVid.FileName;

							string filenamevideo = uplblurvideo.FileName;
							using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", uplblurvideo.FileName), FileMode.Create))
							{
								uplblurvideo.CopyTo(stream);
							}
							using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", uplTitlephfVid.FileName), FileMode.Create))
							{
								uplblurvideo.CopyTo(stream);
							}
							var pag = repository.GetById(page.Id);
							pag.photoOnPage = pag.photoOnPage + "/vidblur:" + filenamevideo + ":" + filenametitlevid;
							pag.pageCount++;
							repository.Update(pag);
						}
					}
				}
				return View(page);
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}

		}
		public IActionResult Delete(int id, string title)
		{
			try
			{
				if (title != null)
				{
					var delitem = titleStoriesRepository.GetByIdTitleStories(id);
					titleStoriesRepository.DeleteTitleStories(delitem);
				}
				if (title == null)
				{
					var delitem = repository.GetById(id);
					repository.DeleteIndexPageItemById(delitem);
				}
				return RedirectToAction(nameof(AdminController.Panel));
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}

		}
		[HttpPost]
		public async Task<ActionResult<IndexPage>> Check(IndexPage page, IFormFileCollection uploads)
		{
			try
			{
				if (ModelState.IsValid)
				{
					foreach (var uplTitlephfVid in uploads)
					{
						using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", uplTitlephfVid.FileName), FileMode.Create))
						{
							uplTitlephfVid.CopyTo(stream);
						}
						page.photoTitle = uplTitlephfVid.FileName;
						repository.SaveIndexPageItem(page);
					}
				}
				return RedirectToAction(nameof(AdminController.Panel));
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}
		}

		[HttpPost("admin/youkassa/jsonstring")]

		public async Task<Root> youkassaAsync([FromBody] Root context)
		{

			if (context.@event == "payment.waiting_for_capture")
			{
				// context.@object.id;
				//здесь нужно менять пеймент ид, уникальный идемпотенс ключ
				// context.@object.amount.currency
				using (var httpClient = new HttpClient())
				{

					var jsonres = new WebHook.Amount
					{

						value = context.@object.amount.value,
						currency = context.@object.amount.currency
					};
					var options = new JsonSerializerOptions { WriteIndented = true };
					string jsonString = System.Text.Json.JsonSerializer.Serialize(jsonres, options);

					Random r = new Random();
					long random = r.Next(10000, 99999);
					long random2 = r.Next(10000, 99999);
					long random3 = r.Next(1000, 9999);

					using (var request = new HttpRequestMessage(new HttpMethod("POST"), $"https://api.yookassa.ru/v3/payments/{context.@object.id}/capture"))
					{
						request.Headers.TryAddWithoutValidation("Idempotence-Key", $"123e{random3}-e89b-12d3-a456-{random2}{random}20");

						var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(""));//нужно ввести ваш токен
						request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
						request.Content = new StringContent($"{{\r\n  \"amount\": {{\r\n    \"value\": \"{context.@object.amount.value}\",\r\n    \"currency\": \"{context.@object.amount.currency}\"\r\n  }}\r\n}}");
						request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

						var response = await httpClient.SendAsync(request);
					}
				}
			}



			return context;
		}

		public async Task<ActionResult<TitleStories>> CheckStories(TitleStories stories, IFormFileCollection uploads)
		{
			try
			{
				if (ModelState.IsValid)
				{
					foreach (var storiesPhoto in uploads)
					{
						using (var stream = new FileStream(Path.Combine(hostingEnvironment.WebRootPath, "images/", storiesPhoto.FileName), FileMode.Create))
						{
							storiesPhoto.CopyTo(stream);
						}
						stories.Photo = storiesPhoto.FileName;
						titleStoriesRepository.SaveTitleStories(stories);
					}
				}
				return RedirectToAction(nameof(AdminController.Panel));
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}

		}
	}
}
