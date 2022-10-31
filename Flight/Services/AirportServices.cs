using ClassLibrary;
using Flights.Utils;
using Nancy.Json;
using System.IO;
using System.Net;
namespace Flights.Services
{
    public class AirportServices
    {
        private readonly IAirportServicesSettings _settings;
        public AirportServices(IAirportServicesSettings settings)
        {
            _settings = settings;
        }
        public Airport GetAirport(string iata)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_settings + "api/Airport/" + iata); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Airport>(message);
        }
    }
}
