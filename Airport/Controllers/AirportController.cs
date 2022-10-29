using System.Collections.Generic;
using Airports.Services;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;

namespace Airports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportServices _airportservices;


        public AirportController(AirportServices airportServices)
        {
            _airportservices = airportServices;
        }
        [HttpGet]
        public ActionResult<List<Airport>> GetAllPassenger() => _airportservices.GetAllAirport();

        [HttpGet("{iata}")]
        public ActionResult<Airport> GetFlights(string iata)
        {
            iata = iata.ToUpper();
            var destiny = _airportservices.GetAirports(iata);

            if (destiny == null)
            {
                return NotFound();
            }
            return Ok(destiny);
        }

        [HttpPost]
        public ActionResult<Airport> GetFlights(string iata, string coutry, string state)
        {
            iata = iata.ToUpper();
            Airport airport = new Airport() { Iata = iata, Coutry = coutry, State = state };
            _airportservices.CreateAirport(airport);
            return Ok(airport);
        }
    }
}
