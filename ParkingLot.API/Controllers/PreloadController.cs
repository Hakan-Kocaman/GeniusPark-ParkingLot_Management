 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.API.Services;

namespace ParkingLot.API.Controllers
{
    [ApiController]
    [Route("api/preload")]

    //sql->c#
    public class PreloadController : ControllerBase
    {
        private readonly PreloadService _preloadService;

        public PreloadController(PreloadService preloadService)
        {
            _preloadService = preloadService;
        }

        [HttpGet("{company_id}")]
        public async Task<IActionResult> Preload(int company_id)
        {
            var result = await _preloadService.Preload(company_id);

            if (result == null)
                return StatusCode(500, "Preload service error");

            return Ok(result);
        }
    }
}

