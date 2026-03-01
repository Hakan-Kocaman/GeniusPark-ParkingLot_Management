using Microsoft.EntityFrameworkCore;
using ParkingLot.Data;

namespace ParkingLot.API.Services
{
    public class UserService
    {
        public readonly ParkingLotDbContext _context;
        public UserService(ParkingLotDbContext context) {
            _context = context;
        }

        public async Task<User> Login(string user_name, string user_password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Name == user_name && u.Password == user_password);
            if (user == null) {
                throw new Exception("No user found in the database.");
            }
            return user;
        }
    }
}
