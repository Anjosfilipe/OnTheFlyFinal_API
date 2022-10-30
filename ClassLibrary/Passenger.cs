using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{
    [BsonIgnoreExtraElements]
    public class Passenger
    {
        [Required(ErrorMessage = "O campo CPF é obrigatório!")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "CPF inválido!")]
        public String CPF { get; set; }
        [Required(ErrorMessage = "O campo Nome é obrigatório!")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Número de caracteres excede o limite!!")]
        public String Name { get; set; }
        [Required(ErrorMessage = "O campo Gênero é obrigatório!")]
        [StringLength(1, ErrorMessage = "Gênero inválido! Digite M para Masculino ou F para Feminino")]
        public char Gender { get; set; }
        [Required]
        [StringLength(14, MinimumLength = 8, ErrorMessage = "Telefone inválido!")]
        public String Phone { get; set; }
        [Required(ErrorMessage = "O campo Data de Nascimento é obrigatório!")]
        [DataType(DataType.Date)]
        public DateTime DtBirth { get; set; }
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime DtRegister { get; set; }
        public bool Status { get; set; }
        public Address Address { get; set; }
    }
}
