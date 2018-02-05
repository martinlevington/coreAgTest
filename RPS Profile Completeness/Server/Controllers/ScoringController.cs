using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RPS.Data.Elasticsearch;
using RPS.Domain.Data;

namespace RPS.Presentation.Server.Controllers
{
    [Route("api/[controller]")]
    public class ScoringController : Controller
    {
        private readonly IOptions<ElasticSearchConfiguration> _optionsApplicationConfiguration;
        private readonly IScoringRepository _scoringRepository;


        public ScoringController(
            IScoringRepository scoringRepository,
            IOptions<ElasticSearchConfiguration> options)
        {
            _scoringRepository = scoringRepository;
            _optionsApplicationConfiguration = options;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("AddAllData")]
        public IActionResult AddAllData()
        {
            var path = _optionsApplicationConfiguration.Value.FilePath;
            _scoringRepository.AddAllData("rpsData.json");
            return Ok();
        }
    }
}
