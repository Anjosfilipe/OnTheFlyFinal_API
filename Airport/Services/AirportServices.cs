using System.Collections.Generic;
using Airports.Utils;
using ClassLibrary;
using MongoDB.Driver;

namespace Airports.Services
{
    public class AirportServices
    {

        private readonly IMongoCollection<Airport> _airport;


        public AirportServices(IDataBaseSettings settings)
        {
            var airports = new MongoClient(settings.ConnectionString);
            var database = airports.GetDatabase(settings.AirportDatabaseName);
            _airport = database.GetCollection<Airport>(settings.AirportCollectionName);
        }

        public List<Airport> GetAllAirport() => _airport.Find<Airport>(airport => true).ToList();
        public Airport GetAirports(string destiny) => _airport.Find<Airport>(airport => airport.Iata == destiny).FirstOrDefault();

        public Airport CreateAirport(Airport airport)
        {
            _airport.InsertOne(airport);
            return airport;
        }
    }
}
