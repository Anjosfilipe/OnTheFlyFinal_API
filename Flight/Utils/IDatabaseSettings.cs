namespace Flights.Utils
{
    public interface IDataBaseSettings
    {
        string FlightCollectionName { get; set; }
        string ConnectionString { get; set; }
        string FlightDataBaseName { get; set; }
       
    }
}
