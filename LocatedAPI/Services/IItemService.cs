using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Services
{
    public interface IItemService
    {
        Task<int> CreateItemAsync(ItemReq itemReq, PersonIdentifyReq personIdentify);
    }

}
