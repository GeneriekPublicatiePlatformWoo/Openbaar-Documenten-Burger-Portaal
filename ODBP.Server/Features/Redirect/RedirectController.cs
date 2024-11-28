using Microsoft.AspNetCore.Mvc;

namespace ODBP.Features.Redirect
{
    [ApiController]
    public class RedirectController(IConfiguration config): ControllerBase
    {
        [HttpGet("/{**wildcard}")]
        public IActionResult Get() => Uri.TryCreate(config["REDIRECT_URL"], UriKind.Absolute, out var uri)
            ? Redirect(uri.ToString())
            : NotFound();
    }
}
