using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Passengers.Services;
using System.Collections.Generic;

namespace Passengers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerGarbageController : ControllerBase
    {
        private readonly PassengerGarbageServices _passengerGarbageServices;
        private readonly PassengerServices _passengerServices;
        public PassengerGarbageController(PassengerServices passengerServices, PassengerGarbageServices passengerGarbageServices)
        {
            _passengerGarbageServices = passengerGarbageServices;
            _passengerServices = passengerServices;
        }
        [HttpPost]
        public ActionResult<PassengerGarbage> PostPassenger(PassengerGarbage passengerGarbage, string cpf)
        {
            Passenger passengerIn = _passengerServices.GetPassenger(cpf);
            Address address1 = _passengerServices.GetAddress(passengerGarbage.Address.ZipCode);
            passengerGarbage.Address = address1;
            passengerGarbage.CPF = passengerIn.CPF;
            passengerGarbage.Name = passengerIn.Name;
            passengerGarbage.Gender = passengerIn.Gender;
            passengerGarbage.Phone = passengerIn.Phone;
            passengerGarbage.DtBirth = passengerIn.DtBirth;
            passengerGarbage.DtRegister = passengerIn.DtRegister;
            passengerGarbage.Status = passengerIn.Status;
            passengerGarbage.Address = passengerIn.Address;
            _passengerGarbageServices.CreatePassengerGarbage(passengerGarbage);
            return CreatedAtRoute("GetPassengerGarbage", new { cpf = passengerIn.CPF.ToString() }, passengerIn);
        }
        [HttpGet("{cpf}", Name = "GetPassengerGarbage")]
        public ActionResult<PassengerGarbage> GetPassenger(string cpf)
        {
            var pass = _passengerGarbageServices.GetPassengerGarbage(cpf);
            {
                if (pass == null)
                {
                    return NotFound();
                }
                return Ok(pass);
            }
        }
        [HttpGet]
        public ActionResult<List<PassengerGarbage>> GetAllPassengersGarbage() => _passengerGarbageServices.GetAllPassengersGarbage();
    }
}
