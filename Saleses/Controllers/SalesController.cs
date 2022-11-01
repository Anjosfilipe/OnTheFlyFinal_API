using System;
using System.Collections.Generic;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Saleses.Services;

namespace Saleses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly SalesServices _salesServices;
        private readonly PassengerServices _passengerServices;
        private readonly FlightServices _flightServices;

        public SalesController(SalesServices salesServices, PassengerServices passengerServices, FlightServices flightServices)
        {
            _salesServices = salesServices;
            _passengerServices = passengerServices;
            _flightServices = flightServices;
        }

        [HttpGet]
        public ActionResult<List<Sales>> GetAllSales() => _salesServices.GetAllSales();


        [HttpGet("{cpf}", Name = "GetSales")]
        public ActionResult<Sales> GetSales(string cpf)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            cpf = cpf.Substring(0, 3) + "." + cpf.Substring(3, 3) + "." + cpf.Substring(6, 3) + "-" + cpf.Substring(9, 2);

            var sales = _salesServices.GetSales(cpf);
            if (sales == null)
            {
                return NotFound();
            }
            return Ok(sales);
        }


        [HttpPost]
        public ActionResult<Sales> PostSales(string cpfs, string rab, string iata, DateTime dateflight, double hours, double minutes, bool sold, bool reserverd)
        {
            string[] listcpf = cpfs.Split(',');
            List<Passenger> passagensAtribute = new List<Passenger>();

            if (rab.Length == 5 || rab.Length == 6)//se for 5 não está formatado se for 6 está formatado
            {
                rab = rab.ToLower();
                rab = rab.Trim();
                rab = rab.Replace("-", "");
            }
            else { return BadRequest("Dados de Rab inconsistente"); }

            for (int i = 0; i < listcpf.Length; i++)
            {
                string cpfperson = listcpf[i];
                cpfperson = cpfperson.Substring(0, 3) + "." + cpfperson.Substring(3, 3) + "." + cpfperson.Substring(6, 3) + "-" + cpfperson.Substring(9, 2);
                var passenger = _passengerServices.GetPassenger(cpfperson);
                if (passenger == null || passenger.Status == false)
                {
                    return BadRequest($"Não podemos realizar a venda para este Cpf {listcpf[i]} !");
                }
                int age = DateTime.Now.Year - passenger.DtBirth.Year;
                if (i == 0 && age < 18)
                    return BadRequest("Precisa ser Maior de 18 Anos para Comprar a Passagem!");
                else
                {
                    passagensAtribute.Add(passenger);
                }
                
            }
           
            iata = iata.ToLower();
            var flight = _flightServices.GetFlight(iata, dateflight, hours, minutes);
            if (flight == null)
            {
                return BadRequest("Voo não localizado!");
            }
            else
            {
                if (sold == false && reserverd == true)
                {
                    if((flight.Sales + listcpf.Length) > flight.Plane.Capacity)
                    {
                        return BadRequest("Quantidade de vendas excedidas!");
                    }
                    flight.Sales = flight.Sales + listcpf.Length;
                  
                    Sales sales = new() { Passagers = passagensAtribute };
                    sales.Flight = flight;
                    sales.Reserved = true;
                    sales.Sold = false;

                   _= _flightServices.PutflightNew(flight, dateflight, hours, minutes);

                    return Ok(_salesServices.CreateSales(sales));
                }
                else if (sold == true && reserverd == false)
                { 
                    if ((flight.Sales + listcpf.Length) > flight.Plane.Capacity)
                    {
                        return BadRequest("Quantidade de vendas excedidas!");
                    }
                    flight.Sales = flight.Sales + listcpf.Length;
                    
                    Sales sales = new() { Passagers = passagensAtribute };
                    sales.Flight = flight;
                    sales.Reserved = false;
                    sales.Sold = true;
                    _= _flightServices.PutflightNew(flight, dateflight, hours, minutes);
                    return Ok(_salesServices.CreateSales(sales));
                }
                else
                {
                    return BadRequest("Inconsistencia de dados de venda!");
                }
            }
        }
    }
}
