using First_Sample.Domain.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace First_Sample.Domain.InterFaces
{
    public interface IUtilities
    {
        public IList<string> GetAllAreasNames();
        public string DataBaseRoleValidationGuid();
    }
}
