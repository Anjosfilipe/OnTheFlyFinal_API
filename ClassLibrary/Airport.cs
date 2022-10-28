using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{ 
    [BsonIgnoreExtraElements]
    public class Airport
    {
        public string IATA { get; set; }
        public string State { get; set; }
        public string Coutry { get; set; }
    }
}
