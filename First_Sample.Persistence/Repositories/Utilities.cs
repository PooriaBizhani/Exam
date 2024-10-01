using First_Sample.Domain.Entities;
using First_Sample.Domain.InterFaces;
using First_Sample.Domain.ViewModels.Role;
using First_Sample.Persistence.Context;
using Microsoft.AspNetCore.Mvc; // فضای نام صحیح
using System.Reflection;

namespace First_Sample.Persistence.Repositories
{
    public class Utilities : IUtilities
    {
        private readonly First_Sample_Context _context;
        public Utilities(First_Sample_Context context)
        {
            _context = context;
        }

        public IList<string> GetAllAreasNames()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var contradistinction = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type)) // استفاده از نوع صحیح Controller
                .SelectMany(type =>
                    type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new
                {
                    Area = x.DeclaringType?.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))

                });

            var list = new List<string>();

            foreach (var item in contradistinction)
            {
                list.Add(item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault());
            }

            if (list.All(string.IsNullOrEmpty))
            {
                return new List<string>();
            }

            list.RemoveAll(x => x == null);

            return list.Distinct().ToList();
        }

        public string DataBaseRoleValidationGuid()
        {
            var roleValidationGuid =
                _context.SiteSettings.SingleOrDefault(s => s.Key == "RoleValidationGuid")?.Value;

            while (roleValidationGuid == null)
            {
                _context.SiteSettings.Add(new SiteSetting()
                {
                    Key = "RoleValidationGuid",
                    Value = Guid.NewGuid().ToString(),
                    LastTimeChenged = DateTime.Now
                });
                _context.SaveChanges();
                roleValidationGuid =
                    _context.SiteSettings.SingleOrDefault(s => s.Key == "RoleValidationGuid")?.Value;
            }

            return roleValidationGuid;
        }
    }
}
