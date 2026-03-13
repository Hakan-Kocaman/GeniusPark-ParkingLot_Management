using System;
using System.Collections.Generic;
using System.Text;
using ParkingLot.Data;

namespace ParkingLot.Core
{
    public class BillRequest
    {
        public Bill Bill { get; set; }
        public int Parkinglot_id { get; set; }
    }
}
