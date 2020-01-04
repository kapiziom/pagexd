using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pagexd.Models;
using pagexd.Repositories;
using X.PagedList;

namespace pagexd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPageRepository _pageRepository;
        public HomeController(ILogger<HomeController> logger, IPageRepository pageRepository)
        {
            _pageRepository = pageRepository;
            _logger = logger;
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var items = _pageRepository.GetAllPostsByAcceptance().ToPagedList(pageNumber, pageSize);
            return View(items);
        }

        public IActionResult NotAccepted(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var items = _pageRepository.GetAllPostsByCreation().ToPagedList(pageNumber, pageSize);
            return View(items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
