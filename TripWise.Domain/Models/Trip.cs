using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TripWise.Domain.Models
{
    public class Trip
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("creatorId")]
        public ObjectId CreatorId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("destination")]
        public string Destination { get; set; } = string.Empty;

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("activities")]
        public List<string> Activities { get; set; } = new();
    }
}
