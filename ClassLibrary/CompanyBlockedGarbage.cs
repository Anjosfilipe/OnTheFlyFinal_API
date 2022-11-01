using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary
{
    [BsonIgnoreExtraElements]
    public class CompanyBlockedGarbage
    {
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(19, ErrorMessage = "Número de CNPJ inválido")]
        public string CNPJ { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(30, ErrorMessage = "Nome inválido")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(30, ErrorMessage = "Nome inválido")]
        public string NameOpt { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime DtOpen { get; set; }
        public bool? Status { get; set; }
        public Address Address { get; set; }
    }

}
