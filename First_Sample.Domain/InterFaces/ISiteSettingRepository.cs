using First_Sample.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Domain.InterFaces
{
    public interface ISiteSettingRepository
    {
        Task<IReadOnlyList<SiteSetting>> GetAll();
        Task<SiteSetting> GetKey(string id);
        Task<bool> Add(SiteSetting siteSetting);
        Task<bool> Update(SiteSetting siteSetting);
    }
}
