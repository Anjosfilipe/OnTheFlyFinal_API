using Flights.Utils;

namespace Saleses.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {

        public string FlightCollectionName { get; set; }
     
        public string ConnectionString { get; set; }
      
        public string FlightDataBaseName { get; set; }
     
     
    }
}
