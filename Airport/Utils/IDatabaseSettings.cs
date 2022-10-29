namespace Airports.Utils
{
    public interface IDataBaseSettings
    {
        string AirportCollectionName { get; set; }
        string ConnectionString { get; set; }
        string AirportDatabaseName { get; set; }      
    }
}
