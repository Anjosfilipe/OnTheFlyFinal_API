using ClassLibrary;
using Flights.Utils;
using Nancy.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Flights.Services
{
    public class AircraftServices
    {
        private readonly IAircraftServicesSettings _settings;
        public AircraftServices(IAircraftServicesSettings settings)
        {
            _settings = settings;
        }
        public Aircraft GetAircraft(string rab)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_settings.Address + "api/Aircraft/" + rab); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Aircraft>(message);
        }

        public async Task<bool> PutAircraftFlight(Flight flight)
        {
            DateTime dateIN = flight.Plane.DtLastFlight;
            string datein = dateIN.ToShortDateString().ToString();
            datein = datein.Trim();
            datein = datein.Replace("/", "-");
            string dateYear = datein.Substring(6, 4);
            string dateMounth = datein.Substring(3, 2);
            string dateDay = datein.Substring(0, 2);

            string dateFinal = dateYear + "-" + dateMounth + "-" + dateDay;


            string cnpj = flight.Plane.Company.CNPJ;

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("/", "%2F");

            var rab = flight.Plane.RAB;
            rab= rab.Trim();
            rab = rab.Replace("-", "");
            using (HttpClient _flightClient = new HttpClient())
            {
                string jsonString = new JavaScriptSerializer().Serialize(flight);
                HttpContent http = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _flightClient.PutAsync(_settings.Address + $"api/Aircraft?rab={rab}&dtLastFlight={dateFinal}&capacity={flight.Plane.Capacity}&cnpj={flight.Plane.Company.CNPJ}", null);
           
                if (response.IsSuccessStatusCode) return true;
                return false;
            }
        }
    }
}