using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public Sales GetSales(string cpf, DateTime date, string rab) => _sales.Find<Sales>(sales => sales.Flight.Plane.RAB == rab && sales.Flight.Departure == date && sales.Passagers.Exists(passenger => passenger.CPF.Contains(cpf))).FirstOrDefault();

        public void UpdateSales(string cpf, Sales salesIN)
        {
            _sales.ReplaceOne(sales => sales.Passagers.Any(passager => passager.CPF == cpf), salesIN);
        }

        public Passenger GetPassenger(string cpf)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44302/api/Passenger/" + cpf); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Passenger>(message);
        }
        public Flight GetFlight(string iata, DateTime date, double hours, double minutes)
        {
            string datein = date.ToString();
            datein = datein.Trim();
            datein = datein.Replace("/", "-");
            string dateYear = datein.Substring(6, 4);
            string dateMounth = datein.Substring(3, 2);
            string dateDay = datein.Substring(0, 2);

            string dateFinal = dateYear + "-" + dateMounth + "-" + dateDay;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://localhost:44353/api/Flight/{dateFinal}?iata={iata}&hours={hours}&minutes={minutes}"); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Flight>(message);
        }

        public Flight PutFlight(Flight flight)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44353/api/Flight/" + flight); //url
            request.AllowAutoRedirect = false;
            HttpWebResponse verificaServidor = (HttpWebResponse)request.GetResponse();
            Stream stream = verificaServidor.GetResponseStream();
            if (stream == null) return null;
            StreamReader answerReader = new StreamReader(stream);
            string message = answerReader.ReadToEnd();
            return new JavaScriptSerializer().Deserialize<Flight>(message);
        }
        public async Task<Flight> PutFlightAsync(Flight flight)
        {
            using (HttpClient _adressClient = new())
            {
                string fligthPost = JsonConvert.SerializeObject(flight);
                HttpResponseMessage response = await _adressClient.GetAsync("https://localhost:44353/api/Flight/" + flight);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Flight>(json);
            }
        }
    }
}

