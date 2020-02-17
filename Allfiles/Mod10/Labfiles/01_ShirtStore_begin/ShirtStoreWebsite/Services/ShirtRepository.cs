using ShirtStoreWebsite.Data;
using ShirtStoreWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShirtStoreWebsite.Services
{
    public class ShirtRepository : IShirtRepository
    {
        private readonly ShirtContext _context;

        public ShirtRepository(ShirtContext context)
        {
            _context = context;
        }

        public IEnumerable<Shirt> GetShirts()
        {
            return _context.Shirts.ToList();
        }

        public bool AddShirt(Shirt shirt)
        {
            _context.Add(shirt);
            int entries = _context.SaveChanges();
            return entries > 0;
        }

        public bool RemoveShirt(int id)
        {
            _context.Remove(_context.Shirts.FirstOrDefault(s => s.Id == id));
            int entries = _context.SaveChanges();
            return entries > 0;
        }
    }
}
