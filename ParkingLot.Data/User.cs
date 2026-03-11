using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingLot.Data
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("Users_id")]
        public int Id { get; set; }
        [Column("Users_name")]
        public string Name { get; set; }
        [Column("Users_password")]
        public string Password { get; set; }
        [Column("Company_id")]
        public int Company_id { get; set; }

    }
}
