using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot.Data
{
    public class Bill
    {
       public int Id { get; set; }
       public string LicensePlate { get; set; }
       public DateTime EnterDate { get; set; }
       public DateTime ExitDate { get; set; }
       public decimal Price { get; set; }
       public int Company_id  { get; set; }
       public int Pricing_id { get; set; }
       public int User_id { get; set; }


    }
}
