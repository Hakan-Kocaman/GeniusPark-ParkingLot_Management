using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLot.Data
{
    [Table("Roles")]

    public class Role
    {
        [Key]
        [Column("Roles_id")]
        public int Id { get; set; }
        [Column("Roles_title")]
        public string Title { get; set; }
    }
}
