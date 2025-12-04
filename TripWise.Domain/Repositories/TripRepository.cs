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
        // GET all trips
        public async Task<IEnumerable<Trip>> GetAllTripsAsync(CancellationToken ct)
        {
            return await _trips.Find(_ => true).ToListAsync(ct);
        }
        // GET trip by ID
        public async Task<Trip?> GetTripByIdAsync(ObjectId tripId, CancellationToken ct)
        {
            return await _trips.Find(t => t.Id == tripId).FirstOrDefaultAsync(ct);
        }
        // CREATE a new trip
        public async Task CreateTripAsync(Trip trip, CancellationToken ct)
        {
            await _trips.InsertOneAsync(trip, cancellationToken: ct);
        }
        // UPDATE an existing trip (full replacement)
        public async Task UpdateTripAsync(Trip trip, CancellationToken ct)
        {
            await _trips.ReplaceOneAsync(t => t.Id == trip.Id, trip, cancellationToken: ct);
        }
        // DELETE a trip by ID
        public async Task DeleteTripAsync(ObjectId tripId, CancellationToken ct)
        {
            await _trips.DeleteOneAsync(t => t.Id == tripId, ct);
        }
        // PATCH a trip (partial update)
        public async Task<bool> PatchTripsAsync(ObjectId tripId, Dictionary<string, object> updates, CancellationToken ct)
        {
            var updateDef = new List<UpdateDefinition<Trip>>();
            var builder = Builders<Trip>.Update;
            foreach (var update in updates)
            {
                updateDef.Add(builder.Set(update.Key, BsonValue.Create(update.Value)));
            }
            var combinedUpdate = builder.Combine(updateDef);
            var result = await _trips.UpdateOneAsync(t => t.Id == tripId, combinedUpdate, cancellationToken: ct);
            return result.ModifiedCount > 0;
        }
    }


}

