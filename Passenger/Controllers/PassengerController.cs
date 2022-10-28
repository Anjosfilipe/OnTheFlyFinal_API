using System;
using System.Collections.Generic;
using Addresses.Services;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Passengers.Services;
using Passengers.Utils;

namespace Passengers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengerController : ControllerBase
    {
        private readonly PassengerServices _passengerServices;
        private readonly AddressServices _addressServices;
        private readonly PassengerGarbageServices _passengerGarbageServices;

        public PassengerController(PassengerServices passengerServices, PassengerGarbageServices passengerGarbageServices, AddressServices addressServices)
        {
            _passengerServices = passengerServices;
            _addressServices = addressServices;
            _passengerGarbageServices = passengerGarbageServices;
        }

        [HttpGet]
        public ActionResult<List<Passenger>> GetAllPassenger() => _passengerServices.GetAllPassengers();


        [HttpGet("{cpf}", Name = "GetCpf")]
        public ActionResult<Passenger> GetPassengerCpf(string cpf)
        {
            var pass = _passengerServices.GetPassenger(cpf);
            if (pass == null)
            {
                return NotFound();
            }
            return Ok(pass);
        }

        [HttpPost]
        public ActionResult<Passenger> PostPassenger(string cpf, string name, char gender, string phone, DateTime dtBirth, string zip, string street, string district, int number, string compl, string city, string state)
        {
            var passenger = new Passenger
            {
                CPF = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2),
                Name = name,
                Gender = gender,
                Phone = "(" + phone.Substring(0, 2) + ")" + phone.Substring(2, 4) + "-" + phone.Substring(6, 4),
                DtBirth = dtBirth,
                Status = true,
                DtRegister = DateTime.Now,
                Address = new AddressServices().GetAddress(zip)
            };

            if (PassengerUtil.ValidateCpf(passenger.CPF) == false)
            {
                return BadRequest("CPF inválido!");
            }
            else if (passenger.CPF == null)
            {
                passenger.Address = new Address
                {
                    ZipCode = zip,
                    Street = street,
                    City = city,
                    Complement = compl,
                    Number = number,
                    District = district,
                    State = state
                };
            }
            else
            {
                passenger.Address.Complement = compl;
                passenger.Address.Number = number;
            }
            _addressServices.Create(passenger.Address);
            _passengerServices.CreatePassenger(passenger);
            return CreatedAtRoute("GetCpf", new { CPF = passenger.CPF.ToString() }, passenger);
        }

        [HttpPut]
        public ActionResult<Passenger> PutPassenger([FromQuery] string cpf, string name, char gender, string phone, string zip, string street, string district, int number, string compl, string city, string state, bool status) //nao esta encontrando o objeto no banco
        {
            var pass = _passengerServices.GetPassenger(cpf);

            if (pass == null)
            {
                return NotFound();
            }
            else
            {
                var passenger = new Passenger
                {
                    CPF = pass.CPF
                };
                if (passenger.CPF == null)
                {
                    return BadRequest("CPF não encontrado!");
                }
                else
                {
                    Passenger passengerIn = new()
                    {
                        CPF = passenger.CPF,
                        Name = name,
                        Gender = gender,
                        Phone = phone,
                        DtBirth = pass.DtBirth,
                        DtRegister = pass.DtRegister,
                        Status = status,
                        Address = new AddressServices().GetAddress(zip)
                    };
                    _passengerServices.UpdatePassenger(passengerIn, cpf);
                    pass = _passengerServices.GetPassenger(cpf);
                }
                return Ok(pass);
            }
        }

        [HttpDelete]
        public ActionResult DeletePassenger(string cpf)
        {
            var passenger = _passengerServices.GetPassenger(cpf);

            if (passenger == null)
            {
                return NotFound("Passageiro não encontrado!");
            }
            else
            {
                PassengerGarbage passengerGarbage = new();

                passengerGarbage.CPF = passenger.CPF;
                passengerGarbage.Name = passenger.Name;
                passengerGarbage.Gender = passenger.Gender;
                passengerGarbage.Phone = passenger.Phone;
                passengerGarbage.DtBirth = passenger.DtBirth;
                passengerGarbage.DtRegister = passenger.DtRegister;
                passengerGarbage.Status = passenger.Status;
                passengerGarbage.Address = passenger.Address;

                _passengerServices.RemovePassenger(passenger, cpf);
                _passengerGarbageServices.CreatePassengerGarbage(passengerGarbage);

            }
            return NoContent();
        }


    }

}
