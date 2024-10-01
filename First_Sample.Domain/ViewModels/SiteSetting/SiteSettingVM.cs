using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Domain.ViewModels.SiteSetting
{
    public class SiteSettingVM
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? LastTimeChenged { get; set; }
    }
}
