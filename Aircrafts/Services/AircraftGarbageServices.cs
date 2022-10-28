using Aircrafts.Utils;
using ClassLibrary;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Aircrafts.Services
{
    public class AircraftGarbageServices
    {
        private readonly IMongoCollection<AircraftGarbage> _aircraftGarbage;

        public AircraftGarbageServices(IDataBaseSettings settings)
        {
            var aircraft = new MongoClient(settings.ConnectionString);
            var database = aircraft.GetDatabase(settings.AircraftDatabaseName);
            _aircraftGarbage = database.GetCollection<AircraftGarbage>(settings.AircraftGarbageCollectionName);
        }

        public AircraftGarbage CreateAircraftGarbage(AircraftGarbage aircraftgarbage)
        {
            _aircraftGarbage.InsertOne(aircraftgarbage);
            return aircraftgarbage;
        }
        public List<AircraftGarbage> GetAllAircraftGarbage() => _aircraftGarbage.Find<AircraftGarbage>(aircraftGarbage => true).ToList();
        public AircraftGarbage GetAircraftGarbage(string rab) => _aircraftGarbage.Find<AircraftGarbage>(aircraftGarbage => aircraftGarbage.RAB == rab).FirstOrDefault();
    }
}