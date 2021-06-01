using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;
using Parky.web.Models;
using Parky.web.Repository;

namespace Parky.web.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;

        public NationalParksController(INationalParkRepository nationalParkRepo)
        {
            _nationalParkRepository = nationalParkRepo;
        }

        public IActionResult Index()
        {
            return View(new NationalPark());
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new {data = await _nationalParkRepository.GetAllAsync(Routing.NationalParkRoute)});
        }
    }
}
