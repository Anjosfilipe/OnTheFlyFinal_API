using Airports.Services;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;

namespace Airports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportServices _airportservice;


        public AirportController(AirportServices airportServices)
        {
            _airportservice = airportServices;
        }

        [HttpGet("{iata}")]
        public ActionResult<Airport> GetFlights(string iata)
        {
            iata = iata.ToUpper();
            var destiny = _airportservice.GetAirports(iata);

            if (destiny == null)
            {
                return NotFound();
            }
            return Ok(destiny);
        }
    }
}
