namespace Addresses.Utils
{
    public interface IDataBaseSettings
    {
     
        string AddressCollectionName { get; set; }
        string ConnectionString { get; set; }
        string PassengerDataBaseName { get; set; }

    }
}
