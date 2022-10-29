using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{
    [BsonIgnoreExtraElements]
    public class Sales
    {
        public Flight Flight { get; set; }
        public List<Passenger> Passagers { get; set; }
        public bool Reserved { get; set; }
        public bool Sold { get; set; }  
    }
}
