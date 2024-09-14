using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace First_Sample.Persistence.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly First_Sample_Context _context;
        public CityRepository(First_Sample_Context context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<City>> GetAllCities()
        {
            var city = await _context.Cities.ToListAsync();
            return city;
        }


        public async Task<City> GetCityById(int id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(o => o.CityId == id);
            return city;
        }

        public async Task<List<City>> GetCitiesByProvinceId(int provinceId)
        {
            // استفاده از ToListAsync برای دریافت لیستی از شهرها
            return await _context.Cities
                                 .Where(c => c.ProvinceId == provinceId)
                                 .ToListAsync();
        }
    }
}
