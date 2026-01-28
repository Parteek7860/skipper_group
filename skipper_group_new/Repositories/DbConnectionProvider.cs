namespace university.Repositories
{
    public class DbConnectionProvider: IDbConnectionProvider
    {
        public string ConnectionString { get; }

        public DbConnectionProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }

}
