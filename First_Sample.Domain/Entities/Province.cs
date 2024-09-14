using System.ComponentModel.DataAnnotations;

namespace First_Sample.Domain.Entities
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }
        public string Name { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
