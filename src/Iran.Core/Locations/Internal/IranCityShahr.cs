using System;
using System.Collections.Generic;
using System.Text;

namespace Iran.Core.Locations.Internal
{
    internal class IranCityShahr
    {
        public int id { get; set; }
        public string name { get; set; }
        public int shahr_type { get; set; }
        public int ostan { get; set; }
        public int shahrestan { get; set; }
        public int bakhsh { get; set; }
        public int amar_code { get; set; }
    }
}
