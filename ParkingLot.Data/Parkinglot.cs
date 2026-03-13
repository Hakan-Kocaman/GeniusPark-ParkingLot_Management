using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("Parkinglot")]
    public class Parkinglot
    {
        [Key]
        [Column("Parkinglot_id")]
        public int Id { get; set; }
        [Column("Parkinglot_name")]
        public string Name { get; set; }
        [Column("Company_id")]
        public int Company_id { get; set; }
    }
}
