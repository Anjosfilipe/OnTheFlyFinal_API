using System.Collections.Generic;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Passengers.Services;

namespace Passangers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerRestrictedController : ControllerBase
    {

        private readonly PassengerRestrictedServices _passengerRestrictedServices;

        public PassengerRestrictedController(PassengerRestrictedServices passengerRestrictedServices)
        {
            _passengerRestrictedServices = passengerRestrictedServices;
        }

        [HttpPost]
        public ActionResult<PassengerRestricted> PostPassengerRestricted([FromQuery] string cpf)
        {
            var passengerRestricted = new PassengerRestricted();
            passengerRestricted.CPF = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);

            _passengerRestrictedServices.CreatePassengerRestricted(passengerRestricted);
            return CreatedAtRoute("GetCpfRestricted", new { CPF = passengerRestricted.CPF.ToString() }, passengerRestricted);
        }

        [HttpGet("{cpf}", Name = "GetCpfRestricted")]
        public ActionResult<Passenger> GetPassengerCpf(string cpf)
        {
            var pass = _passengerRestrictedServices.GetPassengerRestricted(cpf);

            if (pass == null)
            {
                return NotFound();
            }
            return Ok(pass);
        }

        [HttpGet]
        public ActionResult<List<PassengerRestricted>> GetAllPassengersRestricteds() => _passengerRestrictedServices.GetAllPassengersRestricteds();


    }
}