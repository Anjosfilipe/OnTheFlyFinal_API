using ClassLibrary;
using Companys.Services;
using Companys.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


namespace Companys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyGarbageController : ControllerBase
    {
        readonly CompanyUtils companyUtils = new();
        private readonly CompanyGarbageServices _companyGarbageServices;
        private readonly CompanyServices _companyServices;
        public CompanyGarbageController(CompanyGarbageServices companyGarbageServices, CompanyServices companyServices)
        {
            _companyGarbageServices = companyGarbageServices;
            _companyServices = companyServices;
        }
        [HttpPost]
        public ActionResult<CompanyGarbage> PostCompanyGarbage(string cnpj)
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
            Company companyIn = _companyServices.GetCompany(cnpj);
            if (companyIn == null)
                return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            CompanyGarbage companyGarbage = new()
            {
                CNPJ = companyIn.CNPJ,
                Name = companyIn.Name,
                NameOpt = companyIn.NameOpt,
                DtOpen = companyIn.DtOpen,
                Status = companyIn.Status,
                Address = companyIn.Address
            };
            _companyGarbageServices.CreateCompanyGarbage(companyGarbage);
            _companyServices.RemoveCompany(companyIn);
            return NoContent();
        }
        [HttpGet]
        public ActionResult<List<CompanyGarbage>> GetAllCompanyGarbage() => _companyGarbageServices.GetAllCompanyGarbage();
        [HttpGet("{cnpj}", Name = "GetCompanyGarbage")]
        public ActionResult<Company> GetCompanyGarbage(string cnpj)
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
            var company = _companyGarbageServices.GetCompanyGarbage(cnpj);
            if (company == null)
                return NotFound("Algo deu errado na requisição, companhia não encontrada!");
            return Ok(company);
        }
    }
}
