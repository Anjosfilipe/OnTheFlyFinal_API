using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;


namespace ClassLibrary
{
    [BsonIgnoreExtraElements]
    public class Flight
    {
        public Airport Destiny { get; set; }
        public Aircraft Plane { get; set; }
        public int Sales { get; set; }
        public DateTime Departure { get; set; }
        public bool Status { get; set; }
    }
}
