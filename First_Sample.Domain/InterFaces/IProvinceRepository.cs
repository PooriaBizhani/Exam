using First_Sample.Domain.Entities;

namespace First_Sample.Domain.InterFaces
{
    public interface IProvinceRepository
    {
        Task<IReadOnlyList<Province>> GetAllProvinces();
        Task<Province> GetProvinceById(int id);
    }
}
