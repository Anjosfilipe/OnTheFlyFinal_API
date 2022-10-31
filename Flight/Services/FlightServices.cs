using ClassLibrary;
using Flights.Utils;
using MongoDB.Driver;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Flights.Services
{
    public class FlightServices
    {
        private readonly IMongoCollection<Flight> _flight;

        public FlightServices(IDataBaseSettings settings)
        {
            var flights = new MongoClient(settings.ConnectionString);
            var database = flights.GetDatabase(settings.FlightDataBaseName);
            _flight = database.GetCollection<Flight>(settings.FlightCollectionName);
        }
        public Flight CreateFlights(Flight flight)
        {
            _flight.InsertOne(flight);
            return flight;
        }
        public List<Flight> GetAllFlights() => _flight.Find(flights => true).ToList();
        public Flight GetFlights(string iata, DateTime dateTime) => _flight.Find<Flight>(flights => flights.Departure == dateTime && flights.Destiny.Iata == iata).FirstOrDefault();

        public void UpdateFlights(Flight fligthsIn)
        {
            _flight.ReplaceOne(flights => flights.Departure == fligthsIn.Departure && flights.Destiny.Iata == fligthsIn.Destiny.Iata, fligthsIn);
        }

    }
}
