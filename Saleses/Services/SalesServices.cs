using System.Collections.Generic;
using System.Linq;
using ClassLibrary;
using MongoDB.Driver;
using Saleses.Utils;

namespace Saleses.Services
{
    public class SalesServices
    {
        private IMongoCollection<Sales> _sales;

        public SalesServices(IDataBaseSettings settings)
        {
            var sales = new MongoClient(settings.ConnectionString);
            var dataBase = sales.GetDatabase(settings.SalesDatabaseName);
            _sales = dataBase.GetCollection<Sales>(settings.SalesCollectionName);

        }

        public Sales CreateSales(Sales sales)
        {
            _sales.InsertOne(sales);
            return sales;
        }

        public List<Sales> GetAllSales() => _sales.Find(sales => true).ToList();
        public Sales GetSales(string cpf) => _sales.Find<Sales>(sales => sales.Passagers.Any(passager => passager.CPF == cpf)).FirstOrDefault();

        public void UpdateSales(string cpf, Sales salesIN)
        {
            _sales.ReplaceOne(sales => sales.Passagers.Any(passager => passager.CPF == cpf), salesIN);
        }

    }
}
