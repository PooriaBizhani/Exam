using AutoMapper;
using First_Sample.Application.Dtos.User;
using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;

namespace First_Sample.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository context)
        {
            _userRepository = context;
        }
        
        public async Task<User> GetByIdService(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return user;
        }
        public async Task<IReadOnlyList<User>> GetAllService()
        {
            var Users = await _userRepository.GetAllUser();
            return Users;
        }
        public async Task AddService(UserDto userVm)
        {
            var user = new User
            {
                Name = userVm.Name,
                Family = userVm.Family,
                NationalCode = userVm.NationalCode,
                PhoneNumber = userVm.PhoneNumber,
                ProvinceName = userVm.ProvinceName,
                CityName = userVm.CityName,
                Address = userVm.Address,
                Photo = userVm.Photo // در صورتی که فایل عکس در userVm قرار دارد
            };

            // اضافه کردن به مخزن داده‌ها
            await _userRepository.Add(user);
        }
    }
}
