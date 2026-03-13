using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("SubscriptedVehicle")]
    public class SubscriptedVehicle
    {
        [Key]
        [Column("SubscriptedVehicle_id")]
        public int Id { get; set; }
        [Column("SubscriptedVehicle_licensePlate")]
        public string LicensePlate { get; set; }
        [Column("SubscriptedVehicle_startDate")]
        public DateTime StartDate { get; set; }
        [Column("SubscriptedVehicle_endDate")]
        public DateTime? EndDate { get; set; }
        [Column("Subscription_id")]
        public int Subscription_id { get; set; }

    }
}
