using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TimerController : Controller
    {
        private readonly TimerService _timerService;
        public TimerController(TimerService timerService)
        {
                _timerService= timerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result=await _timerService.GetAll();
            return Ok(result);
        }
    }
}
