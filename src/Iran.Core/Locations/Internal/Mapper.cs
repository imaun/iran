using System.Collections.Generic;

namespace Iran.Core.Locations.Internal {

    internal static class Mapper {
        /// <summary>
        /// Map Ostan fields to <see cref="IranCity"/>
        /// تبدیل اطلاعات استان ها به کلاس <see cref="IranCity"/>
        /// </summary>
        /// <param name="data">Ostan data</param>
        /// <returns>List of <see cref="IranCity"/></returns>
        public static IList<IranCity> ToIranCity(this IEnumerable<IranCityOstan> data) {
            var result = new List<IranCity>();
            foreach(var item in data) 
                result.Add(new IranCity {
                    AmarCode = item.amar_code,
                    Name = item.name,
                    Id = item.id,
                    CityType = IranCityType.Province
                });
            
            return result;
        }

        /// <summary>
        /// Map Shahrestan fields to <see cref="IranCity"/>
        /// تبدیل اطلاعات شهرستان ها به کلاس <see cref="IranCity"/>
        /// </summary>
        /// <param name="data">Shahrestan data</param>
        /// <returns>List of <see cref="IranCity"/></returns>
        public static IList<IranCity> ToIranCity(this IEnumerable<IranCityShahrestan> data) {
            var result = new List<IranCity>();
            foreach (var item in data)
                result.Add(new IranCity {
                    AmarCode = item.amar_code,
                    ProvinceId = item.ostan,
                    Name = item.name,
                    Id = item.id,
                    CityType = IranCityType.County
                });
            return result;
        }

        /// <summary>
        /// Map Bakhsh fields to <see cref="IranCity"/>
        /// تبدیل اطلاعات بخش ها به کلاس <see cref="IranCity"/>
        /// </summary>
        /// <param name="data">Bakhsh data</param>
        /// <returns>List of <see cref="IranCity"/></returns>
        public static IList<IranCity> ToIranCity(this IEnumerable<IranCityBakhsh> data) {
            var result = new List<IranCity>();
            foreach (var item in data)
                result.Add(new IranCity {
                    AmarCode = item.amar_code,
                    ProvinceId = item.ostan,
                    Name = item.name,
                    CountyId = item.shahrestan,
                    Id = item.id,
                    CityType = IranCityType.District
                });
            return result;
        }

        /// <summary>
        /// Map Shahr fields to <see cref="IranCity"/>
        /// تبدیل اطلاعات فایل شهر به کلاس <see cref="IranCity"/>
        /// </summary>
        /// <param name="data">Shahr data</param>
        /// <returns>List of <see cref="IranCity"/></returns>
        public static IList<IranCity> ToIranCity(this IEnumerable<IranCityShahr> data) {
            var result = new List<IranCity>();
            foreach (var item in data)
                result.Add(new IranCity {
                    AmarCode = item.amar_code,
                    Name = item.name,
                    Id = item.id,
                    ProvinceId = item.ostan,
                    CountyId = item.shahrestan,
                    DistrictId = item.bakhsh,
                    CityType = IranCityType.City,
                    ShahrType = item.shahr_type
                });
            return result;
        }

        /// <summary>
        /// Map Dehestan fields to <see cref="IranCity"/>
        /// تبدیل اطلاعات فایل دهستان به کلاس <see cref="IranCity"/>
        /// </summary>
        /// <param name="data">Dehestan data</param>
        /// <returns>List of <see cref="IranCity"/></returns>
        public static IList<IranCity> ToIranCity(this IEnumerable<IranCityDehestan> data) {
            var result = new List<IranCity>();
            foreach (var item in data)
                result.Add(new IranCity {
                    AmarCode = item.amar_code,
                    Id = item.id,
                    Name = item.name,
                    ProvinceId = item.ostan,
                    CountyId = item.shahrestan,
                    DistrictId = item.bakhsh,
                    CityType = IranCityType.RuralSection
                });
            return result;
        }

        /// <summary>
        /// Map Abadi fields to <see cref="IranCity"/>
        /// تبدیل اطلاعات فایل آبادی به کلاس <see cref="IranCity"/>
        /// </summary>
        /// <param name="data">Abadi data</param>
        /// <returns>List of <see cref="IranCity"/></returns>
        public static IList<IranCity> ToIranCity(this IEnumerable<IranCityAbadi> data) {
            var result = new List<IranCity>();
            foreach (var item in data)
                result.Add(new IranCity {
                    Id = item.id,
                    Name = item.name,
                    VillageType = item.abadi_type,
                    Diag = item.diag,
                    ProvinceId = item.ostan,
                    CountyId = item.shahrestan,
                    DistrictId = item.bakhsh,
                    RuralSectionId = item.dehestan,
                    AmarCode = item.amar_code,
                    CityType = IranCityType.Village
                });
            return result;
        }
    }
}
