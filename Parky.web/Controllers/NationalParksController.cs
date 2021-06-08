using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Routing;
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

        public async Task<IActionResult> Upsert(int? id)
        {
            //this method appears to populate the view with an empty park (create new) or existing park
            //not wild about the name since its not really DOING an upsert
            var park = new NationalPark();

            if (id == null) //inserts will have a null id
            {
                return View(park);
            }

            park = await _nationalParkRepository.GetAsync(Routing.NationalParkRoute, id.Value);
            if (park == null)
            {
                return NotFound();
            }

            return View(park);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark park)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    await using var fs1 = files[0].OpenReadStream();
                    await using var ms1 = new MemoryStream();
                    await fs1.CopyToAsync(ms1);
                    p1 = ms1.ToArray();

                    park.Picture = p1;
                }
                else
                {
                    var dbPark = await _nationalParkRepository.GetAsync(Routing.NationalParkRoute, park.Id);
                    park.Picture = dbPark.Picture;
                }

                if (park.Id == 0)
                {
                    await _nationalParkRepository.CreateAsync(Routing.NationalParkRoute, park);
                }
                else
                {
                    await _nationalParkRepository.UpdateAsync(Routing.NationalParkRoute + park.Id, park); //expecting the id
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(park);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalParkRepository.DeleteAsync(Routing.NationalParkRoute, id);
            if (status)
            {
                return Json(new {success = true, message = "Delete Successful"});
            }

            return Json(new {success = false, message = "Delete Not Successful"});
        }
    }
}
