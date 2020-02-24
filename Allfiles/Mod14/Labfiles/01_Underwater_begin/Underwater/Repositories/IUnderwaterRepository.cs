using System.Collections.Generic;
using System.Linq;
using Underwater.Models;

namespace Underwater.Repositories
{
    public interface IUnderwaterRepository
    {
        IEnumerable<Fish> GetFishes();
        Fish GetFishById(int id);
        void AddFish(Fish fish);
        void RemoveFish(int id);
        void SaveChanges();
        IQueryable<Aquarium> GetAquariums();
    }
}
