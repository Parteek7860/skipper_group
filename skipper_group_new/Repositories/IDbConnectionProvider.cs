namespace university.Repositories
{
    public interface IDbConnectionProvider
    {
        string ConnectionString { get; }
    }
}
