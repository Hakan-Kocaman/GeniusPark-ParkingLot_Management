using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.API.Services;
using ParkingLot.Core;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ParkingLot.API.Controllers
{
    [ApiController]
    [Route("api/plate")]


    // c#->py->c#

    public class DetectPlateController : ControllerBase
    {
        private readonly DetectPlateService _plateService;

        public DetectPlateController(DetectPlateService plateService)
        {
            _plateService = plateService;
        }
        [HttpPost("detect")]
        public async Task<IActionResult> Detect(IFormFile file)
        {
            var result = await _plateService.DetectPlate(file);

            if (result == null)
                return StatusCode(500, "ML service error");

            return Ok(result);
        }
    }

}
