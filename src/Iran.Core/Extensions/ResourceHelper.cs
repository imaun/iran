using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Iran.Core.Extensions {
    public static class ResourceHelper {

        public static Stream GetResourceStream(this Assembly assembly, string resourcePath)
            => assembly.GetManifestResourceStream(resourcePath);

        public static Stream GetResourceStream(string resourcePath) {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return GetResourceStream(assembly, resourcePath);
        }

    }
}
