using CitiesWebsite.Models;
using System.Collections;
using System.Collections.Generic;

namespace CitiesWebsite.Services
{
    public class CityProvider : ICityProvider
    {
        private readonly Dictionary<string, City> _cities;

        public CityProvider()
        {
            _cities = CityInitializer();
        }

        public City this[string name] => _cities[name];

        private Dictionary<string, City> CityInitializer()
        {
            var cityList = new Dictionary<string, City>
            {
                { "Madrid", new City("Spain", "Madrid", "UTC +1 (Summer +2)", new CityPopulation(2015, 3141991, 6240000, 6529700)) },
                { "London", new City("England", "London", "UTC +0 (Summer +1)", new CityPopulation(2016, 8787892, 9787426, 14040163)) },
                { "Paris", new City("France", "Paris", "UTC +1 (Summer +2)", new CityPopulation(2015, 2206488, 10601122, 12405426)) }
            };
            return cityList;
        }

        public IEnumerator<KeyValuePair<string, City>> GetEnumerator()
        {
            return _cities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cities.GetEnumerator();
        }
    }
}
