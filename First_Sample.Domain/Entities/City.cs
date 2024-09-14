using System.ComponentModel.DataAnnotations;

namespace First_Sample.Domain.Entities
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        public string Name { get; set; }
        public int ProvinceId { get; set; }
        public Province Provinces { get; set; }
    }
}
