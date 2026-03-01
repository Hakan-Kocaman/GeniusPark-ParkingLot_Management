using System;
using System.Collections.Generic;
using System.Text;
using ParkingLot.Data;

namespace ParkingLot.Core
{
    public class PreloadResponse
    {
        public Company Company { get; set; }
        public List<Bill> Bill { get; set; }

    }
}
