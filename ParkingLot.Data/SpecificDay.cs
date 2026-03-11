using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("SpecificDay")]
    public class SpecificDay
    {
        [Key]
        [Column("SpecificDay_id")]
        public int Id { get; set; }
        [Column("SpecificDay_month")]
        public int Month { get; set; }
        [Column("SpecificDay_day")]
        public int Day { get; set; }
        [Column("DayType_id")]
        public int DayType_id { get; set; }
        [Column("Company_id")]
        public int Company_id { get; set; }
    }
}
