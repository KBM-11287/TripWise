using Microsoft.AspNetCore.Mvc;
using TripWise.Domain.Models;
using TripWise.Domain.Repositories;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authorization;

namespace TripWise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripsController : ControllerBase
    {
        private readonly ITripRepository _repository;
        public TripsController(ITripRepository repository)
        {
            _repository = repository;
        }

        // Get /api/trips
        [HttpGet]
        public async Task<IActionResult> GetTrips(CancellationToken ct)
        {
            var trips =  await _repository.GetAllTripsAsync(ct);
            return Ok(trips);
        }
        // Get /api/trips/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrip(string id, CancellationToken ct)
        {
          if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid trip ID");

           var trip = await _repository.GetTripByIdAsync(objectId, ct);
            return trip is null? NotFound() : Ok(trip);
        }

        // POST /api/trips
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody] Trip newTrip, CancellationToken ct)
        {
            newTrip.Id = ObjectId.GenerateNewId();
            await _repository.CreateTripAsync(newTrip, ct);
            return CreatedAtAction(nameof(GetTrips), new { id = newTrip.Id.ToString() }, newTrip);
        }

        // PUT /api/trips/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(string id, [FromBody] Trip updatedTrip, CancellationToken ct)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid trip ID");

            var existingTrip = await _repository.GetTripByIdAsync(objectId, ct);
            if (existingTrip is null)
                return NotFound();
            updatedTrip.Id = objectId;
            await _repository.UpdateTripAsync(updatedTrip, ct);
            return NoContent();
        }

        // DELETE /api/trips/{id}
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(string id, CancellationToken ct)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid trip ID");
            var existingTrip = await _repository.GetTripByIdAsync(objectId, ct);
            if (existingTrip is null)
                return NotFound();
            await _repository.DeleteTripAsync(objectId, ct);
            return NoContent();
        }

        // PATCH /api/trips/{id}
        [Authorize]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTrip(string id, [FromBody] Dictionary<string, object> updates, CancellationToken ct)
        {
            if (!ObjectId.TryParse(id, out var objectId))
                return BadRequest("Invalid trip ID");

            var existingTrip = await _repository.GetTripByIdAsync(objectId, ct);
            if (existingTrip is null)
                return NotFound();

            if (updates == null || updates.Count == 0)
                return BadRequest("No updates provided.");

            var success = await _repository.PatchTripsAsync(objectId, updates, ct);
            if (!success)
                return StatusCode(500, "Failed to update the trip.");
            
            // returning updated trip for client convenience
            var updatedTrip = await _repository.GetTripByIdAsync(objectId, ct);
            return Ok(updatedTrip);
        }

    }

}
