using Microsoft.AspNetCore.Mvc;
using TripWise.Domain.Models;
using TripWise.Domain.Repositories;

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
          if (!ObjectId.Tryparse(id, out var objectId))
                return BadRequest("Invalid trip ID");

           var trip = await _repository.GetTripByIdAsync(objectId, ct);
            return trip is null? NotFound() : Ok(trip);
        }

        // POST /api/trips
        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody] Trip newTrip, CancellationToken ct)
        {
            trip.Id = ObjectId.GenerateNewId();
            await _repository.CreateTripAsync(trip, ct);
            // Placeholder implementation, return the created trip
            return CreatedAtAction(nameof(GetTrips), new { id = newTrip.Id.ToString() }, trip);
        }

        // PUT /api/trips/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrip(string id, [FromBody] Trip updatedTrip, CancellationToken ct)
        {
            if (!ObjectId.Tryparse(id, out var objectId))
                return BadRequest("Invalid trip ID");

            var existingTrip = await _repository.GetTripByIdAsync(objectId, ct);
            if (existingTrip is null)
                return NotFound();
            updatedTrip.Id = objectId;
            await _repository.UpdateTripAsync(updatedTrip, ct);
            return NoContent();
        }

        // DELETE /api/trips/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrip(string id, CancellationToken ct)
        {
            if (!ObjectId.Tryparse(id, out var objectId))
                return BadRequest("Invalid trip ID");
            var existingTrip = await _repository.GetTripByIdAsync(objectId, ct);
            if (existingTrip is null)
                return NotFound();
            await _repository.DeleteTripAsync(objectId, ct);
            return NoContent();
        }

    }

}
