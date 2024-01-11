using LocatedAPI.Data;
using LocatedAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatedAPI.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly Contexto contexto;

        public ItemRepository(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<int> CreateItemAsync(Item item)
        {
            try
            {
                await contexto.Items.AddAsync(item);
                await contexto.SaveChangesAsync();
                return item.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao criar a item", ex);
            }
        }

    }
}
