using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("Pricing")]
    public class Pricing
    {
       [Key]
       [Column("Pricing_id")]
       public int Id { get; set; }
       [Column("Pricing_startHour")]
        public decimal StartHour { get; set; }
         [Column("Pricing_endHour")]
        public decimal EndHour { get; set; }
        [Column("Pricing_priceOfInterval")]
        public decimal PriceOfInterval { get; set; }
            [Column("DayType_id")]
        public int DayType_id { get; set; }
            [Column("SpecificDay_id")]
        public int? SpecificDay_id { get; set; }

    }
}
