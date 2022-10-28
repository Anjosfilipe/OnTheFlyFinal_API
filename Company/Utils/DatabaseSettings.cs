namespace Companys.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {
        public string AddressCollectionName { get; set; }
        public string CompanyCollectionName { get; set; }
        public string CompanyGarbageCollectionName { get; set; }
        public string CompanyBlockedCollectionName { get; set; }
        public string CompanyBlockedGarbageCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string CompanyDatabaseName { get; set; }


    }
}
