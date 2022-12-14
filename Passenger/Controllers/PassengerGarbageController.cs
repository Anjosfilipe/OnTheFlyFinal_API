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
        private readonly AddressServices _addressServices;
        private readonly PassengerServices _passengerServices;
        public PassengerGarbageController(PassengerServices passengerServices, AddressServices address, PassengerGarbageServices passengerGarbageServices)
        {
            _passengerGarbageServices = passengerGarbageServices;
            _addressServices = address;
            _passengerServices = passengerServices;
        }
        [HttpGet("{cpf}", Name = "GetPassengerGarbage")]
        public ActionResult<PassengerGarbage> GetPassenger(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);
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
        [HttpPost]
        public ActionResult<PassengerGarbage> PostPassenger(PassengerGarbage passengerGarbage, string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);

            Passenger passengerIn = _passengerServices.GetPassenger(cpf);
            Address address1 = _addressServices.GetAddress(passengerIn.Address.ZipCode);
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
        [HttpDelete]
        public ActionResult DeletePassenger(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);

            var passenger = _passengerGarbageServices.GetPassengerGarbage(cpf);
            if (passenger == null)
            {
                return NotFound("Passageiro não encontrado!");
            }
            else
            {
                PassengerGarbage passengerGarbage = new();
                passengerGarbage = passenger;
                var passageiro = new Passenger();
                _passengerGarbageServices.RemoveGarbagePassenger(passenger, cpf);
            }
            return NoContent();
        }
    }
}