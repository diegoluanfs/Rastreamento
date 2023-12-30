using LocatedAPI.Data;
using LocatedAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatedAPI.Repositories
{
    public class RouteRepository : IRouteRepository
    {
        private readonly Contexto contexto;

        public RouteRepository(Contexto contexto)
        {
            this.contexto = contexto;
        }

        public async Task<List<TargetRoute>> GetAllRoutesAsync(int personId)
        {
            try
            {
                return await contexto.Routes
                    .AsNoTracking()
                    .Where(t => t.IdPerson == personId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar todos os routes para o person com o ID {personId}", ex);
            }
        }

        public async Task<TargetRoute> GetRouteByIdAsync(int id, int personId)
        {
            try
            {
                return await contexto.Routes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.IdPerson == id && p.IdPerson == personId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar o route com o ID {id} para o person com o ID {personId}", ex);
            }
        }
    }
}
