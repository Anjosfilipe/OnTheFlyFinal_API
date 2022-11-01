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
            if (rab == null) return BadRequest("RAB está nulo");
            if (rab.Length == 5 || rab.Length == 6)//se for 5 não está formatado se for 6 está formatado
            {
                //converto para minúsculo e tiro a formatação
                rab = rab.ToLower();
                rab = rab.Trim();
                rab = rab.Replace("-", "");
            }
            else { return BadRequest("RAB não está de acordo com o tamanho pré estabelecido"); }
            //valido se é um registro válido
            var registration = validation.RabValidation(rab);
            if (registration != rab)
                return BadRequest("RAB não está de acordo com as normas");
            //formato
            rab = registration.Substring(0, 2) + "-" + registration.Substring(2, 3);
            //busco rab formatado
            var plane = _aircraftServices.GetAircraft(rab);
            if (plane == null) return NotFound("Aeronave não encontrada!");
            Aircraft aircraftIn = plane;
            AircraftGarbage aircraftGarbage = new AircraftGarbage()
            {
                RAB = aircraftIn.RAB,
                Capacity = aircraftIn.Capacity,
                DtRegistry = aircraftIn.DtRegistry,
                DtLastFlight = aircraftIn.DtLastFlight,
                Company = aircraftIn.Company
            };
            _aircraftGarbageServices.CreateAircraftGarbage(aircraftGarbage);//crio objeto na lixeira
            _aircraftServices.RemoveAircraft(aircraftIn);//removo objeto da não lixeira
            return CreatedAtRoute("GetAircraftGarbage", new { rab = aircraftIn.RAB.ToString() }, aircraftIn);
        }
        [HttpGet]
        public ActionResult<List<AircraftGarbage>> GetAllAircraft() => _aircraftGarbageServices.GetAllAircraftGarbage();
        [HttpGet("{rab}", Name = "GetAircraftGarbage")]
        public ActionResult<Aircraft> GetAircraftGarbage(string rab)
        {
            if (rab == null) return BadRequest("RAB está nulo");
            if (rab.Length == 5 || rab.Length == 6)//se for 5 não está formatado se for 6 está formatado
            {
                rab = rab.ToLower();
                rab = rab.Trim();
                rab = rab.Replace("-", "");
                rab = rab.Substring(0, 2) + "-" + rab.Substring(2, 3);
            }
            else { return BadRequest("RAB não está de acordo com o tamanho pré estabelecido"); }
            //busco rab formatado já que a no banco de dados está formatado
            var plane = _aircraftGarbageServices.GetAircraftGarbage(rab);
            if (plane == null) return NotFound("Aeronave não encontrada!");
            return Ok(plane);
        }
    }
}
