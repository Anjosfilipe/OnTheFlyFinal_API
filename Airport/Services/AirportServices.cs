using System.Collections.Generic;
using Airports.Utils;
using ClassLibrary;
using MongoDB.Driver;

namespace Airports.Services
{
    public class AirportServices
    {

        private readonly IMongoCollection<Airport> _airports;

        public AirportServices(IDataBaseSettings settings)
        {
            var airport = new MongoClient(settings.ConnectionString);
            var database = airport.GetDatabase(settings.AirportDatabaseName);
            _airports = database.GetCollection<Airport>(settings.AirportCollectionName);
        }

        //public Airport Create (Airport airport)
        //{
        //    _airports.InsertOne(airport);
        //    return airport;
        //}
        public List<Airport> Get() =>
            _airports.Find(airport => true).ToList();

        public Airport Get(string iata) =>
            _airports.Find<Airport>(airport => airport.iata == iata).FirstOrDefault();

        public List<Airport> GetByIcao(string icao) =>
            _airports.Find<Airport>(airport => airport.icao == icao).ToList();

        public List<Airport> GetByCountry(string country_id) =>
            _airports.Find<Airport>(airport => airport.country_id == country_id).ToList();

        public List<Airport> GetByCity(string city_code) =>
            _airports.Find<Airport>(airport => airport.city_code == city_code).ToList();
    }
}
