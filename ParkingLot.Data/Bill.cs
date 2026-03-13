using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("Bill")]
    public class Bill
    {
        [Key]
            [Column("Bill_id")]
        public int Id { get; set; }
         [Column("Bill_licensePlate")]
        public string LicensePlate { get; set; }
            [Column("Bill_enterDate")]
        public DateTime EnterDate { get; set; }
            [Column("Bill_exitDate")]
        public DateTime? ExitDate { get; set; }
            [Column("Bill_price")]
        public decimal? Price { get; set; }
                [Column("Company_id")]
        public int Company_id  { get; set; }
                [Column("Pricing_id")]
        public int? Pricing_id { get; set; }
                [Column("Users_id")]
        public int User_id { get; set; }
        [Column("Subscription_id")]
        public int? Subscription_id { get; set; }
        [Column("Parkinglot_id")]
        public int Parkinglot_id { get; set; }


    }
}
