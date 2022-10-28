namespace Addresses.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string AddressCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string PassengerDataBaseName { get; set; }

    }
}