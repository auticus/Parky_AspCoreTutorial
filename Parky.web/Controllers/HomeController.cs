using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parky.web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Parky.web.Models.ViewModel;
using Parky.web.Repository;

namespace Parky.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepo;
        private readonly ITrailRepository _trailRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository nationalParkRepo, ITrailRepository trailRepo)
        {
            _logger = logger;
            _nationalParkRepo = nationalParkRepo;
            _trailRepo = trailRepo;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new IndexViewModel()
            {
                NationalParks = await _nationalParkRepo.GetAllAsync(Routing.NationalParkRoute),
                Trails = await _trailRepo.GetAllAsync(Routing.TrailsRoute)
            };
            return View(vm);
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
