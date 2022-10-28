namespace Companys.Utils
{
    public interface IDataBaseSettings
    {
        string AddressCollectionName { get; set; }
        string CompanyCollectionName { get; set; }
        string CompanyGarbageCollectionName { get; set; }
        string CompanyBlockedCollectionName { get; set; }
        string CompanyBlockedGarbageCollectionName { get; set; }
        string ConnectionString { get; set; }
        string CompanyDatabaseName { get; set; }
    }
}
