using Microsoft.AspNetCore.Http;

namespace First_Sample.Shared.ViewModels.RegisterUser
{
    public class RegisterUserVM
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public IFormFile Photo { get; set; }
    }
}
