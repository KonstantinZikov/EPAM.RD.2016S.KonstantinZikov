namespace RepositoryInterfaces
{
    public interface IUserRepositoryFactory
    {
        IUserRepository GetRepository();
    }
}
