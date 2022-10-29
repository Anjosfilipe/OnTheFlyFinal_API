using ClassLibrary;
using Companys.Utils;
using MongoDB.Driver;
using Nancy.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Companys.Services
{
    public class CompanyServices
    {
        private readonly IMongoCollection<Company> _company;
        public CompanyServices(IDataBaseSettings settings)
        {
            var company = new MongoClient(settings.ConnectionString);
            var database = company.GetDatabase(settings.CompanyDatabaseName);
            _company = database.GetCollection<Company>(settings.CompanyCollectionName);
        }
        public Company CreateCompany(Company company)
        {
            _company.InsertOne(company);
            return company;
        }
        public List<Company> GetAllCompany() => _company.Find<Company>(company => true).ToList();
        public Company GetCompany(string cnpj) => _company.Find<Company>(company => company.CNPJ == cnpj).FirstOrDefault();
        public void UpdateCompany(Company companyIn, string cnpj)
        {
            _company.ReplaceOne(company => company.CNPJ == cnpj, companyIn);
        }
        public void RemoveCompany(Company companyIn) => _company.DeleteOne(company => company.CNPJ == companyIn.CNPJ);

        public Address GetAddress(string cep)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44372/api/Address/" + cep); //url
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
