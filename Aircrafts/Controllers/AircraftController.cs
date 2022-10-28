using Aircrafts.Services;
using Aircrafts.Utils;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Aircrafts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftController : ControllerBase
    {
        ValidationAircraft validation = new ValidationAircraft();
        private readonly AircraftServices _aircraftServices;
        private readonly AircraftGarbageServices _aircraftGarbageServices;
        private readonly CompanyServices _companyServices;
        public AircraftController(AircraftServices aircraftServices, AircraftGarbageServices aircraftGarbageServices, CompanyServices companyServices)
        {
            _aircraftServices = aircraftServices;
            _aircraftGarbageServices = aircraftGarbageServices;
            _companyServices = companyServices;
        }
        [HttpPost]
        public ActionResult<Aircraft> PostAircraft(string cnpj, int capacity, string registration)
        {
            Aircraft aircraft = new Aircraft() { Capacity = capacity, RAB = registration, DtLastFlight = System.DateTime.Now, DtRegistry = System.DateTime.Now };
            Company company = _companyServices.GetCompany(cnpj);
            aircraft.Company = company;
            aircraft.RAB = aircraft.RAB.ToLower();
            var rab = validation.RabValidation(aircraft.RAB);
            if (rab != aircraft.RAB)
                return BadRequest("Aeronave não está de acordo com as normas");
            aircraft.RAB = rab.Substring(0, 2) + "-" + rab.Substring(2, 3);
            var plane = _aircraftServices.GetAircraft(aircraft.RAB);
            if (plane != null) return NotFound("Aeronave já cadastrada!");
            _aircraftServices.CreateAircraft(aircraft);
            return CreatedAtRoute("GetAircraft", new { rab = aircraft.RAB.ToString() }, aircraft);
        }
        [HttpGet("{rab:length(6)}", Name = "GetAircraft")]
        public ActionResult<Aircraft> GetAircraft(string rab)
        {
            rab = rab.ToLower();
            //rab = rab.Substring(0, 2) + "-" + rab.Substring(2, 3);
            var plane = _aircraftServices.GetAircraft(rab);
            if (plane == null) return NotFound();
            return Ok(plane);
        }
        [HttpGet]
        public ActionResult<List<Aircraft>> GetAllAircraft() => _aircraftServices.GetAllAircraft();
        [HttpPut]
        public ActionResult<Aircraft> PutAircraft([FromQuery] string rab, DateTime dtLastFlight, string cnpj, int capacity)
        {
            Aircraft aircraftIn = new Aircraft()
            {
                RAB = rab,
                DtLastFlight = dtLastFlight,
                Capacity = capacity
            };
            Company company = _companyServices.GetCompany(cnpj);
            aircraftIn.Company = company;
            aircraftIn.RAB = aircraftIn.RAB.ToLower();
            var registration = validation.RabValidation(aircraftIn.RAB);
            if (registration != aircraftIn.RAB)
                return BadRequest("Aeronave não está de acordo com as normas");
            aircraftIn.RAB = rab.Substring(0, 2) + "-" + rab.Substring(2, 3);
            var aircraft = _aircraftServices.GetAircraft(aircraftIn.RAB);
            if (aircraft == null) return NotFound("Não encontrado!");
            aircraftIn.DtRegistry = aircraft.DtRegistry;
            _aircraftServices.UpdateAircraft(aircraft.RAB, aircraftIn);
            return NoContent();
        }
        [HttpDelete]
        public ActionResult<Aircraft> DeleteAircraft(string rab)
        {
            rab = rab.ToLower();
            var aircraftIn = rab.Substring(0, 2) + "-" + rab.Substring(2, 3);
            var aircraft = _aircraftServices.GetAircraft(aircraftIn);
            if (aircraft == null) return NotFound("Aeronave não encontrada");
            AircraftGarbage aircraftGarbage = new AircraftGarbage();    //crio novo objeto
            //populo esse novo objeto
            aircraftGarbage.RAB = aircraft.RAB;
            aircraftGarbage.Capacity = aircraft.Capacity;
            aircraftGarbage.DtRegistry = aircraft.DtRegistry;
            aircraftGarbage.DtLastFlight = aircraft.DtLastFlight;
            aircraftGarbage.Company = aircraft.Company;
            _aircraftServices.RemoveAircraft(aircraft);
            _aircraftGarbageServices.CreateAircraftGarbage(aircraftGarbage);
            //insiro na coleção de "lixeira"
            return NoContent();
        }
    }
}