using ShirtStoreWebsite.Models;
using ShirtStoreWebsite.Services;
using System.Collections.Generic;

namespace ShirtStoreWebsite.Tests.FakeRepositories
{
    internal class FakeShirtRepository : IShirtRepository
    {
        public IEnumerable<Shirt> GetShirts() => new List<Shirt>
        {
            new Shirt { Color = ShirtColor.Black, Size = ShirtSize.S, Price = 11F },
            new Shirt { Color = ShirtColor.Gray, Size = ShirtSize.M, Price = 12F },
            new Shirt { Color = ShirtColor.White, Size = ShirtSize.L, Price = 13F }
        };
        public bool AddShirt(Shirt shirt) => true;
        public bool RemoveShirt(int id) => true;
    }
}
