using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Admin.Repositories;
using Admin.Repositories.DbContext;
using Admin.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Admin.Api.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        [Fact(DisplayName = "AddUserAsync Should Generate UserId For Insert")]
        public void AddUserAsync_Should_Generate_UserId_For_Insert()
        {
            //Arrange
            var userGuid = Guid.NewGuid();
            var email = "email@test.com";

            var users = Enumerable.Empty<Domain.Model.User>().AsQueryable();

            var adminDbContextMock = GetMockAdminDbContext(users);

            var userRepository = new UserRepository(adminDbContextMock.Object);

            //Act
            var partnerUserId = userRepository.AddUserAsync(userGuid, email).Result;

            //Assert
            Assert.False(partnerUserId == Guid.Empty);

            adminDbContextMock.Verify(
                repo => repo.AddAsync(It.Is<Domain.Model.User>(p => p.UserId == userGuid)
                  , default(CancellationToken)), Times.Once);
        }

        [Fact(DisplayName = "AddUserAsync Should Throw DuplicateKeyException On ConditionalCheckFailedException")]
        public async void AddUserAsync_Should_Throw_DuplicateKeyException_On_ConditionalCheckFailedException()
        {
            //Arrange
            var userGuid = Guid.NewGuid();
            var email = "email@test.com";

            var users = Enumerable.Empty<Domain.Model.User>().AsQueryable();

            var adminDbContextMock = GetMockAdminDbContext(users);

            var userRepository = new UserRepository(adminDbContextMock.Object);

            adminDbContextMock
                .Setup(repo =>
                    repo.SaveChangesAsync(default(CancellationToken)))
                .Throws(new DbUpdateException("", new Exception("duplicate entry")));

            //Act
            //Assert
            await Assert.ThrowsAsync<DuplicateKeyException>(() => userRepository.AddUserAsync(userGuid, email));
        }

        [Fact(DisplayName = "GetUserAsync Should Return Null If Not Found By UserId")]
        public void GetUserAsync_Should_Return_Null_If_Not_Found_By_UserId()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var users = Enumerable.Empty<Domain.Model.User>().AsQueryable();

            var adminDbContextMock = GetMockAdminDbContext(users);

            var userRepository = new UserRepository(adminDbContextMock.Object);

            //Act
            var partnerUser = userRepository.GetUserAsync(partnerUserId).Result;

            //Assert
            Assert.Null(partnerUser);
        }

        private static Mock<AdminDbContext> GetMockAdminDbContext(IQueryable<Domain.Model.User> users)
        {
            var mockSet = new Mock<DbSet<Domain.Model.User>>();

            mockSet.As<IAsyncEnumerable<Domain.Model.User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<Domain.Model.User>(users.GetEnumerator()));

            mockSet.As<IQueryable<Domain.Model.User>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Domain.Model.User>(users.Provider));

            mockSet.As<IQueryable<Domain.Model.User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<Domain.Model.User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<Domain.Model.User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());

            var contextOptions = new DbContextOptions<AdminDbContext>();
            var mockContext = new Mock<AdminDbContext>(contextOptions);
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            return mockContext;
        }
    }
}
