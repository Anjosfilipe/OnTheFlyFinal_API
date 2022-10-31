using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System;
namespace ClassLibrary
{
    [BsonIgnoreExtraElements]
    public class AircraftGarbage
    {
        [Required(ErrorMessage = "Este campo é obrigatório!"), StringLength(6, ErrorMessage = "RAB inválido!")]
        public string RAB { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DataType(DataType.Date)]
        public DateTime DtRegistry { get; set; }
        [DataType(DataType.Date)]
        public DateTime DtLastFlight { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!"), StringLength(18, ErrorMessage = "CNPJ inválido!")]
        public Company Company { get; set; }
    }
}
