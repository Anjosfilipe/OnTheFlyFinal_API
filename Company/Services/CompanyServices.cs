using ClassLibrary;
using Companys.Utils;
using MongoDB.Driver;
using System.Collections.Generic;

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
    }
}
