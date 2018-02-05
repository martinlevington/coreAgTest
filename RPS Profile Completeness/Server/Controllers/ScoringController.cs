using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RPS.Presentation.Server.Controllers
{
  [Route("api/[controller]")]
  public class ScoringController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
