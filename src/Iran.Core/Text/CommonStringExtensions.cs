using System;
using System.Collections.Generic;
using System.Text;

namespace Iran.Core.Text {
    public static class CommonStringExtensions {

        public static bool IsNullOrEmpty(this string value)
            => string.IsNullOrWhiteSpace(value);

        public static bool IsNotNullOrEmpty(this string value)
            => !IsNullOrEmpty(value);
    }
}
