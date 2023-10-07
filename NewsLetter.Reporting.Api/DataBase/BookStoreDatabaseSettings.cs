namespace NewsLater.Reporting.Api.DataBase
{
    public class BookStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ArticlesCollectionName { get; set; } = null!;
    }
}
