namespace Saleses.Utils
{
    public class DataBaseSettings : IDataBaseSettings
    {

        public string SalesCollectionName { get; set; }
     
        public string ConnectionString { get; set; }
      
        public string SalesDatabaseName { get; set; }
    }
}
