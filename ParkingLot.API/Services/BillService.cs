using ParkingLot.Data;
using Microsoft.EntityFrameworkCore;

namespace ParkingLot.API.Services
{
    public class BillService
    {
        private readonly ParkingLotDbContext _context;

        public BillService(ParkingLotDbContext context)
        {
            _context = context;
        }
        bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
        public async Task<Bill> PostBill(Bill bill)
        {
            try {
                var bills = await _context.Bills.FirstOrDefaultAsync(b => (b.LicensePlate == bill.LicensePlate) && (b.ExitDate==null));
                if (bills == null)
                {
                    bill.EnterDate = DateTime.Now;
                    bill.ExitDate = null;
                    bill.Price = null;
                    await _context.Bills.AddAsync(bill);
                    await _context.SaveChangesAsync();
                    return bill;
                }
                else
                {
                    bills.ExitDate = DateTime.Now;
                    var timeSpan = bills.ExitDate.Value - bills.EnterDate;
                    Double hours = (timeSpan.TotalHours);

                    var daytypes = await _context.DayTypes.Where(d => d.Company_id == bill.Company_id).ToListAsync();

                    var month = DateTime.Now.Month;
                    var day = DateTime.Now.Day;
                    var specificdays = await _context.SpecificDays.FirstOrDefaultAsync(s => (s.Company_id == bill.Company_id) && (s.Day == day) && (s.Month == month));
                    int? dayTypeId = null;

                    if (specificdays != null)
                    {
                        dayTypeId = specificdays.DayType_id;
                    }
                    else
                    {
                        string daytypeName = IsWeekend(bills.EnterDate) ? "Weekend" : "Weekday";

                        dayTypeId = await _context.DayTypes
                            .Where(d => d.Title == daytypeName)
                            .Select(d => d.Id)
                            .FirstOrDefaultAsync();
                    }

                    if (dayTypeId != null)
                    {
                        var pricing = await _context.Pricings
                            .Where(p => p.DayType_id == dayTypeId
                                     && hours >= (double)(p.StartHour)
                                     && hours < (double)(p.EndHour))
                            .FirstOrDefaultAsync();

                        if (pricing != null)
                        {
                            bills.Price = pricing.PriceOfInterval;
                            bills.Pricing_id = pricing.Id;
                        }
                    }

                    _context.Bills.Update(bills);
                    await _context.SaveChangesAsync();
                    return bills;
                }


                }catch(Exception e) { throw new Exception(e.Message); }
                
            
        }
    }
}
