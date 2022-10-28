using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{
    public class Passenger
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required,StringLength(14, ErrorMessage = "CPF inválido!")]
        public String CPF { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório!"), StringLength(30,ErrorMessage = "Nome inválido!")]
        public String Name { get; set; }
        [Required(ErrorMessage = "O campo Gênero é obrigatório!")]
        public char Gender { get; set; }
        [Required,StringLength(14, ErrorMessage = "Telefone inválido!")]
        public String Phone { get; set; }
        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório!")]
        public DateTime DtBirth { get; set; }
        public DateTime DtRegister { get; set; } = DateTime.Now;
        public Address Address { get;set; }
        public bool Status { get; set; }
    }
}
