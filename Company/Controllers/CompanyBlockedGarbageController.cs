using System.Collections.Generic;
using ClassLibrary;
using Companys.Services;
using Companys.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Companys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyBlockedGarbageController : ControllerBase
    {
        readonly CompanyUtils companyUtils = new();
        private readonly CompanyBlockedGarbageServices _companyBlockedGarbageServices;
        private readonly CompanyBlockedServices _companyBlockedServices;
        private readonly CompanyServices _companyServices;
        public CompanyBlockedGarbageController(CompanyBlockedGarbageServices companyBlockedGarbageServices, CompanyBlockedServices companyBlockedServices, CompanyServices companyServices)
        {
            _companyBlockedGarbageServices = companyBlockedGarbageServices;
            _companyBlockedServices = companyBlockedServices;
            _companyServices = companyServices;
        }
        [HttpPost]
        public ActionResult<CompanyBlockedGarbage> PostCompanyBlockedGarbage(string cnpj)
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
            if (companyUtils.IsCnpjValid(cnpj) == false)
            {
                return BadRequest("CNPJ inválido!");
            }
            var comp = _companyBlockedGarbageServices.GetCompanyBlockedGarbage(cnpj);
            if (comp != null) return BadRequest("Companhia já cadastrada com esse CNPJ!");
            CompanyBlocked companyBlockedIn = _companyBlockedServices.GetCompanyBlocked(cnpj);
            if (companyBlockedIn == null) return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            CompanyBlockedGarbage companyBlockedGarbage = new()
            {
                CNPJ = companyBlockedIn.CNPJ,
                Name = companyBlockedIn.Name,
                NameOpt = companyBlockedIn.NameOpt,
                DtOpen = companyBlockedIn.DtOpen,
                Status = companyBlockedIn.Status,
                Address = companyBlockedIn.Address
            };
            _companyBlockedGarbageServices.CreateCompanyBlockedGarbage(companyBlockedGarbage);
            var company = _companyServices.GetCompany(cnpj);
            if (company == null) return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            company.Status = true;
            _companyServices.UpdateCompany(company, cnpj);
            _companyBlockedServices.RemoveCompanyBlocked(companyBlockedIn);
            return NoContent();
        }
        [HttpGet]
        public ActionResult<List<CompanyBlockedGarbage>> GetAllCompanyBlockedGarbage() => _companyBlockedGarbageServices.GetAllCompanyBlockedGarbage();
        [HttpGet("{cnpj}", Name = "GetCompanyBlockedGarbage")]
        public ActionResult<CompanyBlocked> GetCompanyBlockedGarbage(string cnpj)
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
            if (companyUtils.IsCnpjValid(cnpj) == false)
            {
                return BadRequest("CNPJ inválido!");
            }
            var companyBlockedGarbage = _companyBlockedGarbageServices.GetCompanyBlockedGarbage(cnpj);
            if (companyBlockedGarbage == null) return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            return Ok(companyBlockedGarbage);
        }
    }
}
