using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;


namespace RPS.Presentation.Functional.Tests.Controllers
{
   
    public class DashboardControllerTestsController 
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;


        public DashboardControllerTestsController()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>()
            );
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task DashControllelrGraphQLGet()
        {
           
           var response = await _client.GetAsync("/api/Dashboard");

            // Then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("monthlyscores", responseString);

        }


        [Fact]
        public async Task MonthlyscoresQueryChangeAndScore()
        {
            // Given 
            var query = @"{
                ""query"": ""query { monthlyscores { change score }  }""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            // When
            var response = await _client.PostAsync("/api/Dashboard", content);

            // Then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            responseString.Should().Contain("monthlyscore");
            responseString.Should().Contain("score");

        }

        [Fact]
        public async Task MonthlyscoresQueryChangeOnly()
        {
            // Given 
            var query = @"{
                ""query"": ""query { monthlyscores { change  }  }""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            // When
            var response = await _client.PostAsync("/api/Dashboard", content);

            // Then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("monthlyscore");
            responseString.Should().Contain("score");
            responseString.Should().NotContain("score ");
        }
    }
}