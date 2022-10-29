using System.Collections.Generic;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Saleses.Services;

namespace Saleses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SalesServices _salesservices;

        public SalesController(SalesServices salesServices)
        {
            _salesservices = salesServices;
        }

        [HttpGet]
        public ActionResult<List<Sales>> GetAllSales() => _salesservices.GetAllSales();

        [HttpGet("{cpf}", Name = "GetSales")]
        public ActionResult<Sales> GetSales(string cpf)
        {

            var sales = _salesservices.GetSales(cpf);
            if (sales == null)
            {
                return NotFound();
            }
            return Ok(sales);
        }


    }
}
