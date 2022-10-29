using MongoDB.Driver;
using Aircrafts.Utils;
using System.Collections.Generic;
using ClassLibrary;
using System.Net;
using System.IO;
using Nancy.Json;

namespace Aircrafts.Services
{
    public class AircraftServices
    {
        private readonly IMongoCollection<Aircraft> _aircraft;
        public AircraftServices(IDataBaseSettings settings)
        {
            var aircraft = new MongoClient(settings.ConnectionString);
            var database = aircraft.GetDatabase(settings.AircraftDatabaseName);
            _aircraft = database.GetCollection<Aircraft>(settings.AircraftCollectionName);
        }
        public Aircraft CreateAircraft(Aircraft aircraft)
        {
            string test = aircraft.RAB;
            _aircraft.InsertOne(aircraft);
            return aircraft;
        }
        public Aircraft GetAircraft(string rab) => _aircraft.Find<Aircraft>(aircraft => aircraft.RAB == rab).FirstOrDefault();
        public List<Aircraft> GetAllAircraft() => _aircraft.Find<Aircraft>(aircraft => true).ToList();
        public void UpdateAircraft(string rab, Aircraft aircraft)
        {
            string test = aircraft.RAB;
            _aircraft.ReplaceOne(aircraft => aircraft.RAB == rab, aircraft);
        }
        public void RemoveAircraft(Aircraft aircraft)
        {
            string test = aircraft.RAB;
            _aircraft.DeleteOne(aircraft => aircraft.RAB == test);
        }

        public Company GetCompany(string cnpj)
        {
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace("/", "%2F");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44308/api/Company/" + cnpj); //url
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
