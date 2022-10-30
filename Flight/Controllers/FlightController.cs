using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using Flights.Services;


namespace Flights.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlightServices _flightServices;
    

        public FlightController(FlightServices flightServices)
        { 
            _flightServices = flightServices;
        }

        [HttpGet]
        public ActionResult<List<Flight>> GetAllFlights() => _flightServices.GetAllFlights();

        [HttpPost]
        public ActionResult<Flight> PostFlights(string iata, DateTime date, string rab, double hours, double minutes, string cnpj)
        {
            date = date.AddHours(hours).AddMinutes(minutes);
            iata = iata.ToUpper();
            if (date < DateTime.Now)
            {
                return NotFound("Impossivel criar voo com data retroativa!");
            }
            else
            {

                var destiny = _flightServices.GetAirport(iata);
                if (destiny == null)
                {
                    return NotFound("Destino nao encontrado!");
                }
                else
                {
                    if (rab.Length == 5 || rab.Length == 6)//se for 5 não está formatado se for 6 está formatado
                    {
                        rab = rab.ToLower();
                        rab = rab.Trim();
                        rab = rab.Replace("-", "");
                    }
                    else { return BadRequest("RAB não está de acordo com o tamanho pré estabelecido"); }
                    rab = rab.Substring(0, 2) + "-" + rab.Substring(2, 3);
                    var plane = _flightServices.GetAircraft(rab);

                    if (plane == null)
                    {
                        return NotFound("Impossivel encontar Aeronave!");
                    }
                    else
                    {
                        cnpj = cnpj.Trim();
                        cnpj = cnpj.Replace("/", "").Replace(".", "");
                        cnpj = cnpj.Substring(0, 2).ToString() + "." + cnpj.Substring(2, 3).ToString() + "." + cnpj.Substring(5, 3).ToString() + '/' + cnpj.Substring(8, 4).ToString() + "-" + cnpj.Substring(12, 2).ToString();
                        
                        var restited = _flightServices.GetCompany(cnpj);
                        if (restited.Status == false)
                        {
                            return NotFound("Infelizmente essa Compania não pode cadastrar voos");
                        }
                        else
                        {

                            Flight flight = new Flight() { Status = true, Plane = plane, Destiny = destiny, Departure = date };

                        _flightServices.CreateFlights(flight);
                        return Ok(flight);
                        }
                    }

                }
            }

        }

        [HttpGet("{date}", Name = "GetFlights")]
        public ActionResult<Flight> GetFlights(string iata, DateTime date, double hours, double minutes)
        {
            date = date.AddHours(hours).AddMinutes(minutes); // ver esse horario no banco 
            iata = iata.ToUpper();
            var destiny = _flightServices.GetAirport(iata);
            if (destiny == null)
            {
                return NotFound();
            }
            else
            {
                var flight = _flightServices.GetFlights(destiny.Iata, date);

                if (flight == null)
                {
                    return NotFound();
                }
                return Ok(flight);
            }
        }

        [HttpPut("{date}")]
        public ActionResult<Flight> UpdateFlights(string iata, DateTime date, double hours, double minutes, bool status)
        {
            date = date.AddHours(hours).AddMinutes(minutes);
            iata = iata.ToUpper();
            var destiny = _flightServices.GetAirport(iata);
            if (destiny == null)
            {
                return NotFound();
            }
            else
            {
                var flight = _flightServices.GetFlights(destiny.Iata, date);
                if (flight == null)
                {
                    return NotFound();
                }
                else
                {
                    flight.Status = status;
                    _flightServices.UpdateFlights(flight);
                    return flight;

                }
            }


        }
    }
}
