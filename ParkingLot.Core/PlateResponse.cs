using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot.Core
{
    public class PlateResponse
    {
        public bool Success { get; set; }
        public string? Plate { get; set; }
        public double Confidence { get; set; }
        public string? Error { get; set; }


    }
}
