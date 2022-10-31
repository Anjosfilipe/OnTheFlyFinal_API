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
        //private readonly PassengerGarbageServices _passengerGarbageServices;
        private readonly PassengerServices _passengerServices;
        public PassengerRestrictedController(PassengerServices passengerServices, PassengerRestrictedServices passengerRestrictedServices)
        {
            _passengerRestrictedServices = passengerRestrictedServices;
            _passengerServices = passengerServices;
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
        [HttpPost]
        public ActionResult<PassengerRestricted> PostPassengerRestricted([FromQuery] string cpf) //REVER ESTE METODO
        {
            var passenger = new Passenger();
            passenger = _passengerServices.GetPassenger(cpf);
            if (passenger == null)
            {
                _passengerServices.CreatePassenger(passenger);
                return Ok();
            }
            else
            {
                var passengerRestricted = new PassengerRestricted();
                passengerRestricted.CPF = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);
                _passengerRestrictedServices.CreatePassengerRestricted(passengerRestricted);
                return CreatedAtRoute("GetCpfRestricted", new { CPF = passengerRestricted.CPF.ToString() }, passengerRestricted);
            }
        }
        [HttpPut]
        public ActionResult<PassengerRestricted> PutPassenger([FromQuery] string cpf)
        {
            var pass = new PassengerRestricted();
            pass = _passengerRestrictedServices.GetPassengerRestricted(cpf);
            if (pass == null)
            {
                return BadRequest("CPF não encontrado!");
            }
            else
            {
                _passengerRestrictedServices.RemovePassengerRestricted(pass, cpf);
                var passenger = _passengerServices.GetPassenger(cpf);
                passenger.Status = true;
                _passengerServices.UpdatePassenger(passenger, cpf);
            }
            var passengerRestrictedIn = new PassengerRestricted();
            passengerRestrictedIn.CPF = pass.CPF;
            _passengerRestrictedServices.CreatePassengerRestricted(passengerRestrictedIn);
            return Ok(pass);
        }
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
                var pass = new Passenger();
                pass = _passengerServices.GetPassenger(cpf);
                if (pass == null)
                {
                    _passengerRestrictedServices.RemovePassengerRestricted(passenger, cpf);
                }
                else
                {
                    pass.Status = true;
                    _passengerServices.UpdatePassenger(pass, cpf);
                    _passengerRestrictedServices.RemovePassengerRestricted(passenger, cpf);
                }
            }
            return NoContent();
        }
    }
}