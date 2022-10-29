using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{ 

    [BsonIgnoreExtraElements]
    public class Airport
    {
        [Required]
        public string Iata { get; set; }
        public string State { get; set; }
        public string Coutry { get; set; }
    }
}
