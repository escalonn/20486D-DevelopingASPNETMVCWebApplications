using CitiesWebsite.Models;
using System.Collections.Generic;

namespace CitiesWebsite.Services
{
    public interface ICityProvider : IEnumerable<KeyValuePair<string, City>>
    {
        City this[string name] { get; }
    }
}
