using Microsoft.AspNetCore.Mvc;
using MProj6.DataAccess.Contracts;
using MProj6.DataAccess.Entities;
using MProj6.Models;
using System.Diagnostics;

namespace MProj6.Controllers
{
	public class HomeController : Controller
	{
		private static IServiceProvider _serviceProvider;
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _configuration;
		private readonly IIndexPageItemsRepository repository;
		private readonly IWebHostEnvironment hostingEnvironment;
		private readonly ITitleStoriesRepository titleStoriesRepository;
		public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IServiceProvider serviceProvider, IWebHostEnvironment hostingEnvironment, IIndexPageItemsRepository repository, ITitleStoriesRepository titleStoriesRepository)
		{
			_logger = logger;

			_configuration = configuration;

			_serviceProvider = serviceProvider;

			this.hostingEnvironment = hostingEnvironment;
			this.repository = repository;
			this.titleStoriesRepository = titleStoriesRepository;

		}
		//  [HttpGet("{id:int}")]
		public IActionResult Index()
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
		public IActionResult ItemPage(int id, int page, int onlyblur, string premium)
		{
			try
			{
				if (page < 1)
				{
					page = 1;
				}
				var a = GetServiceRepository();
				var pagev = a.GetById(id);
				//лист для получения значений из бд
				List<IndexPage> sortDtos = new List<IndexPage>
			{
				pagev
			};
				int countpage = page * 30;//получениеи страницы из адресной строки и умножение на 30, что бы узнать с какого по какой ид нужно доставать фото

				List<string> ph = new();
				foreach (var item in sortDtos)
				{
					//делим сторку из бд что бы получить фото по элементам добавляе в массив
					string[] phot = item.photoOnPage.Split("/");
					//получаем длинну массива что бы знать сколько фоток
					double photocount = phot.Length;
					//делим фотки на 30 (30 фото на 1 стр) 
					double pc = photocount / 30.0;
					//округляем страницы для того что бы плучать точные значения создания страниц
					int linkpagecount = (int)Math.Ceiling(pc);
					//передача страниц числа в представление 
					ViewBag.pagecounts = linkpagecount + 1;
					int ifinish = countpage - 30; //получание стартового значения фоток
					ViewBag.Name = $"{item.Name}"; //передача имени обьекта в представление
					ViewBag.Id = $"{item.Id}";
					ViewBag.onlyblur = onlyblur;
					if (premium != null)
					{
						ViewBag.Premium = $"{premium}";

					}
					if (onlyblur == 0)
					{
						for (int i = ifinish; i < countpage; i++) //добавление фото в лист для передачи в представление
						{

							if (i >= phot.Length)
							{
								break; //проверка не закончились ли фото что бы не получить ошибку
							}
							ph.Add(phot[i]); //добавление фото в viewbag
						}
					}
					if (onlyblur == 1)
					{
						List<string> listforpagecount = new List<string>();
						for (int i = 0; i < phot.Length; i++)
						{
							if (i >= phot.Length)
							{
								break;//проверка не закончились ли фото что бы не получить ошибку
							}
							if (phot[i].StartsWith("blur:") || phot[i].StartsWith("vidblur:"))
							{
								listforpagecount.Add(phot[i]); //добавление фото в viewbag
							}
						}
						for (int i = ifinish; i < countpage; i++)
						{
							if (i >= listforpagecount.Count())
							{
								break;//проверка не закончились ли фото что бы не получить ошибку
							}
							ph.Add(listforpagecount[i]); //добавление фото в viewbag
						}
						double photocount2 = (double)listforpagecount.Count();
						//делим фотки на 30 (30 фото на 1 стр) 
						double pc2 = photocount2 / 30.0;
						//округляем страницы для того что бы плучать точные значения создания страниц
						int linkpagecount2 = (int)Math.Ceiling(pc2);
						//передача страниц числа в представление 
						ViewBag.pagecounts = linkpagecount2 + 1;
					}
					ViewBag.ph = ph;
				}

				return View(sortDtos);
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}
		}
		public IActionResult All(int page, string tag, string premium)
		{
			try
			{
				if (page < 1)
				{
					page = 1;
				}
				if (premium != null)
				{
					ViewBag.premium = $"{premium}";

				}
				List<string> storyinfo = new List<string>();
				foreach (var item in titleStoriesRepository.GetAllTitleStories())
				{
					storyinfo.Add($"{item.Name}" + ":" + $"{item.UrlId}" + ":" + $"{item.Id}" + ":" + $"{item.Photo}");
				}
				ViewBag.titleinfo = storyinfo;

				var a = GetServiceRepository();
				List<IndexPage> sortDtos = new List<IndexPage>();
				int countpage = page * 51; //получениеи страницы из адресной строки и умножение на 51, что бы узнать с какого по какой ид нужно доставать фото

				if (tag == null)
				{
					int ipc = a.GetIndexPageItems().Count();
					double pc = ipc / 51.0;//делим фотки на 51 (51 фото на 1 стр) 
					int linkpagecount = (int)Math.Ceiling(pc); //округляем страницы для того что бы плучать точные значения создания страниц
					ViewBag.pagecounts = linkpagecount + 1; //сколько страниц нужно
				}

				int ifinish = countpage - 51;

				foreach (var item in a.GetIndexPageItems())//добавление фото в лист для передачи в представление
				{
					if (tag != null)
					{
						if (item.Tag == tag)
						{
							sortDtos.Count();
							sortDtos.Add(item);
						}
					}
					if (tag == null)
					{
						sortDtos.Add(item);
					}
				}
				List<IndexPage> returnpage = new List<IndexPage>();//создание нового листа для вывода
				for (int i = ifinish; i < countpage; i++)
				{
					if (sortDtos.Count() <= i)
					{
						break;
					}
					returnpage.Add(sortDtos[i]);
				}
				if (tag != null)
				{
					double pc = sortDtos.Count() / 51.0;//делим фотки на 51 (51 фото на 1 стр) 
					int linkpagecount = (int)Math.Ceiling(pc); //округляем страницы для того что бы плучать точные значения создания страниц
					ViewBag.pagecounts = linkpagecount + 1; //сколько страниц нужно
				}
				return View(returnpage);
			}
			catch (Exception ex)
			{
				return RedirectToAction(nameof(HomeController.Error));
			}

		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
		private List<int> GetAllId()
		{
			try
			{
				var usersService = GetServiceRepository();
				var result = usersService.GetAllIdIndex();
				return result;
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		private IIndexPageItemsRepository GetServiceRepository()
		{
			return _serviceProvider.GetService<IIndexPageItemsRepository>();
		}
	}
}