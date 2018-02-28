using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RPS.Application;
using RPS.Application.Dashboard;
using RPS.Data.Elasticsearch;
using RPS.Domain.Data;

namespace RPS.Presentation.Server.Controllers
{
    [Route("api/[controller]")]
    public class ScoringController : Controller
    {
        private readonly IOptions<ElasticSearchConfiguration> _optionsApplicationConfiguration;
        private readonly IScoringRepository _scoringRepository;
        private readonly IDashboardService _dashboardService;


        public ScoringController(
            IScoringRepository scoringRepository,
            IOptions<ElasticSearchConfiguration> options, IDashboardService dashboardService)
        {
            _scoringRepository = scoringRepository;
            _optionsApplicationConfiguration = options;
            _dashboardService = dashboardService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("TopImprovers")]
        public IActionResult TopImprovers()
        {
            var range = DateTime.Parse("2017-11-24");
   
            var monthlyScoreRequest = new MonthlyScoreRequest()
            {
                StartPeriod  = range,
                NumberOfRecords = 10
            };


            var topImporvers = _dashboardService.GetMonthlyScores(monthlyScoreRequest);

            return Json(topImporvers);
        }

        [HttpGet("top10")]
        public IActionResult Top10()
        {

            var topImporvers = _scoringRepository.Get(10);

            return Json(topImporvers);
        }

        [HttpGet("monthlyavg")]
        public IActionResult MonthlyAvg()
        {

            var topImporvers = _scoringRepository.GetMonthlyAverage(10, 6);

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

        [HttpGet("DeleteIndex")]
        public IActionResult DeleteIndex()
        {
           
            _scoringRepository.DeleteIndex();
            return Ok();
        }
    }


}
