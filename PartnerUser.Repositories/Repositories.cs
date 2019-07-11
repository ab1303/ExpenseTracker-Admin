namespace PartnerUser.Repositories
{
    public class Repositories: IRepositories
    {
        public Repositories(IPartnerUserRepository partnerUserRepository)
        {
            PartnerUserRepository = partnerUserRepository;
        }

        public IPartnerUserRepository PartnerUserRepository { get; }
    }
}
