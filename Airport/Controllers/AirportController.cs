using System.Collections.Generic;
using Airports.Services;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;

namespace Airports.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly AirportServices _airportServices;
        public AirportController(AirportServices airportServices)
        {
            _airportServices = airportServices;
        }

        //[HttpPost]
        //public ActionResult<Airport> Create(Airport airport)
        //{
        //    _airportServices.Create(airport);
        //    return Ok();
        //}

        [HttpGet]
        public ActionResult<List<Airport>> Get() => _airportServices.Get();

        [HttpGet("{iata}", Name = "GetAirportIata")]
        public ActionResult<Airport> Get(string iata)
        {
            var airport = _airportServices.Get(iata);

            if (airport == null)
                return NotFound();

            return airport;
        }
        [HttpGet("/ByCountry/{country_id}", Name = "GetAirportCountry")]
        public ActionResult<List<Airport>> GetByCountry(string country_id)
        {
            var airport = _airportServices.GetByCountry(country_id);

            if (airport == null)
                return NotFound();

            return airport;
        }

        [HttpGet("/ByCity/{city_code}", Name = "GetAirportCity")]
        public ActionResult<List<Airport>> GetByCity(string city_code)
        {
            var airport = _airportServices.GetByCity(city_code);

            if (airport == null)
                return NotFound();

            return airport;
        }

        [HttpGet("/ByIcao/{icao}", Name = "GetAirportIcao")]
        public ActionResult<List<Airport>> GetByIcao(string icao)
        {
            var airport = _airportServices.GetByIcao(icao);

            if (airport == null)
                return NotFound();

            return airport;
        }
    }
}
