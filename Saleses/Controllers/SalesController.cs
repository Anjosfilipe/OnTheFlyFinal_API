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
        private readonly SalesServices _salesservices;

        public SalesController(SalesServices salesServices)
        {
            _salesservices = salesServices;
        }

        [HttpGet]
        public ActionResult<List<Sales>> GetAllSales() => _salesservices.GetAllSales();

        [HttpGet("{cpf}", Name = "GetSales")]
        public ActionResult<Sales> GetSales(string cpf, DateTime date, string rab)
        {

            var sales = _salesservices.GetSales(cpf, date, rab);
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


            for (int i = 0; i < listcpf.Length; i++)
            {
                string cpfperson = listcpf[i];
                cpfperson = cpfperson.Substring(0, 3) + "." + cpfperson.Substring(3, 3) + "." + cpfperson.Substring(6, 3) + "-" + cpfperson.Substring(9, 2);
                var passenger = _salesservices.GetPassenger(cpfperson);
                if (passenger == null)
                {
                    return BadRequest($"Não encotramos esse Cpf{listcpf[i]} em nossos Cadastros de Passageiro!");
                }
                int age = DateTime.Now.Year - passenger.DtBirth.Year;
                if (i == 0 && age < 18)
                    return BadRequest("Precisa ser Maior de 18 Anos para Comprar a Passagem!");
                else
                {
                    passagensAtribute.Add(passenger);
                }
                
            }

            var flight = _salesservices.GetFlight(iata, dateflight, hours, minutes);
            if (flight == null)
            {
                return BadRequest("Voo não localizado!");
            }
            else
            {
                if (sold == false && reserverd == true)
                {
                    //metodo de http clint para alterar o sales de voo = list.cont
                    Sales sales = new() { Passagers = passagensAtribute };
                    sales.Flight = flight;
                    sales.Reserved = true;
                    sales.Sold = false;

                    return Ok(_salesservices.CreateSales(sales));
                }
                else if (sold == true && reserverd == false)
                {
                    //metodo de http clint para alterar o sales de voo 
                    Sales sales = new() { Passagers = passagensAtribute };
                    sales.Flight = flight;
                    sales.Reserved = false;
                    sales.Sold = true;
                    return Ok(_salesservices.CreateSales(sales));
                }
                else
                {
                    return BadRequest("Inconsistencia de dados de venda!");
                }
            }
        }
    }
}
