using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace ParkingLot.Data
{
    

    public class ParkingLotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<DayType> DayTypes { get; set; }
        public DbSet<SpecificDay> SpecificDays { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Bill> Bills { get; set; }

        public ParkingLotDbContext(DbContextOptions<ParkingLotDbContext> options)
            : base(options)
        {

        }

        
    }
}
