using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace First_Sample.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly First_Sample_Context _context;
        public UserRepository(First_Sample_Context context)
        {
            _context = context;
        }

        public async Task<bool> Add(User entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;// اگر رکوردی اضافه شد، true برگردانید.
            }
            catch (Exception)
            {
                // در صورت بروز خطا، false برگردانید.
                return false;
            }
        }

        public async Task<User> GetUserById(int id)
        {
            var users = await _context.Users.FirstOrDefaultAsync(o => o.UserId == id);
            return users;
        }

        public async Task<IReadOnlyList<User>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<bool> Remove(User entity)
        {
            try
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
                return true; // اگر رکوردی اضافه شد، true برگردانید.
            }
            catch (Exception)
            {
                // در صورت بروز خطا، false برگردانید.
                return false;
            }
        }

        public async Task<bool> Update(User entity)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return true; // اگر رکوردی اضافه شد، true برگردانید.
            }
            catch (Exception)
            {
                // در صورت بروز خطا، false برگردانید.
                return false;
            }
        }
    }
}
