using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary;
using MongoDB.Driver;
using Nancy.Json;
using Newtonsoft.Json;
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

