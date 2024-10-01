using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Domain.ViewModels.Role
{
    public class CreateRoleVM
    {
        public CreateRoleVM()
        {
            ActionAndControllerNames = new List<ActionAndControllerName>();
        }
        public string RoleName { get; set; }
        public IList<ActionAndControllerName> ActionAndControllerNames { get; set; }
    }
    public class ActionAndControllerName
    {
        public string AreaName { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public bool IsSelected { get; set; }
    }
}
