using HybridLifecycle.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HybridLifecycle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet("{key}")]
        public IActionResult GetSetting(string key)
        {
            var value = _configurationService.GetSetting(key);
            return Ok(value);
        }

        [HttpPost("{key}")]
        public IActionResult UpdateSetting(string key, [FromBody] string value)
        {
            _configurationService.UpdateSetting(key, value);
            return Ok();
        }
    }
}
