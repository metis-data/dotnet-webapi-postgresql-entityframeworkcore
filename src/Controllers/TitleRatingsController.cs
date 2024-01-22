using Microsoft.AspNetCore.Mvc;
using dotnet_webapi_postgresql_entityframeworkcore.Models;

namespace dotnet_webapi_postgresql_entityframeworkcore.Controllers;

[ApiController]
[Route("titles")]
public class TitleRatingsController : ControllerBase
{
    private readonly ILogger<TitleRatingsController> _logger;
    private readonly ImdbContext _context;

    public TitleRatingsController(ILogger<TitleRatingsController> logger, ImdbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("ratings/best")]
    public IEnumerable<TitleRating> Get()
    {
        return _context.TitleRatings.Where(t => t.AverageRating == 10.0);
    }
}
