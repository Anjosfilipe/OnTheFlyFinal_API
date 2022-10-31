using ClassLibrary;
using Flights.Utils;
using Nancy.Json;
using System.IO;
using System.Net;
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
    }
}