using System;
using System.Collections.Generic;
using System.Text;
using Admin.Repositories;
using Moq;

namespace Admin.Api.UnitTests.Repositories
{
    public class MockRepositories: IRepositories
    {
        public IUserRepository UserRepository { get; }

        public MockRepositories(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

    }
}
