using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Companys.Utils;
using Companys.Services;
using ClassLibrary;

namespace Companys.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        readonly CompanyUtils companyUtils = new();
        private readonly CompanyServices _companyServices;
        private readonly CompanyGarbageServices _companyGarbageServices;
        private readonly CompanyBlockedServices _companyBlockedServices;
        private readonly AddressServices _addressServices;
        public CompanyController(CompanyServices companyServices, CompanyGarbageServices companyGarbageServices, CompanyBlockedServices companyBlockedServices, AddressServices addressServices)
        {
            _companyServices = companyServices;
            _companyGarbageServices = companyGarbageServices;
            _companyBlockedServices = companyBlockedServices;
            _addressServices = addressServices;
        }
        [HttpPost]
        public ActionResult<Company> CreateCompany(string cnpj, string name, string nameOpt, DateTime dtOpen, string cep, int number, string complement)
        {
            if (cnpj == null) return BadRequest("Campo CNPJ obrigatório!");
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("/", "").Replace(".", "");
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
            cnpj = cnpj.Replace("+", "").Replace("*", "").Replace(",", "").Replace("?", "");
            cnpj = cnpj.Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "");
            cnpj = cnpj.Replace("%", "").Replace("¨", "").Replace("&", "").Replace("(", "");
            cnpj = cnpj.Replace("=", "").Replace("[", "").Replace("]", "").Replace(")", "");
            cnpj = cnpj.Replace("{", "").Replace("}", "").Replace(":", "").Replace(";", "");
            cnpj = cnpj.Replace("<", "").Replace(">", "").Replace("ç", "").Replace("Ç", "");
            Company company = new();
            company.CNPJ = cnpj;
            if (companyUtils.IsCnpjValid(company.CNPJ) == false) return BadRequest("CNPJ inválido!");
            cnpj = company.CNPJ;
            cnpj = cnpj[..2].ToString() + "." + cnpj.Substring(2, 3).ToString() + "." + cnpj.Substring(5, 3).ToString() + '/' + cnpj.Substring(8, 4).ToString() + "-" + cnpj.Substring(12, 2).ToString();
            var comp = _companyServices.GetCompany(cnpj);
            if (comp != null) return BadRequest("Companhia já cadastrada com esse CNPJ!");
            company = new()
            {
                CNPJ = cnpj,
                Name = name,
                NameOpt = nameOpt,
                DtOpen = dtOpen,
                Status = true,
                Address = company.Address
            };
            if (company.Name == null) return BadRequest("Campo nome obrigatório!");
            if (company.Name.Length > 30) return BadRequest("Quantidade de caracteres máximos foi atingida!");
            if (company.NameOpt == null)
            {
                company.NameOpt = company.Name;
            }
            if (company.NameOpt.Length > 30) return BadRequest("Quantidade de caracteres máximos foi atingida!");
            dtOpen = DateTime.Parse(dtOpen.ToShortDateString());
            company.DtOpen = dtOpen;
            TimeSpan result;
            result = DateTime.Now - company.DtOpen;
            if (result.Days / 30 < 6)
            {
                return BadRequest($"A companhia tem {result.Days / 30} meses, o tempo é insufiente para finalizar o cadastro!");
            }
            if (company.Address.ZipCode.Length > 9) return BadRequest("Quantidade de caracteres máximos excedidos");
            if (company.Address.Complement.Length > 10) return BadRequest("Quantidade de caracteres máximos excedidos");
            company.Address = _addressServices.GetAddress(cep);
            if (company.Address == null) return NotFound("Endereço não encontrado!");
            else
            {
                company.Address.Number = number;
                company.Address.Complement = complement;
            }
            _companyServices.CreateCompany(company);
            return CreatedAtRoute("GetCompany", new { cnpj = company.CNPJ.ToString() }, company);
        }
        [HttpGet]
        public ActionResult<List<Company>> GetAllCompany() => _companyServices.GetAllCompany();
        [HttpGet("{cnpj}", Name = "GetCompany")]
        public ActionResult<Company> GetCompany(string cnpj)
        {
            if (cnpj == null) return BadRequest("Campo CNPJ obrigatório!");
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("%2F", "/");
            cnpj = cnpj.Replace("/", "").Replace(".", "");
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
            cnpj = cnpj.Replace("+", "").Replace("*", "").Replace(",", "").Replace("?", "");
            cnpj = cnpj.Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "");
            cnpj = cnpj.Replace("%", "").Replace("¨", "").Replace("&", "").Replace("(", "");
            cnpj = cnpj.Replace("=", "").Replace("[", "").Replace("]", "").Replace(")", "");
            cnpj = cnpj.Replace("{", "").Replace("}", "").Replace(":", "").Replace(";", "");
            cnpj = cnpj.Replace("<", "").Replace(">", "").Replace("ç", "").Replace("Ç", "");
            cnpj = cnpj[..2].ToString() + "." + cnpj.Substring(2, 3).ToString() + "." + cnpj.Substring(5, 3).ToString() + '/' + cnpj.Substring(8, 4).ToString() + "-" + cnpj.Substring(12, 2).ToString();
            var company = _companyServices.GetCompany(cnpj);
            if (company == null) return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            return Ok(company);
        }
        [HttpPut]
        public ActionResult<Company> PutCompany([FromQuery] string cnpj, string nameOpt, bool status, string cep, int number, string complement)
        {
            if (cnpj == null) return BadRequest("Campo CNPJ obrigatório!");
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("%2F", "/");
            cnpj = cnpj.Replace("/", "").Replace(".", "");
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
            cnpj = cnpj.Replace("+", "").Replace("*", "").Replace(",", "").Replace("?", "");
            cnpj = cnpj.Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "");
            cnpj = cnpj.Replace("%", "").Replace("¨", "").Replace("&", "").Replace("(", "");
            cnpj = cnpj.Replace("=", "").Replace("[", "").Replace("]", "").Replace(")", "");
            cnpj = cnpj.Replace("{", "").Replace("}", "").Replace(":", "").Replace(";", "");
            cnpj = cnpj.Replace("<", "").Replace(">", "").Replace("ç", "").Replace("Ç", "");
            cnpj = cnpj[..2].ToString() + "." + cnpj.Substring(2, 3).ToString() + "." + cnpj.Substring(5, 3).ToString() + '/' + cnpj.Substring(8, 4).ToString() + "-" + cnpj.Substring(12, 2).ToString();
            Company companyIn = new()
            {
                CNPJ = cnpj,
                NameOpt = nameOpt,
                Status = status,
            };
            if (companyUtils.IsCnpjValid(companyIn.CNPJ) == false) return BadRequest("CNPJ inválido!");
            if (companyIn.CNPJ == companyIn.CNPJ)
            {
                if (companyIn.NameOpt == null) return BadRequest("Preencha o campo name optional!");
                if (companyIn.NameOpt.Length > 30) return BadRequest("Quantidade de caracteres máximos foi atingida!");
                var company = _companyServices.GetCompany(cnpj);
                if (company == null) return NotFound("Algo deu errado na requisição, companhia não encontrada!");
                if (company.Address.ZipCode.Length > 9) return BadRequest("Quantidade de caracteres máximos excedidos");
                if (company.Address.Complement.Length > 10) return BadRequest("Quantidade de caracteres máximos excedidos");
                var address = _addressServices.GetAddress(cep);
                if (address == null) return NotFound("Endereço não encontrado!");
                else
                {
                    companyIn.Name = company.Name;
                    companyIn.Address = address;
                    companyIn.Address.Number = number;
                    companyIn.Address.Complement = complement;
                    companyIn.DtOpen = company.DtOpen;
                    _companyServices.UpdateCompany(companyIn, cnpj);
                    if (company.Status == false)
                    {
                        CompanyBlocked companyBlocked = new()
                        {
                            CNPJ = companyIn.CNPJ,
                            Name = companyIn.Name,
                            NameOpt = companyIn.NameOpt,
                            DtOpen = companyIn.DtOpen,
                            Status = companyIn.Status,
                            Address = companyIn.Address
                        };
                        _companyBlockedServices.CreateCompanyBlocked(companyBlocked);
                    }
                    return NoContent();
                }
            }
            else
                return BadRequest("Impossivel editar o campo CNPJ!");
        }
        [HttpDelete]
        public ActionResult<Company> DeleteCompany(string cnpj)
        {
            if (cnpj == null) return BadRequest("Campo CNPJ obrigatório!");
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("%2F", "/");
            cnpj = cnpj.Replace("/", "").Replace(".", "");
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
            cnpj = cnpj.Replace("+", "").Replace("*", "").Replace(",", "").Replace("?", "");
            cnpj = cnpj.Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "");
            cnpj = cnpj.Replace("%", "").Replace("¨", "").Replace("&", "").Replace("(", "");
            cnpj = cnpj.Replace("=", "").Replace("[", "").Replace("]", "").Replace(")", "");
            cnpj = cnpj.Replace("{", "").Replace("}", "").Replace(":", "").Replace(";", "");
            cnpj = cnpj.Replace("<", "").Replace(">", "").Replace("ç", "").Replace("Ç", "");
            cnpj = cnpj.Replace("%2F", "/");
            cnpj = cnpj[..2].ToString() + "." + cnpj.Substring(2, 3).ToString() + "." + cnpj.Substring(5, 3).ToString() + '/' + cnpj.Substring(8, 4).ToString() + "-" + cnpj.Substring(12, 2).ToString();
            var company = _companyServices.GetCompany(cnpj);
            if (company == null) return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            CompanyGarbage companyGarbage = new()
            {
                CNPJ = company.CNPJ,
                Name = company.Name,
                NameOpt = company.NameOpt,
                DtOpen = company.DtOpen,
                Status = company.Status,
                Address = company.Address
            };
            _companyGarbageServices.CreateCompanyGarbage(companyGarbage);
            _companyServices.RemoveCompany(company);
            return NoContent();
        }
    }
}
