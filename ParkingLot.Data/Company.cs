using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("Company")]
    public class Company
    {
       [Key]
       [Column("Company_id")]
       public int Id { get; set; }
       [Column("Company_name")]
       public string Name { get; set; }

    }
}
