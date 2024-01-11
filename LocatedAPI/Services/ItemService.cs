using LocatedAPI.Models;
using LocatedAPI.Models.DTO;
using LocatedAPI.Repositories;
using System.Data.SqlTypes;
using System.Security.Claims;

namespace LocatedAPI.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository itemRepository;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public ItemService(IItemRepository itemRepository, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.itemRepository = itemRepository;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }

        public async Task<int> CreateItemAsync(ItemReq itemReq, PersonIdentifyReq personIdentify)
        {
            Item item = new Item();

            item.IdPerson = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);
            item.Name = itemReq.Name;
            item.Created = DateTime.Now;
            item.Updated = DateTime.Now;

            var resp = await itemRepository.CreateItemAsync(item);
            return resp;
        }
    }
}
