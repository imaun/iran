using System;
using System.Collections.Generic;
using System.Text;

namespace Iran.Core.Locations {

    public class IranCity {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhonePreNumber { get; set; }
        public string AmarCode { get; set; }
        public int? ParentId { get; set; }
        public int? ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public int? CountyId { get; set; }
        public string CountyName { get; set; }
        public int? DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? RuralSectionId { get; set; }
        public string RuralSectionName { get; set; }
        public string Latitude { get; set; }
        public string Longtiude { get; set; }
        public string Description { get; set; }
    }
}
