using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky.api.Models.DTOs;
using Parky.api.Repository.Interfaces;

namespace Parky.api.Controllers
{
    //when launching this in Postman you can view the controller by going to https://localhost:{port}/api/nationalparks
    //this is because the controller is called NationalParksController, and the route is api/nationalparks (defined in the attribute)
    //this will hit the Get endpoint automatically... 
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/nationalparks")] //if a version is not defined it will be 1.0 (note it is not here)
    [ApiController]
    //[ApiExplorerSettings(GroupName= "ParkyOpenAPISpecPark")] //helps swagger know that this controller belongs to this group
    [ProducesResponseType(StatusCodes.Status400BadRequest)] //placing this here indicates that any of the methods below can generate this
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _db;
        
        public NationalParksController(INationalParkRepository db)
        {
            _db = db;
        }

        //developer note - Get vs Post - I have seen a great many places use Post to retrieve data.  This is because Post is more secure.  If you are not passing
        //any important information as parameters, get should be fine.  If you are using important information like socials or credit card info, use post as it is
        //more secure and does not get logged out on the server (and thus hacked and stolen later)
        /// <summary>
        /// Returns a list of all national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDTO>))]
        public IActionResult GetAllNationalParks()
        {
            var parks = _db.GetNationalParks();

            //do not return the doman model, convert it to a DTO which is for passing back and forth out of the service
            var parksDto = parks.Select(park => park.ToDTO()).ToList();

            return Ok(parksDto);
        }

        //ex: https://localhost:{port}/api/nationalparks/1
        /// <summary>
        /// Get a park by an ID
        /// </summary>
        /// <param name="id">The db ID of the national park</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name="GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int id)
        {
            var park = _db.GetNationalPark(id);
            if (park == null) return NotFound(); //returns a 404 NOT FOUND result

            return Ok(park.ToDTO());
        }

        //https://lcoalhost{port}/api/nationalparks - in postman in the body create the json format for the dto and specify POST
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NationalParkDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO park)
        {
            if (park == null) return BadRequest(ModelState);

            if (_db.NationalParkExists(park.Name))
            {
                ModelState.AddModelError("", "National Park Already Exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalPark = park.ToNationalPark();
            if (!_db.CreateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            //this returns a 201 Created response, and calls the GetNationalPark endpoint to retrieve the object
            //route name, route value, object value

            //error below - when you add versioning and you try to CreatedAtRoute, you need to also pass in the version
            //return CreatedAtRoute("GetNationalPark", new {id = nationalPark.Id}, nationalPark);
            return CreatedAtRoute("GetNationalPark", new
            {
                version=HttpContext.GetRequestedApiVersion().ToString(),
                id = nationalPark.Id
            }, nationalPark);
        }

        //ex: https://localhost:{port}/api/nationalparks/1 - and set postman to PATCH
        [HttpPatch("{id:int}", Name="UpdateNationalPark")] //would use PUT if we were updating the entire resource, not just a part of it
        [ProducesResponseType(StatusCodes.Status204NoContent)] //another way to view them, probably better since they aren't magic numbers
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int id, [FromBody] NationalParkDTO park)
        {
            if (park == null || id != park.Id) return BadRequest(ModelState);

            if (_db.NationalParkExists(park.Name))
            {
                ModelState.AddModelError("", "National Park Already Exists");
                return StatusCode(404, ModelState);
            }

            var nationalPark = park.ToNationalPark();
            if (!_db.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent(); //204 No Content
        }

        //ex: https://localhost:{port}/api/nationalparks/1 - and set postman to DELETE
        [HttpDelete("{id:int}", Name="DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //another way to view them, probably better since they aren't magic numbers
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int id)
        {
            if (!_db.NationalParkExists(id))
            {
                return NotFound();
            }

            var nationalPark = _db.GetNationalPark(id);
            if (!_db.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something went wrong when delete the record {nationalPark.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent(); //204 No Content
        }
    }
}
