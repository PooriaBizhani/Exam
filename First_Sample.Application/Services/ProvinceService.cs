using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Persistence.Repositories;

namespace First_Sample.Application.Services
{
    public class ProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;
        public ProvinceService(IProvinceRepository context)
        {
            _provinceRepository = context;
        }
        public async Task<IReadOnlyList<Province>> GetAllService()
        {
            var Province = await _provinceRepository.GetAllProvinces();
            return Province;
        }
        public async Task<Province> GetByIdService(int id)
        {
            var Province = await _provinceRepository.GetProvinceById(id);
            return Province;
        }
    }
}
