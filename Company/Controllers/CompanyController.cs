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
    
        public CompanyController(CompanyServices companyServices, CompanyGarbageServices companyGarbageServices, CompanyBlockedServices companyBlockedServices)
        {
            _companyServices = companyServices;
            _companyGarbageServices = companyGarbageServices;
            _companyBlockedServices = companyBlockedServices;
          
        }
        [HttpPost]
        public ActionResult<Company> CreateCompany(string cnpj, string name, string nameopc, DateTime dtopen, string cep, int numero, string complemento)
        {
            Company company = new Company() { CNPJ = cnpj, Name = name, NameOpt =  nameopc, DtOpen = dtopen, Status = true};    
            var address = _companyServices.GetAddress(cep); // api
            if (address == null)
                return NotFound("Endereço não encontrado!");
            else
            {
                address.Number = numero;
                address.Complement = complemento;
                company.Address = address;
            }
            if (companyUtils.IsCnpjValid(company.CNPJ) == false)
            {
                return BadRequest("CNPJ inválido!");
            }
            else
            {
                var Cnpj = company.CNPJ;
                company.CNPJ = Cnpj.Substring(0, 2).ToString() + "." + Cnpj.Substring(2, 3).ToString() + "." + Cnpj.Substring(5, 3).ToString() + '/' + Cnpj.Substring(8, 4).ToString() + "-" + Cnpj.Substring(12, 2).ToString();
                var comp = _companyServices.GetCompany(company.CNPJ);
                if (comp != null) return BadRequest("Companhia já cadastrada com esse CNPJ!");
                if (company.NameOpt == null)
                {
                    company.NameOpt = company.Name;
                }
                DateTime date = company.DtOpen;
                date = DateTime.Parse(date.ToShortDateString());
                if (date > DateTime.Now)
                {
                    return BadRequest("A data de abertura não pode ser maior que a data atual!");
                }
                TimeSpan result;
                result = DateTime.Now - date;
                if (result.Days / 30 < 6)
                {
                    return BadRequest($"A companhia tem {result.Days / 30} meses, o tempo é insufiente para finalizar o cadastro!");
                }
                _companyServices.CreateCompany(company);
                return CreatedAtRoute("GetCompany", new { cnpj = company.CNPJ.ToString() }, company);
            }
        }
        [HttpGet]
        public ActionResult<List<Company>> GetAllCompany() => _companyServices.GetAllCompany();
        [HttpGet("{cnpj}", Name = "GetCompany")]
        public ActionResult<Company> GetCompany(string cnpj)
        {
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("%2F", "/");
            var company = _companyServices.GetCompany(cnpj);
            if (company == null)
                if (company == null)
                    return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            return Ok(company);
        }
        [HttpPut]
        public ActionResult<Company> PutCompany([FromQuery] string cnpj, string nameOpt, bool status, string cep, int number, string complement)
        {
            var address = _companyServices.GetAddress(cep); // api 
            if (address == null)
                return NotFound("Endereço não encontrado!");
            else
            {
                Company companyIn = new() { CNPJ = cnpj, NameOpt = nameOpt, Status = status };
                if (companyUtils.IsCnpjValid(companyIn.CNPJ) == false)
                    return BadRequest("CNPJ inválido!");
                var company = _companyServices.GetCompany(cnpj);
                if (company == null)
                    return NotFound("Algo deu errado na requisição, companhia não encontrada!");
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
                        CNPJ = company.CNPJ,
                        Name = company.Name,
                        NameOpt = company.NameOpt,
                        DtOpen = company.DtOpen,
                        Status = company.Status,
                        Address = company.Address
                    };
                    _companyBlockedServices.CreateCompanyBlocked(companyBlocked);
                }
                return NoContent();
            }
        }
        [HttpDelete]
        public ActionResult<Company> DeleteCompany(string cnpj)
        {
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("%2F", "/");
            var company = _companyServices.GetCompany(cnpj);
            if (company == null)
                return NotFound("Algo deu errado na requisição, companhia não encontrada!");
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
