using System.Net.Http;
using System.Text;
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
        public async void TestGraphQLGet()
        {
           
           var response = await _client.GetAsync("/api/Dashboard");

            // Then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("hello", responseString);

        }


        [Fact]
        public async void TestGraphQL()
        {
            // Given 
            var query = @"{
                ""query"": ""query { monthlys { Change Score }  }""
            }";
            var content = new StringContent(query, Encoding.UTF8, "application/json");

            // When
            var response = await _client.PostAsync("/api/Dashboard", content);

            // Then
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("R2-D2", responseString);
        }
    }
}