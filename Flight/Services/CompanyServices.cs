using ClassLibrary;
using Flights.Utils;
using Nancy.Json;
using System.IO;
using System.Net;
namespace Flights.Services
{
    public class CompanyServices
    {
        private readonly ICompanyServicesSettings _settings;
        public CompanyServices(ICompanyServicesSettings settings)
        {
            _settings = settings;
        }
        public Company GetCompany(string cnpj)
        {
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("/", "%2F");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("api/Company/" + cnpj); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Company>(message);
        }
    }
}