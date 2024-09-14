using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;

namespace First_Sample.Application.Services
{
    public class CityService
    {
        private readonly ICityRepository _cityRepository;
        public CityService(ICityRepository context)
        {
            _cityRepository = context;
        }
        public async Task<IReadOnlyList<City>> GetAllService()
        {
            var City = await _cityRepository.GetAllCities();
            return City;
        }
        public async Task<City> GetByIdService(int id)
        {
            var City = await _cityRepository.GetCityById(id);
            return City;
        }
        public async Task<IReadOnlyList<City>> GetCitiesByProvinceIdService(int id)
        {
            var City = await _cityRepository.GetCitiesByProvinceId(id);
            return City;
        }
    }
}
