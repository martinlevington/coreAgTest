using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RPS.Data.Elasticsearch;
using RPS.Domain.Snakes;

namespace RPS.Presentation.Server.Controllers
{
  [Route("api/[controller]")]
  public class SnakeDataController : Controller
  {
    private IElasticSearchContext _context;
    private ISnakeDataRepository _snakeDataRepository;
    private readonly IOptions<ElasticSearchConfiguration> _optionsApplicationConfiguration;

    public SnakeDataController(ISnakeDataRepository snakeDataRepository, IElasticSearchContext context, IOptions<ElasticSearchConfiguration> options)
    {
      _snakeDataRepository = snakeDataRepository;
      _optionsApplicationConfiguration = options;
      _context = context;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpGet]
    [Route("create")]
    public JsonResult Create()
    {
      var result = _context.GetClient().CreateIndex("netcore");

      return new JsonResult(result);
    }

    [HttpGet]
    [Route("delete")]
    public JsonResult Delete()
    {
      var result = _context.GetClient().DeleteIndex("netcore");

      return new JsonResult(result);
    }



    [HttpGet("GeographicalRegions")]
    public List<SnakeBites> GetGeographicalRegions()
    {
      return _snakeDataRepository.GetGeographicalRegions();
    }

    [HttpGet("RegionBarChart/{region}")]
    public GeographicalCountries GetBarChartDataForRegion(string region)
    {
      return _snakeDataRepository.GetBarChartDataForRegion(region);
    }

    // http://localhost:8333/api/SnakeData/AddAllData
    [HttpGet("AddAllData")]
    public IActionResult AddAllData()
    {

      var path = _optionsApplicationConfiguration.Value.FilePath;
      _snakeDataRepository.AddAllData(path);
     return Ok();
    }

  }
}
