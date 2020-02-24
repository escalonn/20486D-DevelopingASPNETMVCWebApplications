using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Underwater.Data;
using Underwater.Models;

namespace Underwater.Repositories
{
    public class UnderwaterRepository : IUnderwaterRepository
    {
        private readonly UnderwaterContext _context;
        private readonly IConfiguration _configuration;
        private readonly CloudBlobContainer _container;

        public UnderwaterRepository(UnderwaterContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("AzureStorageConnectionString-1");
            var containerName = _configuration.GetValue<string>("ContainerSettings:ContainerName");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
        }

        public async Task<IEnumerable<Fish>> GetFishesAsync()
        {
            return await _context.Fishes.ToListAsync();
        }

        public async Task<Fish> GetFishByIdAsync(int id)
        {
            return await _context.Fishes.Include(a => a.Aquarium)
                .FirstOrDefaultAsync(f => f.FishId == id);
        }

        public async Task AddFishAsync(Fish fish)
        {
            if (fish.PhotoAvatar != null && fish.PhotoAvatar.Length > 0)
            {
                string imageUrl = await UploadImageAsync(fish.PhotoAvatar);
                fish.ImageUrl = imageUrl;
                fish.ImageMimeType = fish.PhotoAvatar.ContentType;
                fish.ImageName = Path.GetFileName(fish.PhotoAvatar.FileName);
                await _context.AddAsync(fish);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFishAsync(int id)
        {
            Fish fish = await _context.Fishes.FirstOrDefaultAsync(f => f.FishId == id);
            if (fish.ImageUrl != null)
            {
                await DeleteImageAsync(fish.ImageName);
            }
            _context.Fishes.Remove(fish);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<Aquarium> GetAquariums()
        {
            var aquariumsQuery = _context.Aquariums.OrderBy(a => a.Name);
            return aquariumsQuery;
        }

        private async Task<string> UploadImageAsync(IFormFile photo)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(Path.GetFileName(photo.FileName));
            using (Stream source = photo.OpenReadStream())
            {
                await blob.UploadFromStreamAsync(source);
            }
            return blob.Uri.ToString();
        }

        private async Task<bool> DeleteImageAsync(string photoFileName)
        {
            CloudBlockBlob blob = _container.GetBlockBlobReference(photoFileName);
            await blob.DeleteAsync();
            return true;
        }
    }
}
