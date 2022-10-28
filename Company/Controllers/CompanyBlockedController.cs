using ClassLibrary;
using Companys.Services;
using Companys.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Companys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyBlockedController : ControllerBase
    {
        readonly CompanyUtils companyUtils = new();
        private readonly CompanyBlockedServices _companyBlockedServices;
        private readonly CompanyServices _companyServices;

        public CompanyBlockedController(CompanyBlockedServices companyBlockedServices, CompanyServices companyServices)
        {
            _companyBlockedServices = companyBlockedServices;
            _companyServices = companyServices;
        }

        [HttpPost("{cnpj:length(18)}")]
        public ActionResult<CompanyBlocked> PostCompanyBlocked(string cnpj)
        {
            Company comp = new();
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", ".").Replace("-", "-").Replace("%", "/").Replace("F", "");

            if (companyUtils.IsCnpjValid(comp.CNPJ) == false)
            {
                return BadRequest("CNPJ inválido!");

            }
            var company = _companyServices.GetCompany(cnpj);
            if (company == null) return NotFound("Companhia não encontrada!!");

            CompanyBlocked companyBlocked = new() { CNPJ = company.CNPJ, Name = company.Name, NameOpt = company.NameOpt, DtOpen = company.DtOpen, Status = company.Status };
            _companyBlockedServices.CreateCompanyBlocked(companyBlocked);
            company.Status = false;
            _companyServices.UpdateCompany(company, cnpj);
            return Ok(companyBlocked);
        }

        [HttpGet]
        public ActionResult<List<CompanyBlocked>> GetAllCompanyBlocked() => _companyBlockedServices.GetAllCompanyBlocked();

        [HttpGet("{cnpj}", Name = "GetCompanyBlocked")]
        public ActionResult<CompanyBlocked> GetCompanyBlocked(string cnpj)
        {
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("%2F", "/");

            var companyBlocked = _companyBlockedServices.GetCompanyBlocked(cnpj);

            if (companyBlocked == null)
                return NotFound("Algo deu errado na requisição, companhia não encontrada!");

            return Ok(companyBlocked);
        }
        //[HttpDelete]
        //public ActionResult<CompanyBlocked> DeleteCompanyBlocked(string cnpj)
        //{
        //    cnpj = cnpj.Trim();
        //    cnpj = cnpj.Replace("%2F", "/");

        //    var company = _companyServices.GetCompany(cnpj);
        //    if (company == null)
        //        return NotFound("Algo deu errado na requisição, companhia não encontrada!");

        //    CompanyBlocked companyBlocked = new()
        //    {
        //        CNPJ = company.CNPJ,
        //        Name = company.Name,
        //        NameOpt = company.NameOpt,
        //        DtOpen = company.DtOpen,
        //        Status = company.Status,
        //        Address = company.Address
        //    };

        //    _companyBlockedServices.CreateCompanyBlocked(companyBlocked);

        //    _companyServices.RemoveCompany(company);
        //    return NoContent();
        //}
    }
}
