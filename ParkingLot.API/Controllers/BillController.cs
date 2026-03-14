using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLot.API.Services;
using ParkingLot.Core;
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
        public async Task<IActionResult> PostBill(BillRequest billrequest)
        {
            if (billrequest == null)
            {
                return BadRequest("Bill data is required.");
            }

            var result = await _postBillService.PostBill(billrequest);


            return Ok(result);
        }

        [HttpGet("{company_id}/{parkinglot_id}")]
        public async Task<IActionResult> GetBills(int company_id, int parkinglot_id)
        {
            var bills = await _postBillService.GetBills(company_id, parkinglot_id);
            return Ok(bills);

        }
    }
}
