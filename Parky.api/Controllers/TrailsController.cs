using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Parky.api.Models.DTOs;
using Parky.api.Repository.Interfaces;

namespace Parky.api.Controllers
{
    [Route("api/Trails")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)] //placing this here indicates that any of the methods below can generate this
    public class TrailsController : Controller
    {
        private readonly ITrailRepository _db;
        
        public TrailsController(ITrailRepository db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns a list of all Trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDTO>))]
        public IActionResult GetAllTrails()
        {
            var trails = _db.GetTrails();
            var trailsDto = trails.Select(trail => trail.ToDTO()).ToList();

            return Ok(trailsDto);
        }

        /// <summary>
        /// Get a trail by an ID
        /// </summary>
        /// <param name="id">The db ID of the trail</param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name="GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int id)
        {
            var trail = _db.GetTrail(id);
            if (trail == null) return NotFound(); //returns a 404 NOT FOUND result

            return Ok(trail.ToDTO());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailInsertDTO trailDto)
        {
            if (trailDto == null) return BadRequest(ModelState);

            if (_db.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Already Exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trail = trailDto.ToTrail();
            if (!_db.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new {id = trail.Id}, trail);
        }

        [HttpPatch("{id:int}", Name="UpdateTrail")] //would use PUT if we were updating the entire resource, not just a part of it
        [ProducesResponseType(StatusCodes.Status204NoContent)] //another way to view them, probably better since they aren't magic numbers
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int id, [FromBody] TrailUpdateDTO trailDto)
        {
            if (trailDto == null || id != trailDto.Id) return BadRequest(ModelState);

            if (_db.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Already Exists");
                return StatusCode(404, ModelState);
            }

            var trail = trailDto.ToTrail();
            if (!_db.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent(); //204 No Content
        }

        [HttpDelete("{id:int}", Name="DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)] //another way to view them, probably better since they aren't magic numbers
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int id)
        {
            if (!_db.TrailExists(id))
            {
                return NotFound();
            }

            var trail = _db.GetTrail(id);
            if (!_db.DeleteTrail(trail))
            {
                ModelState.AddModelError("", $"Something went wrong when delete the record {trail.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent(); //204 No Content
        }
    }
}
