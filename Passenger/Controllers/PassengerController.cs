using System;
using System.Collections.Generic;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
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
        private readonly PassengerRestrictedServices _passengerRestrictedServices;
        private readonly PassengerGarbageServices _passengerGarbageServices;

        public PassengerController(PassengerRestrictedServices passengerRestrictedServices, PassengerServices passengerServices, PassengerGarbageServices passengerGarbageServices, AddressServices addressServices)
        {
            _passengerServices = passengerServices;
            _addressServices = addressServices;
            _passengerGarbageServices = passengerGarbageServices;
            _passengerRestrictedServices = passengerRestrictedServices;
        }
        [HttpGet]
        public ActionResult<List<Passenger>> GetAllPassenger() => _passengerServices.GetAllPassengers();

        [HttpGet("{cpf}", Name = "GetCpf")]
        public ActionResult<Passenger> GetPassengerCpf(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);

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
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);
            //phone = phone.Trim();
            //phone.Replace("(", "").Replace(")", "").Replace("-", "");

            var passengerRestricted = new PassengerRestricted();
            var passengerExists = new Passenger();

            passengerExists = _passengerServices.GetPassenger(cpf);

            if (passengerExists == null)
            {
                if (passengerRestricted == null)
                {
                    var passenger = new Passenger
                    {
                        CPF = cpf,
                        Name = name,
                        Gender = gender,
                        Phone = phone,
                        DtBirth = dtBirth,
                        Status = true,
                        DtRegister = DateTime.Now,
                        Address = _addressServices.GetAddress(zip)
                    };
                    if (PassengerUtil.ValidateCpf(passenger.CPF) == false)
                    {
                        return BadRequest("CPF inválido!");
                    }
                    else
                    {
                        if (phone.Length == 11)
                        {
                            passenger.Phone = passenger.Phone = "(" + phone.Substring(0, 2) + ")" + phone.Substring(2, 5) + "-" + phone.Substring(7, 4);
                        }
                        else if (phone.Length == 10)
                        {
                            passenger.Phone = passenger.Phone = "(" + phone.Substring(0, 2) + ")" + phone.Substring(2, 4) + "-" + phone.Substring(6, 4);
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
                        //_addressServices.Create(passenger.Address); ******************************************
                        _passengerServices.CreatePassenger(passenger);
                        return CreatedAtRoute("GetCpf", new { CPF = passenger.CPF.ToString() }, passenger);
                    }
                }
                else
                {
                    var passenger = new Passenger
                    {
                        CPF = cpf,
                        Name = name,
                        Gender = gender,
                        Phone = phone,
                        DtBirth = dtBirth,
                        Status = false,
                        DtRegister = DateTime.Now,
                        Address = _addressServices.GetAddress(zip)
                    };
                    if (PassengerUtil.ValidateCpf(passenger.CPF) == false)
                    {
                        return BadRequest("CPF inválido!");
                    }
                    else
                    {

                        if (phone.Length == 11)
                        {
                            passenger.Phone = passenger.Phone = "(" + phone.Substring(0, 2) + ")" + phone.Substring(2, 5) + "-" + phone.Substring(7, 4);
                        }
                        else if (phone.Length == 10)
                        {
                            passenger.Phone = passenger.Phone = "(" + phone.Substring(0, 2) + ")" + phone.Substring(2, 4) + "-" + phone.Substring(6, 4);
                        }
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
                        //_addressServices.Create(passenger.Address); ******************************************
                        _passengerServices.CreatePassenger(passenger);
                        return CreatedAtRoute("GetCpf", new { CPF = passenger.CPF.ToString() }, passenger);
                    }
                }
            }
            else
            {
                return BadRequest("Cadastro já existe!");
            }
            //passengerRestricted = _passengerRestrictedServices.GetPassengerRestricted(cpf);
        }
        [HttpPut]
        public ActionResult<Passenger> PutPassenger([FromQuery] string cpf, string name, char gender, string phone, string zip, string street, string district, int number, string compl, string city, string state, bool status)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);
            phone = phone.Trim();
            phone.Replace("(", "").Replace(")", "").Replace("-", "");

            var pass = new Passenger();
            pass = _passengerServices.GetPassenger(cpf);

            if (pass == null)
            {
                return BadRequest("CPF não encontrado!");
            }
            else
            {
                pass.CPF = cpf;
                pass.Name = name;
                pass.Gender = gender;
                pass.Phone = phone;
                pass.Status = status;
                pass.DtBirth = pass.DtBirth;
                pass.DtRegister = pass.DtRegister;
                pass.Address = _addressServices.GetAddress(zip);
                if (pass.Status != true)
                {
                    var passengerRestrictedIn = new PassengerRestricted();
                    passengerRestrictedIn.CPF = pass.CPF;
                    _passengerRestrictedServices.CreatePassengerRestricted(passengerRestrictedIn);
                }
                else
                {
                    var passengerRestrictedIn = new PassengerRestricted();
                    passengerRestrictedIn = _passengerRestrictedServices.GetPassengerRestricted(cpf);
                    if (passengerRestrictedIn != null)
                    {
                        _passengerRestrictedServices.RemovePassengerRestricted(passengerRestrictedIn, cpf);
                    }
                }
            }
            _passengerServices.UpdatePassenger(pass, cpf);
            pass = _passengerServices.GetPassenger(cpf);
            return Ok(pass);
        }
        [HttpDelete]
        public ActionResult DeletePassenger(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);

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
