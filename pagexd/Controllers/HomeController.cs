using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pagexd.Models;
using pagexd.Repositories;

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

        public IActionResult Index()
        {
            var items = _pageRepository.GetAllPosts();
            return View(items);
        }

        public IActionResult NotAccepted()
        {
            var items = _pageRepository.GetAllPosts();
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
