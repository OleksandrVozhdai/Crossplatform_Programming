using Microsoft.AspNetCore.Mvc;

namespace disease_outbreaks_detector.Controllers
{
	public class HomePageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
