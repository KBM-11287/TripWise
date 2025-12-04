using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripWise.Domain.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TripWise.Domain.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly IMongoCollection<Trip> _trips;

        public TripRepository(IMongoDatabase database)
        {
            _trips = database.GetCollection<Trip>("Trips");
        }

        public async Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken ct)
        {
            return await _trips.Find(_ => true).ToListAsync(ct);
        }

        public async Task<Trip?> GetTripByIdAsync(ObjectId tripId, CancellationToken ct)
        {
            return await _trips.Find(t => t.Id == tripId).FirstOrDefaultAsync(ct);
        }

        public async Task CreateTripAsync(Trip trip, CancellationToken ct)
        {
            await _trips.InsertOneAsync(trip, cancellationToken: ct);
        }

        public async Task UpdateTripAsync(Trip trip, CancellationToken ct)
        {
            await _trips.ReplaceOneAsync(t => t.Id == trip.Id, trip, cancellationToken: ct);
        }

        public async Task DeleteTripAsync(ObjectId tripId, CancellationToken ct)
        {
            await _trips.DeleteOneAsync(t => t.Id == tripId, ct);
        }
    }


}

