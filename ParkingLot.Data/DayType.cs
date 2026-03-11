using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("DayType")]
    public class DayType
    {
        [Key]
        [Column("DayType_id")]
        public int Id { get; set; }
        
        [Column("DayType_title")]
        public string Title { get; set; }
        [Column("Company_id")]
        public int Company_id { get; set; }
    }
}
