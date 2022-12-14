
namespace Passangers.Utils
{
    public class DatabaseSettings : IDatabaseSettings
    {

        public string PassengerCollectionName { get; set; }
        public string PassengerGarbageCollectionName { get; set; }
        public string PassengerRestrictedCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string PassengerDataBaseName { get; set; }
    }
}

