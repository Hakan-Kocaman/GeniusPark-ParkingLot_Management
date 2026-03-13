using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("Subscription")]
    public class Subscription
    {
        [Key]
        [Column("Subscription_id")]
        public int Id { get; set; }
        [Column("Subscription_duration")]
        public int Duration { get; set; }
        [Column("Subscription_coveringPercentage")]
        public decimal CoveringPercentage { get; set; }
        [Column("Company_id")]
        public int Company_id { get; set; }
    }
}
