using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using Nancy.Json;
using Saleses.Utils;

namespace Saleses.Services
{
    public class FlightServices
    {

        private readonly IFlightServicesSettings _settings;
        public FlightServices(IFlightServicesSettings settings)
        {
            _settings = settings;
        }

        public Flight GetFlight(string iata, DateTime date, double hours, double minutes)
        {
            string datein = date.ToString();
            datein = datein.Trim();
            datein = datein.Replace("/", "-");
            string dateYear = datein.Substring(6, 4);
            string dateMounth = datein.Substring(3, 2);
            string dateDay = datein.Substring(0, 2);

            string dateFinal = dateYear + "-" + dateMounth + "-" + dateDay;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_settings.Address+$"api/Flight/{dateFinal}?iata={iata}&hours={hours}&minutes={minutes}"); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Flight>(message);
        }

        public async Task<bool> PutflightNew(Flight flight, DateTime date, double hours, double minutes)
        {
            using (HttpClient _airCraftClient = new HttpClient())
            {

                string datein = date.ToString();
                datein = datein.Trim();
                datein = datein.Replace("/", "-");
                string dateYear = datein.Substring(6, 4);
                string dateMounth = datein.Substring(3, 2);
                string dateDay = datein.Substring(0, 2);

                string dateFinal = dateYear + "-" + dateMounth + "-" + dateDay;


                string jsonString = new JavaScriptSerializer().Serialize(flight);
                HttpContent http = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _airCraftClient.PutAsync(_settings.Address+$"api/Flight?iata=glu&date={dateFinal}&hours={hours}&minutes={minutes}&status={flight.Status}&sales={flight.Sales}", null);

                if (response.IsSuccessStatusCode) return true;
                return false;
            }
        }
    }
}
