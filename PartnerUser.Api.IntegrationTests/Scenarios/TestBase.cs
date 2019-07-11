using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PartnerUser.Common;
using PartnerUser.Repositories.DbContext;
using Xunit;

namespace PartnerUser.Api.IntegrationTests.Scenarios
{
    public class TestBase : IAsyncLifetime
    {
        private readonly PartnerUserDbContext _dbContext;
        public TestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PartnerUserDbContext>();
            optionsBuilder.UseMySql(StackVariables.PartnerUserDbConnString);
            _dbContext = new PartnerUserDbContext(optionsBuilder.Options);
        }

        public Dictionary<Guid, Guid> OfxUserGuidPartnerAppIdListForCleanUp { get; set; }

        public async Task<TResponse> DeserializeHttpResponseMessageContentAsync<TResponse>(HttpResponseMessage httpResponseMessage)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var typedResponse = JsonConvert.DeserializeObject<TResponse>(content);
            return typedResponse;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task InitializeAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            OfxUserGuidPartnerAppIdListForCleanUp = new Dictionary<Guid, Guid>();
        }

        public async Task DisposeAsync()
        {
            foreach (var item in OfxUserGuidPartnerAppIdListForCleanUp)
            {
                var partnerUser = await _dbContext.PartnerUsers.SingleOrDefaultAsync(pu =>
                    pu.OfxUserGuid == item.Key && pu.PartnerAppId == item.Value);

                if (partnerUser != null)
                {
                    _dbContext.PartnerUsers.Remove(partnerUser);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
