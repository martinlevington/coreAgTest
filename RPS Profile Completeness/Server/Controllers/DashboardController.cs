using System.Threading.Tasks;
using GraphQL;
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
        private IDashboardSchema _schema;
        private readonly ILogger _logger;

        public DashboardController(IDashboardSchema schema, IDocumentExecuter documentExecuter, ILogger logger)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var executionOptions = new ExecutionOptions { Schema = _schema, Query = query.Query };
            var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
