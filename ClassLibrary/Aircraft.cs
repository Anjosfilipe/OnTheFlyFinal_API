using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{
    public class Aircraft
    {

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!"), StringLength(6, ErrorMessage = "RAB inválido!")]
        public string RAB { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DataType(DataType.Date)]
        public DateTime DtRegistry { get; set; }
        [DataType(DataType.Date)]
        public DateTime DtLastFlight { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!"), StringLength(19, ErrorMessage = "CNPJ inválido!")]
        public Company Company { get; set; }
        public Aircraft()
        {
            DtRegistry = DateTime.Now;
            this.DtLastFlight = DtRegistry;
        }
    }
}
