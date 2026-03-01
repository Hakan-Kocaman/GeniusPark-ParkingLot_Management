using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot.Data
{
    public class Pricing
    {
       public int Id { get; set; }
       public decimal StartHour { get; set; }
       public decimal EndHour { get; set; }
       public decimal PriceOfInterval { get; set; }
       public int DayType_id { get; set; }
       public int SpecificDay_id { get; set; }

    }
}
