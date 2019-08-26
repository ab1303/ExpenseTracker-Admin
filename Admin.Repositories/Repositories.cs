namespace Admin.Repositories
{
    public class Repositories: IRepositories
    {
        public Repositories(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; }
    }
}
