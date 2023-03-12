using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quiz_maker_models.Helpers
{
    public static class ResourceHelper
    {
        private static List<ResourceManager> _resourceManagers = new List<ResourceManager>()
        {
            Resources.ResourceManager,
            ErrorRexs.ResourceManager
        };

        public static List<ResourceManager> ResourcesMangers => _resourceManagers;

        public static void RegisterNewResourceManager(ResourceManager mananger)
        {
            if (_resourceManagers.Any(x => x.BaseName == mananger.BaseName))
            {
                return;
            }

            _resourceManagers.Add(mananger);
        }

        /// <summary>
        /// Not yet work
        /// </summary>
        public static void ApplyResources(this Form form)
        {

        }

        public static void ApplyResources(this Control control)
        {
            if (isNotApplyResource(control.Name))
            {
                return;
            }
            var key = replace(control.Name);
            control.Text = Translate(key);
        }

        private static string replace(string key)
        {
            key = key.Replace("btn", "")
                     .Replace("lbl", "")
                     .Replace("txt", "")
                     .Replace("chk", "");
            return key;
        }

        private static bool isNotApplyResource(string key)
        {
            return key.StartsWith("txt") ||
                   key.EndsWith("txt") ||
                   key.StartsWith("_") ||
                   key.EndsWith("_");
        }

        public static string Translate(string key)
        {
            foreach (var manager in ResourcesMangers)
            {
                var result = manager.GetString(key);
                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }
            }

            return key;
        }

        /// <summary>
        /// Convention for naming enum key in Resources is
        /// Name of Enum + "_" + Property Name
        /// Example: "QuestionType_Text" as key, then value "Question Text"
        /// </summary>
        public static string Translate(Enum @enum)
        {
            var key = $"{@enum.GetType().Name}_{@enum.ToString()}";
            return Translate(key);
        }
    }
}
