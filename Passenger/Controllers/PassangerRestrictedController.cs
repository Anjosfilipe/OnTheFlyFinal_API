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
        private readonly PassengerGarbageServices _passengerGarbageServices;
        private readonly PassengerServices _passengerServices;
        public PassengerRestrictedController(PassengerServices passengerServices, PassengerRestrictedServices passengerRestrictedServices)
        {
            _passengerRestrictedServices = passengerRestrictedServices;
            _passengerServices = passengerServices;
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
        [HttpDelete]
        public ActionResult DeletePassenger(string cpf)
        {
            var passenger = _passengerRestrictedServices.GetPassengerRestricted(cpf);
            if (passenger == null)
            {
                return NotFound("Passageiro não encontrado!");
            }
            else
            {
                PassengerRestricted passengerRestricted = new();
                passengerRestricted.CPF = passenger.CPF;
                var passageiro = new Passenger();
                passageiro = _passengerServices.GetPassenger(cpf);
                passageiro.Status = true;
                _passengerServices.UpdatePassenger(passageiro, cpf);
            }
            return NoContent();
        }
    }
}