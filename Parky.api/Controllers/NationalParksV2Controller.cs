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
    [Route("api/v{version:apiVersion}/nationalparks")] //if a version is not defined it will be 1.0 (its listed as 2.0 below)
    [ApiVersion("2.0")]
    [ApiController]
    //[ApiExplorerSettings(GroupName= "ParkyOpenAPISpecPark")] //helps swagger know that this controller belongs to this group
    [ProducesResponseType(StatusCodes.Status400BadRequest)] //placing this here indicates that any of the methods below can generate this
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _db;
        
        public NationalParksV2Controller(INationalParkRepository db)
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
    }
}
