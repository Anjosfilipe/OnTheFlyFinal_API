using System.Collections.Generic;
using System.IO;
using System.Net;
using MongoDB.Driver;
using Addresses.Utils;
using ClassLibrary;
using Newtonsoft.Json;
namespace Addresses.Services
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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://viacep.com.br/ws/" + cep + "/json/"); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return JsonConvert.DeserializeObject<Address>(message);
        }
    }
}