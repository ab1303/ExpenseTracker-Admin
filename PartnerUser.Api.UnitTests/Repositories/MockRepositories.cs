using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using PartnerUser.Repositories;

namespace PartnerUser.Api.UnitTests.Repositories
{
    public class MockRepositories: IRepositories
    {
        public IPartnerUserRepository PartnerUserRepository { get; }

        public MockRepositories(IPartnerUserRepository partnerUserRepository)
        {
            PartnerUserRepository = partnerUserRepository;
        }

    }
}
