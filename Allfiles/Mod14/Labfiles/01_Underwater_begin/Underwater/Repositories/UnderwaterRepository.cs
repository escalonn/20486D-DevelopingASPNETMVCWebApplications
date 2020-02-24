using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Underwater.Data;
using Underwater.Models;

namespace Underwater.Repositories
{
    public class UnderwaterRepository : IUnderwaterRepository
    {
        private readonly UnderwaterContext _context;

        public UnderwaterRepository(UnderwaterContext context)
        {
            _context = context;
        }

        public IEnumerable<Fish> GetFishes()
        {
            return _context.Fishes.ToList();
        }

        public Fish GetFishById(int id)
        {
            return _context.Fishes.Include(a => a.Aquarium)
                .FirstOrDefault(f => f.FishId == id);
        }

        public void AddFish(Fish fish)
        {
            if (fish.PhotoAvatar != null && fish.PhotoAvatar.Length > 0)
            {
                fish.ImageMimeType = fish.PhotoAvatar.ContentType;
                fish.ImageName = Path.GetFileName(fish.PhotoAvatar.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    fish.PhotoAvatar.CopyTo(memoryStream);
                    fish.PhotoFile = memoryStream.ToArray();
                }
                _context.Add(fish);
                _context.SaveChanges();
            }
        }

        public void RemoveFish(int id)
        {
            var fish = _context.Fishes.FirstOrDefault(f => f.FishId == id);
            _context.Fishes.Remove(fish);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public IQueryable<Aquarium> GetAquariums()
        {
            var aquariumsQuery = _context.Aquariums.OrderBy(a => a.Name);
            return aquariumsQuery;
        }
    }
}
