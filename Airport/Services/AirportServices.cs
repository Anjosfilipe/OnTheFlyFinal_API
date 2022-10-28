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
            var database = airports.GetDatabase(settings.AirportDataBaseName);
            _airport = database.GetCollection<Airport>(settings.AirportCollectionName);
        }

        public Airport GetAirports(string destiny) => _airport.Find<Airport>(airport => airport.IATA == destiny).FirstOrDefault();
    }
}
