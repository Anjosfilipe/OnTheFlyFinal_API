using ClassLibrary;
using MongoDB.Driver;
using Passangers.Utils;
using System.Collections.Generic;

namespace Passengers.Services
{
    public class PassengerRestrictedServices
    {
        private readonly IMongoCollection<PassengerRestricted> _passengerRestrictedServices;

        public PassengerRestrictedServices(IDataBaseSettings settings)
        {
            var passenger = new MongoClient(settings.ConnectionString);
            var database = passenger.GetDatabase(settings.PassengerDataBaseName);
            _passengerRestrictedServices = database.GetCollection<PassengerRestricted>(settings.PassengerRestrictedCollectionName);
        }

        public PassengerRestricted CreatePassengerRestricted(PassengerRestricted passengerRestricted)
        {
            _passengerRestrictedServices.InsertOne(passengerRestricted);
            return passengerRestricted;
        }
        public List<PassengerRestricted> GetAllPassengersRestricteds() => _passengerRestrictedServices.Find(passengerRestricted => true).ToList();
        public PassengerRestricted GetPassengerRestricted(string cpf) => _passengerRestrictedServices.Find<PassengerRestricted>(passengerRestricted => passengerRestricted.CPF == cpf).FirstOrDefault();
    }
}
