using ClassLibrary;
using MongoDB.Driver;
using Passangers.Utils;
using System.Collections.Generic;

namespace Passengers.Services
{
    public class PassengerGarbageServices
    {
        private readonly IMongoCollection<PassengerGarbage> _passengerGarbageServices;
        public PassengerGarbageServices(IDatabaseSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.PassengerDataBaseName);
            _passengerGarbageServices = database.GetCollection<PassengerGarbage>(settings.PassengerGarbageCollectionName);
        }
        public PassengerGarbage CreatePassengerGarbage(PassengerGarbage passengerGarbage)
        {
            _passengerGarbageServices.InsertOne(passengerGarbage);
            return passengerGarbage;
        }
        public PassengerGarbage GetPassengerGarbage(string cpf) => _passengerGarbageServices.Find<PassengerGarbage>(passengerGarbage => passengerGarbage.CPF == cpf).FirstOrDefault();
        public List<PassengerGarbage> GetAllPassengersGarbage() => _passengerGarbageServices.Find(passengerGarbage => true).ToList();
        public void RemoveGarbagePassenger(PassengerGarbage passengerGarbage, string cpf) => _passengerGarbageServices.DeleteOne(passenger => passenger.CPF == cpf);
    }
}
