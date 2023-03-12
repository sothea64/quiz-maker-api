using Microsoft.Extensions.DependencyInjection;
using quiz_maker_api.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace quiz_maker_api.Helpers
{
    public static class LogicInjectionHelpers
    {
        /// <summary>
        /// Scan all assemlby all class endsWith Logic and inject to DI services.
        /// </summary> 
        public static void InjectAllLogics(this IServiceCollection services)
        {
            var logicsAssembly = Assembly.GetAssembly(typeof(QuizLogic));
            var types = logicsAssembly.GetTypes().Where(x => x.Name.EndsWith("Logic"));
            foreach (var item in types)
            {
                services.AddTransient(item);
            }
        }
    }
}
