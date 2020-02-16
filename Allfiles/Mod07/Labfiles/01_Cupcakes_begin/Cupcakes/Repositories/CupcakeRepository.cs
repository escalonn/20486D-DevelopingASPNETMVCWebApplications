using Cupcakes.Data;
using Cupcakes.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cupcakes.Repositories
{
    public class CupcakeRepository : ICupcakeRepository
    {
        private readonly CupcakeContext _context;

        public CupcakeRepository(CupcakeContext cupcakeContext)
        {
            _context = cupcakeContext;
        }

        public IEnumerable<Cupcake> GetCupcakes()
        {
            return _context.Cupcakes.ToList();
        }

        public Cupcake GetCupcakeById(int id)
        {
            return _context.Cupcakes.FirstOrDefault(c => c.CupcakeId == id);
        }

        public void CreateCupcake(Cupcake cupcake)
        {
            if (cupcake.PhotoAvatar?.Length > 0)
            {
                cupcake.ImageMimeType = cupcake.PhotoAvatar.ContentType;
                cupcake.ImageName = cupcake.PhotoAvatar.FileName;
                using (var memoryStream = new MemoryStream())
                {
                    cupcake.PhotoAvatar.CopyTo(memoryStream);
                    cupcake.PhotoFile = memoryStream.ToArray();
                }
            }
            _context.Add(cupcake);
            _context.SaveChanges();
        }

        public void DeleteCupcake(int id)
        {
            var cupcake = _context.Cupcakes.First(c => c.CupcakeId == id);
            _context.Remove(cupcake);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<Bakery> PopulateBakeriesDropDownList()
        {
            var bakeriesQuery = _context.Bakeries.OrderBy(b => b.BakeryName);
            return bakeriesQuery;
        }
    }
}
