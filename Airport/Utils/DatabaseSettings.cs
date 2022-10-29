namespace Airports.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string AirportCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string AirportDatabaseName { get; set; }

    }
}
