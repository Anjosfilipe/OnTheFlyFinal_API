using System.Collections.Generic;
using ClassLibrary;
using Companys.Utils;
using MongoDB.Driver;

namespace Companys.Services
{
    public class CompanyBlockedGarbageServices
    {
        private readonly IMongoCollection<CompanyBlockedGarbage> _companyBlockedGarbage;
        public CompanyBlockedGarbageServices(IDataBaseSettings settings)
        {
            var companyBlockedGarbage = new MongoClient(settings.ConnectionString);
            var database = companyBlockedGarbage.GetDatabase(settings.CompanyDatabaseName);
            _companyBlockedGarbage = database.GetCollection<CompanyBlockedGarbage>(settings.CompanyBlockedGarbageCollectionName);
        }
        public CompanyBlockedGarbage CreateCompanyBlockedGarbage(CompanyBlockedGarbage companyBlockedGarbage)
        {
            _companyBlockedGarbage.InsertOne(companyBlockedGarbage);
            return companyBlockedGarbage;
        }
        public List<CompanyBlockedGarbage> GetAllCompanyBlockedGarbage() => _companyBlockedGarbage.Find<CompanyBlockedGarbage>(companyBlockedGarbage => true).ToList();
        public CompanyBlockedGarbage GetCompanyBlockedGarbage(string cnpj) => _companyBlockedGarbage.Find<CompanyBlockedGarbage>(companyBlockedGarbage => companyBlockedGarbage.CNPJ == cnpj).FirstOrDefault();
    }
}
