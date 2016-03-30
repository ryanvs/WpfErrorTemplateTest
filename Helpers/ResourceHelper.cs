using System.Collections;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace WpfErrorTemplateTest.Helpers
{
    public static class ResourceHelper
    {
        /// <summary>
        /// http://stackoverflow.com/a/5185454/29762
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string[] GetResourcesUnder(string folder)
        {
            folder = folder.ToLower() + "/";

            var assembly = Assembly.GetCallingAssembly();
            var resourcesName = assembly.GetName().Name + ".g.resources";
            var stream = assembly.GetManifestResourceStream(resourcesName);
            var resourceReader = new ResourceReader(stream);

            var resources =
                from p in resourceReader.OfType<DictionaryEntry>()
                let theme = (string)p.Key
                where theme.StartsWith(folder)
                select theme.Substring(folder.Length);

            return resources.ToArray();
        }
    }
}
