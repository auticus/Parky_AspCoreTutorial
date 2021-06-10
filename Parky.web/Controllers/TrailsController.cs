using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Parky.web.Models;
using Parky.web.Models.ViewModel;
using Parky.web.Repository;

namespace Parky.web.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepo;
        private readonly ITrailRepository _trailRepo;
        private const string TOKEN_NAME = "JWToken";

        public TrailsController(INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _nationalParkRepo = npRepo;
            _trailRepo = trailRepo;
        }

        public IActionResult Index()
        {
            return View(new Trail());
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            //this is called Upsert so that the view can be called Upsert, which is for the Upsert action... makes things a little difficult to understand
            //considering we have two upserts here... one to actually do the upsert and one to run the UpsertView so renamed this appropriately
            var parks = await _nationalParkRepo.GetAllAsync(Routing.NationalParkRoute, HttpContext.Session.GetString(TOKEN_NAME));
            var vm = new TrailsViewModel()
            {
                NationalParkList = parks.Select(data=> new SelectListItem //represents an item in a SelectList or MultiSelectList rendered as an HTML <option> element
                {
                    Text = data.Name,
                    Value = data.Id.ToString()
                }),
                Trail = new Trail()
            };

            if (id == null)
            {
                return View(vm);
            }

            vm.Trail = await _trailRepo.GetAsync(Routing.TrailsRoute, id.Value, HttpContext.Session.GetString(TOKEN_NAME));
            if (vm.Trail == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        public async Task<IActionResult> GetAllTrails()
        {
            return Json(new {data = await _trailRepo.GetAllAsync(Routing.TrailsRoute, HttpContext.Session.GetString(TOKEN_NAME)) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(Routing.TrailsRoute, vm.Trail, HttpContext.Session.GetString(TOKEN_NAME));
                }
                else
                {
                    await _trailRepo.UpdateAsync(Routing.NationalParkRoute + vm.Trail.Id, vm.Trail, HttpContext.Session.GetString(TOKEN_NAME)); //expecting the id
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var parks = await _nationalParkRepo.GetAllAsync(Routing.NationalParkRoute, HttpContext.Session.GetString(TOKEN_NAME));
                var trailVM = new TrailsViewModel()
                {
                    NationalParkList = parks.Select(data => new SelectListItem //represents an item in a SelectList or MultiSelectList rendered as an HTML <option> element
                    {
                        Text = data.Name,
                        Value = data.Id.ToString()
                    }),
                    Trail = vm.Trail
                };
                return View(trailVM);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(Routing.TrailsRoute, id, HttpContext.Session.GetString(TOKEN_NAME));
            return Json(status ? new { success = true, message = "Delete Successful" } 
                : new { success = false, message = "Delete Not Successful" });
        }
    }
}
