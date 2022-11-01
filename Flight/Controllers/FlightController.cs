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
        private readonly AircraftServices _aircraftServices;
        private readonly CompanyServices _companyServices;
        private readonly AirportServices _airportServices;


        public FlightController(FlightServices flightServices, AircraftServices aircraftServices, CompanyServices companyServices, AirportServices airportServices)
        {
            _flightServices = flightServices;
            _aircraftServices = aircraftServices;
            _companyServices = companyServices;
            _airportServices = airportServices;
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
                return BadRequest("Impossivel criar voo com data retroativa!");
            }
            else
            {
                var destiny = _airportServices.GetAirport(iata);
                if (destiny == null)
                {
                    return BadRequest("Destino nao encontrado!");
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
                    var plane = _aircraftServices.GetAircraft(rab);
                    if(plane.Company.Status == false) { return BadRequest("Companhia Bloqueada!"); }
                    if (plane == null)
                    {
                        return BadRequest("Impossivel encontar Aeronave!");
                    }
                    else
                    {
                        cnpj = cnpj.Trim();
                        cnpj = cnpj.Replace("%2F", "/");
                        cnpj = cnpj.Replace("/", "").Replace(".", "");
                        cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");
                        cnpj = cnpj.Replace("+", "").Replace("*", "").Replace(",", "").Replace("?", "");
                        cnpj = cnpj.Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "");
                        cnpj = cnpj.Replace("%", "").Replace("¨", "").Replace("&", "").Replace("(", "");
                        cnpj = cnpj.Replace("=", "").Replace("[", "").Replace("]", "").Replace(")", "");
                        cnpj = cnpj.Replace("{", "").Replace("}", "").Replace(":", "").Replace(";", "");
                        cnpj = cnpj.Replace("<", "").Replace(">", "").Replace("ç", "").Replace("Ç", "");
                        cnpj = cnpj[..2].ToString() + "." + cnpj.Substring(2, 3).ToString() + "." + cnpj.Substring(5, 3).ToString() + '/' + cnpj.Substring(8, 4).ToString() + "-" + cnpj.Substring(12, 2).ToString();
                        var company = _companyServices.GetCompany(cnpj);
                        if (company.Status == false)
                        {
                            return BadRequest("Infelizmente essa Companhia não pode cadastrar voos");
                        }
                        else
                        {
                            plane.DtLastFlight = date;
                            Flight flight = new Flight() { Status = true, Plane = plane, Destiny = destiny, Departure = date };
                            _flightServices.CreateFlights(flight);
                            _=_aircraftServices.PutAircraftFlight(flight);
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
            var destiny = _airportServices.GetAirport(iata);
            if (destiny == null)
            {
                return NotFound("Aeroporto não encontrado!");
            }
            var flight = _flightServices.GetFlights(destiny.iata, date);
            if (flight == null)
            {
                return NotFound("Voo não encontrado!");
            }
            return Ok(flight);

        }

        [HttpPut]
        public ActionResult<Flight> UpdateFlights(string iata, DateTime date, double hours, double minutes, bool status, int sales)
        {
            date = date.AddHours(hours).AddMinutes(minutes);
            iata = iata.ToUpper();
            var destiny = _airportServices.GetAirport(iata);
            if (destiny == null)
            {
                return NotFound("Aeroporto não encontrado!");
            }
            var flight = _flightServices.GetFlights(destiny.iata, date);
            if (flight == null)
            {
                return NotFound("Voo não encontrado!");
            }
            flight.Status = status;
            flight.Sales = sales;
            _flightServices.UpdateFlights(flight);
            return Ok(flight);
        }
    }
}
