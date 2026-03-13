using ParkingLot.Data;
using Microsoft.EntityFrameworkCore;
using ParkingLot.Core;

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
                var bills = await _context.Bills.FirstOrDefaultAsync(b => (b.LicensePlate == bill.LicensePlate) && (b.ExitDate == null));
                if (bills == null)
                {
                    var subscriptedVehicles = await _context.SubscriptedVehicles.FirstOrDefaultAsync(sv=> sv.LicensePlate==bill.LicensePlate);

                    if (subscriptedVehicles != null)
                    {
                        bill.Subscription_id = subscriptedVehicles.Subscription_id;
                    }

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
                            Decimal coveringDiscount;
                                
                                var subscriptedVehicle = await _context.SubscriptedVehicles.FirstOrDefaultAsync(sv => sv.Subscription_id == bills.Subscription_id && DateTime.Now < sv.EndDate && DateTime.Now > sv.StartDate);
                            if (subscriptedVehicle == null) { 
                                coveringDiscount = 0;
                            }
                            else { 
                                var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscriptedVehicle.Subscription_id);
                                if (subscription == null) { coveringDiscount = 0; }
                                else
                                    coveringDiscount = subscription.CoveringPercentage;
                            }                                                                               
                            bills.Price = pricing.PriceOfInterval - pricing.PriceOfInterval*coveringDiscount;
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
