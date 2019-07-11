using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;
using PartnerUser.Repositories;
using PartnerUser.Repositories.DbContext;
using PartnerUser.Repositories.Exceptions;
using Xunit;

namespace PartnerUser.Api.UnitTests.Repositories
{
    public class PartnerUserRepositoryTests
    {
        [Fact(DisplayName = "AddPartnerUserAsync Should Generate PartnerUserId For Insert")]
        public void AddPartnerUserAsync_Should_Generate_PartnerUserId_For_Insert()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            var partnerAppId = Guid.NewGuid();

            var partnerUsers = Enumerable.Empty<Domain.Model.PartnerUser>().AsQueryable();

            var partnerUserDbContextMock = GetMockPartnerUserDbContext(partnerUsers);

            var partnerUserRepository = new PartnerUserRepository(partnerUserDbContextMock.Object);

            //Act
            var partnerUserId = partnerUserRepository.AddPartnerUserAsync(ofxUserGuid, partnerAppId).Result;

            //Assert
            Assert.False(partnerUserId == Guid.Empty);

            partnerUserDbContextMock.Verify(
                repo => repo.AddAsync(It.Is<Domain.Model.PartnerUser>(p =>
                  p.OfxUserGuid == ofxUserGuid && p.PartnerAppId == partnerAppId && p.PartnerUserId == partnerUserId)
                  , default(CancellationToken)), Times.Once);
        }

        [Fact(DisplayName = "AddPartnerUserAsync Should Throw DuplicateKeyException On ConditionalCheckFailedException")]
        public async void AddPartnerUserAsync_Should_Throw_DuplicateKeyException_On_ConditionalCheckFailedException()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            var partnerAppId = Guid.NewGuid();

            var partnerUsers = Enumerable.Empty<Domain.Model.PartnerUser>().AsQueryable();

            var partnerUserDbContextMock = GetMockPartnerUserDbContext(partnerUsers);

            var partnerUserRepository = new PartnerUserRepository(partnerUserDbContextMock.Object);

            partnerUserDbContextMock
                .Setup(repo =>
                    repo.SaveChangesAsync(default(CancellationToken)))
                .Throws(new DbUpdateException("", new Exception("duplicate entry")));

            //Act
            //Assert
            await Assert.ThrowsAsync<DuplicateKeyException>(() => partnerUserRepository.AddPartnerUserAsync(ofxUserGuid, partnerAppId));
        }

        [Fact(DisplayName = "GetPartnerUserAsync Should Return Null If Not Found By PartnerUserId")]
        public void GetPartnerUserAsync_Should_Return_Null_If_Not_Found_By_PartnerUserId()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var partnerUsers = Enumerable.Empty<Domain.Model.PartnerUser>().AsQueryable();

            var partnerUserDbContextMock = GetMockPartnerUserDbContext(partnerUsers);

            var partnerUserRepository = new PartnerUserRepository(partnerUserDbContextMock.Object);

            //Act
            var partnerUser = partnerUserRepository.GetPartnerUserAsync(partnerUserId).Result;

            //Assert
            Assert.Null(partnerUser);
        }

        /*


[Fact(DisplayName = "GetPartnerUserList Returns List of PartnerUserResources When Associated UserId And PartnerAppId Exists")]
public async void GetPartnerUserList_Returns_List_of_PartnerUserResources_When_Associated_UserId_And_PartnerAppId_Exists()
{
    //Arrange

    var ofxUserGuid = Guid.Parse("ae5e939c-1639-49b2-be1f-ac2d8f588a8f");
    var partnerAppId = Guid.Parse("6cc17482-218a-4bdf-94df-83cf4fda7888");
    var partnerUserId = Guid.Parse("9cc17482-218a-4bdf-94df-83cf4fda7888");

    _dynamoDbManagerMock.Setup(dbMgr => dbMgr.GetByKey(It.IsAny<Dictionary<string, DynamoDBEntry>>()))
        .Returns(new Domain.Model.PartnerUser
        {
            OfxUserGuid = ofxUserGuid,
            PartnerAppId = partnerAppId,
            PartnerUserId = partnerUserId,
            BeneficiaryId = It.IsAny<Guid>(),
            CreatedDate = It.IsAny<DateTime>()
        });

    //Act
    var query = new PartnerUserListQuery(
        (ofxUserGuid, partnerAppId, null, null)
    );
    var partnerUserList = await _partnerUserQueryService.Execute(query);

    //Assert
    Assert.True(((List<Domain.Model.PartnerUser>)partnerUserList.Model).Count > 0);
}

[Fact(DisplayName = "GetPartnerUserList Returns Empty List When No PartnerUsers Found")]
public async void GetPartnerUserList_Returns_Empty_List_When_No_PartnerUsers_Found()
{
    //Arrange
    var ofxUserGuid = Guid.NewGuid();
    var partnerAppId = Guid.NewGuid();

    //Act
    var query = new PartnerUserListQuery(
        (ofxUserGuid, partnerAppId, null, null)
    );
    var partnerUserList = await _partnerUserQueryService.Execute(query);

    //Assert
    Assert.Empty(partnerUserList.Model);
}
*/

        private static Mock<PartnerUserDbContext> GetMockPartnerUserDbContext(IQueryable<Domain.Model.PartnerUser> partnerUsers)
        {
            var mockSet = new Mock<DbSet<Domain.Model.PartnerUser>>();

            mockSet.As<IAsyncEnumerable<Domain.Model.PartnerUser>>()
                .Setup(m => m.GetEnumerator())
                .Returns(new TestAsyncEnumerator<Domain.Model.PartnerUser>(partnerUsers.GetEnumerator()));

            mockSet.As<IQueryable<Domain.Model.PartnerUser>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Domain.Model.PartnerUser>(partnerUsers.Provider));

            mockSet.As<IQueryable<Domain.Model.PartnerUser>>().Setup(m => m.Expression).Returns(partnerUsers.Expression);
            mockSet.As<IQueryable<Domain.Model.PartnerUser>>().Setup(m => m.ElementType).Returns(partnerUsers.ElementType);
            mockSet.As<IQueryable<Domain.Model.PartnerUser>>().Setup(m => m.GetEnumerator()).Returns(() => partnerUsers.GetEnumerator());

            var contextOptions = new DbContextOptions<PartnerUserDbContext>();
            var mockContext = new Mock<PartnerUserDbContext>(contextOptions);
            mockContext.Setup(c => c.PartnerUsers).Returns(mockSet.Object);

            return mockContext;
        }
    }
}
