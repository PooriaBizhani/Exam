using First_Sample.Domain.Entities;

namespace First_Sample.Domain.InterFaces
{
    public interface ICityRepository
    {
        Task<IReadOnlyList<City>> GetAllCities();
        Task<City> GetCityById(int id);
        Task<List<City>> GetCitiesByProvinceId(int provinceId);

    }
}
