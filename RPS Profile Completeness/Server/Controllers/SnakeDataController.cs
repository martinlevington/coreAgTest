using Microsoft.AspNetCore.Mvc;
using RPS.Data.Elasticsearch;
using RPS.Domain.Snakes;

namespace RPS.Presentation.Server.Controllers
{
  [Route("snakedata")]
  public class SnakeDataController : Controller
  {
    private IElasticSearchContext _context;
    private ISnakeDataRepository _snakeDataRepository;

    public SnakeDataController(ISnakeDataRepository snakeDataRepository, IElasticSearchContext context)
    {
      _snakeDataRepository = snakeDataRepository;
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
  }
}
