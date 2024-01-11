using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Repositories
{
    public interface IItemRepository
    {
        Task<int> CreateItemAsync(Item item);
    }

}
