using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PartnerUser.Common;

namespace PartnerUser.Api.IntegrationTests.Setup
{
    public class TestContext
    {
        private TestServer _server;

        public HttpClient Client { get; set; }
        public HttpClient BslClient { get; set; }
        public HttpClient AuthServerClient { get; set; }

        public TestContext()
        {
            SetupEnvironmentVariables();

            var runtimeEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            SetupClient(runtimeEnvironment);
        }

        private void SetupEnvironmentVariables()
        {
            const string launchSettingsFilePath = "Properties\\launchSettings.json";

            if (!File.Exists(launchSettingsFilePath))
            {
                return;
            }

            using (var file = File.OpenText(launchSettingsFilePath))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }
        private void SetupClient(string runtimeEnvironment)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (request, cert, chain, errors) => true
            };

            if (runtimeEnvironment == "Local")
            {
                _server = new TestServer(new WebHostBuilder()
                    .UseEnvironment(runtimeEnvironment)
                    .UseStartup<Startup>())
                {
                    BaseAddress = new Uri("http://localhost:8000")
                };

                Client = _server.CreateClient();
            }
            else
            {
                Client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(Environment.GetEnvironmentVariable("PartnerUserEndpoint"))
                };
            }
            BslClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(StackVariables.BslBaseAddress)
            };
            AuthServerClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(Environment.GetEnvironmentVariable("AuthServerBaseAddress"))
            };
    }
    }
}
