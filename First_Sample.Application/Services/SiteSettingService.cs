using First_Sample.Application.InterFaces;
using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Domain.ViewModels.SiteSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Application.Services
{
    public class SiteSettingService : ISiteSettingService
    {
        private readonly ISiteSettingRepository _siteSettingRepository;

        public SiteSettingService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        public async Task<bool> AddService(SiteSettingVM key)
        {
            var setting = new SiteSetting
            {
                Key = key.Key,
                Value = key.Value,
                LastTimeChenged = key.LastTimeChenged
            };
            var IsAdded = await _siteSettingRepository.Add(setting);
            return IsAdded;
        }

        public async Task<IReadOnlyList<SiteSetting>> GetAllService()
        {
            var settings = await _siteSettingRepository.GetAll();
            return settings;
        }

        public async Task<SiteSetting> GetKeyService(string key)
        {
            var guid = await _siteSettingRepository.GetKey(key);
            return guid;
        }

        public async Task<bool> UpdateService(SiteSettingVM key)
        {
            var setting = new SiteSetting
            {
                Key = key.Key,
                Value = key.Value,
                LastTimeChenged = key.LastTimeChenged
            };
            var guid = await _siteSettingRepository.Update(setting);
            return guid;
        }
    }
}
