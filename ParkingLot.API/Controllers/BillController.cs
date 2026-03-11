using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.API.Services;
using ParkingLot.Data;

namespace ParkingLot.API.Controllers
{
    [Route("api/bill")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillService _postBillService;

        public BillController(BillService postBillService)
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
