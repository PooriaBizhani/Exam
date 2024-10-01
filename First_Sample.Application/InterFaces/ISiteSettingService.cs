using First_Sample.Domain.Entities;
using First_Sample.Domain.ViewModels.SiteSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Application.InterFaces
{
    public interface ISiteSettingService
    {
        Task<IReadOnlyList<SiteSetting>> GetAllService();
        Task<SiteSetting> GetKeyService(string key);
        Task<bool> AddService(SiteSettingVM key);
        Task<bool> UpdateService (SiteSettingVM key);
    }
}
