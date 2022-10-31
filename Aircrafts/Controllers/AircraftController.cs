using Aircrafts.Services;
using Aircrafts.Utils;
using ClassLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
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
        public ActionResult<Aircraft> PostAircraft(string rab, string cnpj, int capacity, DateTime dtLastFlight)
        {
            if (rab == null) return BadRequest("RAB está nulo com valor");
            if (cnpj == null) return BadRequest("CNPJ está com valor nulo");
            if (capacity == 0) return BadRequest("Capacidade está com valor 0");
            if (rab.Length == 5 || rab.Length == 6)//se for 5 não está formatado se for 6 está formatado
            {
                rab = rab.ToLower();
                rab = rab.Trim();
                rab = rab.Replace("-", "");
            }
            else { return BadRequest("RAB não está de acordo com o tamanho pré estabelecido"); }
            var dtr = System.DateTime.Now.ToString("dd/MM/yyyy");
            var dtlf = dtLastFlight.ToString("dd/MM/yyyy");
            var dtNull = ("01/01/0001");
            var dtRegistry = DateTime.Parse(dtr);
            dtLastFlight = DateTime.Parse(dtlf);
            if (dtlf != dtNull)
            {
                if (dtLastFlight.CompareTo(dtRegistry) < 0)//data anterior a data atual
                    return BadRequest("Data de último voo é um valor no passado");
                if (dtLastFlight.CompareTo(dtRegistry) > 0)//data é posterior a data atual
                    return BadRequest("Data de último voo é um valor futuro");
                //como verificar se dtLastFlight é null
            }
            Aircraft aircraft = new Aircraft() { Capacity = capacity, RAB = rab, DtLastFlight = dtLastFlight, DtRegistry = dtRegistry };
            if (cnpj.Length == 14 || cnpj.Length == 18)
            {
                cnpj = cnpj.Trim();
                cnpj = cnpj.Replace("-", "");
                cnpj = cnpj.Replace(".", "");
                cnpj = cnpj.Replace("/", "");
            }
            else { return BadRequest("CNPJ não está de acordo com o tamanho pré estabelecido"); }
            Company company = _companyServices.GetCompany(cnpj);
            if (company == null) return BadRequest("Companhia Aérea não encontrada!");
            aircraft.Company = company;
            var registration = validation.RabValidation(aircraft.RAB);//aqui já vê se a aeronave é válida
            if (registration != aircraft.RAB)
                return BadRequest("Aeronave não está de acordo com as normas");
            //formato o rab
            aircraft.RAB = registration.Substring(0, 2) + "-" + registration.Substring(2, 3);
            //busco no banco de dados com rab formatado
            var plane = _aircraftServices.GetAircraft(aircraft.RAB);
            if (plane != null) return NotFound("Aeronave já cadastrada!");
            _aircraftServices.CreateAircraft(aircraft);//crio o objeto aeronave
            return CreatedAtRoute("GetAircraft", new { rab = aircraft.RAB.ToString() }, aircraft);
        }
        [HttpGet("{rab}", Name = "GetAircraft")]
        public ActionResult<Aircraft> GetAircraft(string rab)
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
            //busco no banco de dados com rab formatado
            var plane = _aircraftServices.GetAircraft(rab);
            if (plane == null) return NotFound("Aeronave não encontrada!");
            return Ok(plane);
        }
        [HttpGet]
        public ActionResult<List<Aircraft>> GetAllAircraft() => _aircraftServices.GetAllAircraft();
        [HttpPut]
        public ActionResult<Aircraft> PutAircraft([FromQuery] string rab, DateTime dtLastFlight, int capacity, string cnpj)
        {
            if (rab == null) return BadRequest("RAB está nulo");
            if (cnpj == null) return BadRequest("CNPJ está nulo");
            if (capacity == 0) return BadRequest("Capacidade está com valor 0");
            if (rab.Length == 5 || rab.Length == 6)//se for 5 não está formatado se for 6 está formatado
            {
                rab = rab.ToLower();
                rab = rab.Trim();
                rab = rab.Replace("-", "");
            }
            else { return BadRequest("RAB não está de acordo com o tamanho pré estabelecido"); }

            Aircraft aircraftIn = new Aircraft()
            {
                RAB = rab,
                DtLastFlight = dtLastFlight,
                Capacity = capacity
            };
            //no entanto, pode ser um valor igual ou posterior a data de registro
            if (cnpj.Length == 14 || cnpj.Length == 18)
            {
                cnpj = cnpj.Trim();
                cnpj = cnpj.Replace("-", "");
                cnpj = cnpj.Replace(".", "");
                cnpj = cnpj.Replace("/", "");
            }
            else { return BadRequest("CNPJ não está de acordo com o tamanho pré estabelecido"); }
            Company company = _companyServices.GetCompany(cnpj);
            if (company == null) return BadRequest("Companhia Aérea não encontrada!");
            aircraftIn.Company = company;
            //verifico se rab está de acordo com os padrões estabelcidos
            var registration = validation.RabValidation(aircraftIn.RAB);
            if (registration != aircraftIn.RAB)
                return BadRequest("RAB não está de acordo com as normas");
            aircraftIn.RAB = rab.Substring(0, 2) + "-" + rab.Substring(2, 3);//formato RAB
            //busco aeronave já formatada no banco de dados
            var aircraft = _aircraftServices.GetAircraft(aircraftIn.RAB);
            if (aircraft == null) return NotFound("Aeronave não encontrada!");
            aircraftIn.DtRegistry = aircraft.DtRegistry;
            //validações para data de úlimo voo
            var dtlf = dtLastFlight.ToString("dd/MM/yyyy");
            int dtlfDay = int.Parse(dtLastFlight.ToString("dd"));
            int dtlfMonth = int.Parse(dtLastFlight.ToString("MM"));
            int dtlfYear = int.Parse(dtLastFlight.ToString("yyyy"));
            dtLastFlight = DateTime.Parse(dtlf);
            // var dtRegistry = aircraft.DtRegistry.ToString("dd/MM/yyyy");
            int dtRegistryDay = int.Parse(aircraft.DtRegistry.ToString("dd"));
            int dtRegistryMonth = int.Parse(aircraft.DtRegistry.ToString("MM"));
            int dtRegistryYear = int.Parse(aircraft.DtRegistry.ToString("yyyy"));
            var dtNull = ("01/01/0001");
            if (dtlf != dtNull)
            {
                if (dtlfDay <= dtRegistryDay)
                {

                    if ((dtlfMonth < dtRegistryMonth) && (dtlfYear == dtRegistryYear))
                    {
                        return BadRequest("Data de último voo é um valor anterior a data de registro da aeronave");
                    }
                    if ((dtlfMonth < dtRegistryMonth) && (dtlfYear < dtRegistryYear))
                    {
                        return BadRequest("Data de último voo é um valor anterior a data de registro da aeronave");
                    }
                    if ((dtlfMonth == dtRegistryMonth) && (dtlfYear < dtRegistryYear))
                    {
                        return BadRequest("Data de último voo é um valor anterior a data de registro da aeronave");
                    }
                }

            }
            _aircraftServices.UpdateAircraft(aircraft.RAB, aircraftIn);
            return NoContent();
        }
        [HttpDelete]
        public ActionResult<Aircraft> DeleteAircraft(string rab)
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
            var aircraft = _aircraftServices.GetAircraft(rab);
            if (aircraft == null) return NotFound("Aeronave não encontrada!");
            AircraftGarbage aircraftGarbage = new AircraftGarbage();    //crio novo objeto
                                                                        //populo esse novo objeto,"clone" do objeto que estava fora da lixeira
            aircraftGarbage.RAB = aircraft.RAB;
            aircraftGarbage.Capacity = aircraft.Capacity;
            aircraftGarbage.DtRegistry = aircraft.DtRegistry;
            aircraftGarbage.DtLastFlight = aircraft.DtLastFlight;
            aircraftGarbage.Company = aircraft.Company;
            _aircraftServices.RemoveAircraft(aircraft);
            _aircraftGarbageServices.CreateAircraftGarbage(aircraftGarbage);  //insiro na coleção de "lixeira"
            return NoContent();
        }
    }
}