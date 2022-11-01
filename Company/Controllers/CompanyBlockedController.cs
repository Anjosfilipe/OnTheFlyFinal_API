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
        private readonly CompanyBlockedGarbageServices _companyBlockedGarbageServices;
        public CompanyBlockedController(CompanyBlockedServices companyBlockedServices, CompanyServices companyServices, CompanyBlockedGarbageServices companyBlockedGarbageServices)
        {
            _companyBlockedServices = companyBlockedServices;
            _companyServices = companyServices;
            _companyBlockedGarbageServices = companyBlockedGarbageServices;
        }
        [HttpPost]
        public ActionResult<CompanyBlocked> PostCompanyBlocked(string cnpj)
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
            var comp = _companyBlockedServices.GetCompanyBlocked(cnpj);
            if (comp != null) return BadRequest("Companhia já cadastrada com esse CNPJ!");
            var company = _companyServices.GetCompany(cnpj);
            if (company == null) return NotFound("Companhia não encontrada!!");
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
            company.Status = false;
            _companyServices.UpdateCompany(company, cnpj);
            return Ok(companyBlocked);
        }
        [HttpGet]
        public ActionResult<List<CompanyBlocked>> GetAllCompanyBlocked() => _companyBlockedServices.GetAllCompanyBlocked();
        [HttpGet("{cnpj}", Name = "GetCompanyBlocked")]
        public ActionResult<CompanyBlocked> GetCompanyBlocked(string cnpj)
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
            var companyBlocked = _companyBlockedServices.GetCompanyBlocked(cnpj);
            if (companyBlocked == null)
                return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            return Ok(companyBlocked);
        }
        [HttpDelete]
        public ActionResult<CompanyBlocked> DeleteCompanyBlocked(string cnpj)
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
            var companyBlocked = _companyBlockedServices.GetCompanyBlocked(cnpj);
            if (companyBlocked == null)
                return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            var company = _companyServices.GetCompany(cnpj);
            if (company == null)
                return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            company.Status = true;
            _companyServices.UpdateCompany(company, cnpj);
            CompanyBlockedGarbage companyBlockedGarbage = new()
            {
                CNPJ = companyBlocked.CNPJ,
                Name = companyBlocked.Name,
                NameOpt = companyBlocked.NameOpt,
                DtOpen = companyBlocked.DtOpen,
                Status = companyBlocked.Status,
                Address = companyBlocked.Address
            };
            _companyBlockedGarbageServices.CreateCompanyBlockedGarbage(companyBlockedGarbage);
            _companyBlockedServices.RemoveCompanyBlocked(companyBlocked);
            return NoContent();
        }
    }
}
