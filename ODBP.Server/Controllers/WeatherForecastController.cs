using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODBP.Data;

namespace ODBP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly OdbpDbContext _context;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(OdbpDbContext context, ILogger<WeatherForecastController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return _context.WeatherForecasts.AsEnumerable();
        }
    }
}