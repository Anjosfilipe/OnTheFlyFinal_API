using System.IO;
using System.Net;
using ClassLibrary;
using Nancy.Json;
using Saleses.Utils;

namespace Saleses.Services
{
    public class PassengerServices
    {
        private readonly IPassengerServicesSettings _settings;
        public PassengerServices(IPassengerServicesSettings settings)
        {
            _settings = settings;
        }

        public Passenger GetPassenger(string cpf)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_settings.Address+"api/Passenger/" + cpf); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Passenger>(message);
        }
    }
}
