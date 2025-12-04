using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripWise.Domain.Models;
using MongoDB.Bson;

namespace TripWise.Domain.Repositories
{
   public interface ITripRepository
    {
        Task<Trip?> GetTripByIdAsync(ObjectId tripId, CancellationToken ct);
        Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken ct);
        Task CreateTripAsync(Trip trip, CancellationToken ct);
        Task UpdateTripAsync(Trip trip, CancellationToken ct);
        Task DeleteTripAsync(ObjectId tripId, CancellationToken ct);
        Task <bool> PatchTripsAsync (ObjectId tripId, Dictionary<string, object> updates, CancellationToken ct);
    }
}
