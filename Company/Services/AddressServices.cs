using ClassLibrary;
using Companys.Utils;
using Nancy.Json;
using System.IO;
using System.Net;
namespace Companys.Services
{
    public class AddressServices
    {
        private readonly IAddressServicesSettings _settings;
        public AddressServices(IAddressServicesSettings settings)
        {
            _settings = settings;
        }
        public Address GetAddress(string cep)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_settings.Address + "api/Address/" + cep); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Address>(message);
        }
    }
}