namespace Passangers.Utils
{
    public interface IDatabaseSettings
    {
     
        string PassengerCollectionName { get; set; }
        string PassengerGarbageCollectionName { get; set; }
        string PassengerRestrictedCollectionName { get; set; }
        string ConnectionString { get; set; }
        string PassengerDataBaseName { get; set; }
    }
}
