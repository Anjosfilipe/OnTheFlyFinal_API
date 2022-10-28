using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{
        public class PassengerRestrictedGarbage
        {
            [BsonIgnoreExtraElements]
            public class PassengerRestricted
            {
                [Required]
                public String CPF { get; set; }
            }
        }
    
}
