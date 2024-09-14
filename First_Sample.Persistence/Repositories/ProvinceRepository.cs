using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace First_Sample.Persistence.Repositories
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly First_Sample_Context _context;
        public ProvinceRepository(First_Sample_Context context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Province>> GetAllProvinces()
        {
            var Province = await _context.Provinces.ToListAsync();
            return Province;
        }

        public async Task<Province> GetProvinceById(int id)
        {
            var Province = await _context.Provinces.FirstOrDefaultAsync(o => o.ProvinceId == id);
            return Province;
        }
    }
}
