using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Underwater.Models;

namespace Underwater.Repositories
{
    public interface IUnderwaterRepository
    {
        Task<IEnumerable<Fish>> GetFishesAsync();
        Task<Fish> GetFishByIdAsync(int id);
        Task AddFishAsync(Fish fish);
        Task RemoveFishAsync(int id);
        Task SaveChangesAsync();
        IQueryable<Aquarium> GetAquariums();
    }
}
