using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.API.Services;
using ParkingLot.Data;

namespace ParkingLot.API.Controllers
{
    [Route("api/bill")]
    [ApiController]
    public class PostBillController : ControllerBase
    {
        private readonly PostBillService _postBillService;

        public PostBillController(PostBillService postBillService)
        {
            _postBillService = postBillService;
        }


        [HttpPost]
        public async Task<IActionResult> PostBill(Bill bill)
        {
            if (bill == null)
            {
                return BadRequest("Bill data is required.");
            }

            var result = await _postBillService.PostBill(bill);
         
            
            return Ok(result);
        }

    }
}
