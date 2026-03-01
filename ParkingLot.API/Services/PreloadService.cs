using Microsoft.EntityFrameworkCore;
using ParkingLot.Core;
using ParkingLot.Data;
using System.ComponentModel.Design;

namespace ParkingLot.API.Services
{
    public class PreloadService
    {
        private readonly ParkingLotDbContext _context;

        public PreloadService(ParkingLotDbContext context)
        {
            _context = context;
        }

        public async Task<PreloadResponse> Preload(int company_id)
        {
            var company = await _context.Companies.SingleOrDefaultAsync(c=> c.Id == company_id);

            if (company == null) {
                throw new Exception("No company found in the database.");
            }
            

            var bills = await _context.Bills.Where(b => b.Company_id == company_id).ToListAsync();

            var _preloadResponse = new PreloadResponse
            {
                Company = company,
                Bill   = bills
            };

            return _preloadResponse;
        }
    }
}
