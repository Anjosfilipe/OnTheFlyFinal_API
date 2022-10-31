using Aircrafts.Utils;
using ClassLibrary;
using Nancy.Json;
using System.IO;
using System.Net;
namespace Aircrafts.Services
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
            cnpj = cnpj.Substring(0, 2).ToString() + "." + cnpj.Substring(2, 3).ToString() + "." + cnpj.Substring(5, 3).ToString() + "/" + cnpj.Substring(8, 4).ToString() + "-" + cnpj.Substring(12, 2).ToString();
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("/", "%2F");
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_settings.Address + "api/Company/" + cnpj); //url
                request.Headers.Add("Accept", "text/plain");
                request.AllowAutoRedirect = false;
                HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
                Stream stream = verificaServidor.GetResponseStream();
                if (stream == null) return null;
                StreamReader answerReader = new StreamReader(stream);
                string message = answerReader.ReadToEnd();
                return new JavaScriptSerializer().Deserialize<Company>(message);
            }
            catch (System.Net.WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.NotFound)
                    return null;
                throw ex;
            }
        }
    }
}
























