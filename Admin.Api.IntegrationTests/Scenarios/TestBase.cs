using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Admin.Common;
using Admin.Repositories.DbContext;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace Admin.Api.IntegrationTests.Scenarios
{
    public class TestBase : IAsyncLifetime
    {
        private readonly AdminDbContext _dbContext;
        public TestBase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AdminDbContext>();
            optionsBuilder.UseMySql(StackVariables.UserDbConnString);
            _dbContext = new AdminDbContext(optionsBuilder.Options);
        }

        public Dictionary<Guid, string> UserGuidListForCleanUp { get; set; }

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
            UserGuidListForCleanUp = new Dictionary<Guid, string>();
        }

        public async Task DisposeAsync()
        {
            foreach (var item in UserGuidListForCleanUp)
            {
                var partnerUser = await _dbContext.Users.SingleOrDefaultAsync(pu =>
                    pu.UserId == item.Key && pu.Email == item.Value);

                if (partnerUser != null)
                {
                    _dbContext.Users.Remove(partnerUser);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
