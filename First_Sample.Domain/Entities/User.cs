using First_Sample.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace First_Sample.Domain.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Photo { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
