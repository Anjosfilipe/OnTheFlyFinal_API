namespace Companys.Utils
{
    public interface IDataBaseSettings
    {
        string CompanyCollectionName { get; set; }
        string CompanyGarbageCollectionName { get; set; }
        string CompanyBlockedCollectionName { get; set; }
        string ConnectionString { get; set; }
        string CompanyDatabaseName { get; set; }
    }
}
