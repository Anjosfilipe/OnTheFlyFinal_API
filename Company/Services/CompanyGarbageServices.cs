using ClassLibrary;
using Companys.Utils;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Companys.Services
{
    public class CompanyGarbageServices
    {
        private readonly IMongoCollection<CompanyGarbage> _companyGarbage;
        public CompanyGarbageServices(IDataBaseSettings settings)
        {
            var companyGarbage = new MongoClient(settings.ConnectionString);
            var database = companyGarbage.GetDatabase(settings.CompanyDatabaseName);
            _companyGarbage = database.GetCollection<CompanyGarbage>(settings.CompanyGarbageCollectionName);
        }
        public CompanyGarbage CreateCompanyGarbage(CompanyGarbage companyGarbage)
        {
            _companyGarbage.InsertOne(companyGarbage);
            return companyGarbage;
        }
        public List<CompanyGarbage> GetAllCompanyGarbage() => _companyGarbage.Find<CompanyGarbage>(companyGarbage => true).ToList();
        public CompanyGarbage GetCompanyGarbage(string cnpj) => _companyGarbage.Find<CompanyGarbage>(companyGarbage => companyGarbage.CNPJ == cnpj).FirstOrDefault();
    }
}
