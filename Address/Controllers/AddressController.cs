using Addresses.Services;
using Microsoft.AspNetCore.Mvc;

namespace Addresses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AddressController : ControllerBase
    {
        private readonly AddressServices _addressServices;

        public AddressController(AddressServices addressServices)
        {
            _addressServices = addressServices;
        }

        [HttpGet("{zipCode}")]
        public ActionResult<string> GetAddress(string zipCode)
        {
            var address = _addressServices.GetAddress(zipCode);

            if (address == null) return NotFound();

            return Ok(address);
        }
    }
}