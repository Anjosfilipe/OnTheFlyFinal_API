namespace Aircrafts.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string AircraftCollectionName { get; set; }
        public string AircraftGarbageCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string AircraftDatabaseName { get; set; }
        
    }
}
