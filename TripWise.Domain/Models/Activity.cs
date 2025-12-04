using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace TripWise.Domain.Models
{
    public class Activity
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } = "Activity"; // Flight, Hotel, sport, etc.

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("startTime")]
        public DateTime StartTime { get; set; }

        [BsonElement("endTime")]
        public DateTime EndTime { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }


    }
}
