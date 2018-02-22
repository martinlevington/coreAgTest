using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RPS.Presentation.Server.Models;

namespace RPS.Presentation.Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Dashboard")]
    public class DashboardController : Controller
    {

        private IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ISchema schema, IDocumentExecuter documentExecuter, ILogger<DashboardController> logger)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {

            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var executionOptions = new ExecutionOptions { Schema = _schema, Query = query.Query };
            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            return new OkObjectResult("{hello}");
        }
    }
}
