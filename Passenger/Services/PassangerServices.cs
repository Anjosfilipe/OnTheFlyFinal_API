using ClassLibrary;
using MongoDB.Driver;
using Nancy.Json;
using Newtonsoft.Json;
using Passangers.Utils;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Passengers.Services
{
    public class PassengerServices
    {
        private readonly IMongoCollection<Passenger> _passenger;
        public PassengerServices(IDatabaseSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.PassengerDataBaseName);
            _passenger = database.GetCollection<Passenger>(settings.PassengerCollectionName);
        }
        public Passenger CreatePassenger(Passenger passenger)
        {
            _passenger.InsertOne(passenger);
            return passenger;
        }
        public List<Passenger> GetAllPassengers() => _passenger.Find(passenger => true).ToList();
        public Passenger GetPassenger(string cpf) => _passenger.Find<Passenger>(passenger => passenger.CPF == cpf).FirstOrDefault();
        public void UpdatePassenger(Passenger passengerIn, string cpf)
        {
            _passenger.ReplaceOne(passenger => passenger.CPF == cpf, passengerIn);
            GetPassenger(passengerIn.CPF);
        }
        public void RemovePassenger(Passenger passenger, string cpf) => _passenger.DeleteOne(passenger => passenger.CPF == cpf);

    }
}
