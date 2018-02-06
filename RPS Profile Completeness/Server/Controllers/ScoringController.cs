using System;
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

        [HttpGet("TopImprovers")]
        public IActionResult TopImprovers()
        {
            var range = DateTime.Parse("2017-11-10");
            var topImporvers = _scoringRepository.GetTopImprovers(5, range);

            return Json(topImporvers);
        }

        [HttpGet("top10")]
        public IActionResult Top10()
        {

            var topImporvers = _scoringRepository.Get(10);

            return Json(topImporvers);
        }

        [HttpGet("AddAllData")]
        public IActionResult AddAllData()
        {
            var path = _optionsApplicationConfiguration.Value.FilePath;
            _scoringRepository.AddAllData("rpsData.json");
            return Ok();
        }

        [HttpGet("UpdateAllData")]
        public IActionResult UpdateAllData()
        {
            var path = _optionsApplicationConfiguration.Value.FilePath;
            _scoringRepository.UpdateAllData("rpsData.json");
            return Ok();
        }
    }


}
