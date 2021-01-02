using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using CsvHelper;
using Iran.Core.Locations.Internal;
using Iran.Core.Extensions;

namespace Iran.Core.Locations {

    public class IranCityProvider {

        private const string _RES_Provinces_Path = "Resource/ostan.csv";
        private const string _RES_Counties_Path = "Resource/shahrestan.csv";
        private const string _RES_Districts_Path = "Resource/bakhsh.csv";
        private const string _RES_Cities_Path = "Resource/shahr.csv";
        private const string _RES_RuralSections_Path = "Resource/dehestan.csv";
        private const string _RES_Village_Path = "Resource/abadi.csv";

        public IEnumerable<IranCity> GetIranProvinces()
            => getFromCsv<IranCityOstan>(_RES_Provinces_Path).ToIranCity();

        public async Task<IEnumerable<IranCity>> GetIranProvincesAsync()
            => (await getFromCsvAsync<IranCityOstan>(_RES_Provinces_Path)).ToIranCity();

        public IEnumerable<IranCity> GetIranCounties()
            => getFromCsv<IranCityShahrestan>(_RES_Counties_Path).ToIranCity();

        public async Task<IEnumerable<IranCity>> GetIranCountiesAsync()
            => (await getFromCsvAsync<IranCityShahrestan>(_RES_Counties_Path)).ToIranCity();

        public IEnumerable<IranCity> GetIranDistricts()
            => getFromCsv<IranCityBakhsh>(_RES_Districts_Path).ToIranCity();

        public async Task<IEnumerable<IranCity>> GetIranDistrictsAsync()
            => (await getFromCsvAsync<IranCityBakhsh>(_RES_Districts_Path)).ToIranCity();

        public IEnumerable<IranCity> GetIranCities()
            => getFromCsv<IranCityShahr>(_RES_Cities_Path).ToIranCity();

        public async Task<IEnumerable<IranCity>> GetIranCitiesAsync()
            => (await getFromCsvAsync<IranCityShahr>(_RES_Cities_Path)).ToIranCity();

        public IEnumerable<IranCity> GetIranRuralSections()
            => getFromCsv<IranCityDehestan>(_RES_RuralSections_Path).ToIranCity();

        public async Task<IEnumerable<IranCity>> GetIranRuralSectionsAsync()
            => (await getFromCsvAsync<IranCityDehestan>(_RES_RuralSections_Path)).ToIranCity();

        public IEnumerable<IranCity> GetIranVillages()
            => getFromCsv<IranCityAbadi>(_RES_Village_Path).ToIranCity();

        public async Task<IEnumerable<IranCity>> GetIranVillagesAsync()
            => (await getFromCsvAsync<IranCityAbadi>(_RES_Village_Path)).ToIranCity();

        private IEnumerable<T> getFromCsv<T>(string resourcePath) where T: class {
            var resourceStream = ResourceHelper.GetResourceStream(resourcePath);
            using var reader = new StreamReader(resourceStream);
            using var csv = new CsvReader(reader, culture: CultureInfo.InvariantCulture);
            return csv.GetRecords<T>();
        }

        private async Task<IEnumerable<T>> getFromCsvAsync<T>(string resourcePath) where T: class {
            var resourceStream = ResourceHelper.GetResourceStream(resourcePath);
            using var reader = new StreamReader(resourceStream);
            using var csv = new CsvReader(reader, culture: CultureInfo.InvariantCulture);
            return await csv
                .GetRecordsAsync<T>()
                .ToListAsync<T>();
        }
    }
}
