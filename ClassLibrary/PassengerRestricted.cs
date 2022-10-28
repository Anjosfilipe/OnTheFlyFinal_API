using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
 
        [BsonIgnoreExtraElements]
        public class PassengerRestricted
        {
            [Required]
            public String CPF { get; set; }
        }
    
}
