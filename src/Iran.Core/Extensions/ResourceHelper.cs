using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Iran.Core.Extensions {
    public static class ResourceHelper {

        public static Stream GetResourceStream(this Assembly assembly, string resourcePath) {
            List<string> resourceNames = new List<string>(assembly.GetManifestResourceNames());

            resourcePath = resourcePath.Replace(@"/", ".");
            resourcePath = resourceNames.FirstOrDefault(r => r.Contains(resourcePath));

            if (resourcePath == null)
                throw new FileNotFoundException("Resource not found");

            return assembly.GetManifestResourceStream(resourcePath);
        }

        public static Stream GetResourceStream(string resourcePath) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return assembly.GetResourceStream(resourcePath);
        }


    }
}
