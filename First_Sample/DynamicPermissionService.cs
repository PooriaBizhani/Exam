using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace First_Sample.Presentation
{
    public class DynamicPermissionService
    {
        public class ControllerActionInfo
        {
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
        }
        public List<ControllerActionInfo> GetControllersAndActions()
        {
            var controllerActionList = new List<ControllerActionInfo>();

            // دریافت تمام کنترلرهای موجود در اسمبلی
            var controllers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type) && !type.IsAbstract)
                .ToList();

            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => !m.IsDefined(typeof(NonActionAttribute)));

                foreach (var action in actions)
                {
                    controllerActionList.Add(new ControllerActionInfo
                    {
                        ControllerName = controller.Name,
                        ActionName = action.Name
                    });
                }
            }

            return controllerActionList;
        }
    }
}
