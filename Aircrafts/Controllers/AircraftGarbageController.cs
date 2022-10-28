using System.Collections.Generic;
using Aircrafts.Services;
using Aircrafts.Utils;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;

namespace Aircrafts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AircraftGarbageController : ControllerBase
    {
        ValidationAircraft validation = new ValidationAircraft();
        private readonly AircraftGarbageServices _aircraftGarbageServices;
        private readonly AircraftServices _aircraftServices;
        public AircraftGarbageController(AircraftGarbageServices aircraftGarbageServices, AircraftServices aircraftServices)
        {
            _aircraftGarbageServices = aircraftGarbageServices;
            _aircraftServices = aircraftServices;
        }
        [HttpPost]
        public ActionResult<AircraftGarbage> PostAircraft(string rab)
        {
            rab = rab.ToLower();
            var registration = validation.RabValidation(rab);
            if (registration != rab)
                return BadRequest("Aeronave não está de acordo com as normas");
            rab = registration.Substring(0, 2) + "-" + registration.Substring(2, 3);
            var plane = _aircraftServices.GetAircraft(rab);
            if (plane == null) return NotFound("Aeronave não encontrada!");
            //aqui tem que inserir formatado
            //pergunta -> como fazer uma verificação do tipo -> tá formatada? se sim, busca, se não formata e busca
            Aircraft aircraftIn = plane;
            AircraftGarbage aircraftGarbage = new AircraftGarbage()
            {
                RAB = aircraftIn.RAB,
                Capacity = aircraftIn.Capacity,
                DtRegistry = aircraftIn.DtRegistry,
                DtLastFlight = aircraftIn.DtLastFlight,
                Company = aircraftIn.Company
            };
            _aircraftGarbageServices.CreateAircraftGarbage(aircraftGarbage);
            _aircraftServices.RemoveAircraft(aircraftIn);
            return CreatedAtRoute("GetAircraftGarbage", new { rab = aircraftIn.RAB.ToString() }, aircraftIn);
        }
        [HttpGet]
        public ActionResult<List<AircraftGarbage>> GetAllAircraft() => _aircraftGarbageServices.GetAllAircraftGarbage();
        [HttpGet("{rab:length(6)}", Name = "GetAircraftGarbage")]
        public ActionResult<Aircraft> GetAircraftGarbage(string rab)
        {
            var plane = _aircraftGarbageServices.GetAircraftGarbage(rab);
            if (plane == null) return NotFound();
            return Ok(plane);
        }
    }
}
