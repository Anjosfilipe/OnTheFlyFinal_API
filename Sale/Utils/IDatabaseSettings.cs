namespace Saleses.Utils
{
    public interface IDataBaseSettings
    {
     
        string SalesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string SalesDatabaseName { get; set; }
    }
}
