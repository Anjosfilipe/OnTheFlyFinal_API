namespace Aircrafts.Utils
{
    public interface IDataBaseSettings
    {
       
        string AircraftCollectionName { get; set; }
        string AircraftGarbageCollectionName { get; set; }
        string ConnectionString { get; set; }
        string AircraftDatabaseName { get; set; }
       
    }
}
