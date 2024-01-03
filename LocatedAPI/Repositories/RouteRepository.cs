using LocatedAPI.Data;
using LocatedAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

        public async Task<List<RouteComplete>> GetAllRoutesByIdListAsync(List<int> idsList)
        {
            try
            {
                if (idsList == null || !idsList.Any())
                {
                    return new List<RouteComplete>();
                }

                var resp = await contexto.Routes
                    .AsNoTracking()
                    .Where(t => idsList.Contains(t.IdTarget))
                    .OrderBy(t => t.Created)
                    .Select(route => new RouteComplete
                    {
                        Id = route.Id,
                        IdTarget = route.IdTarget,
                        IdPerson = route.IdPerson,
                        Color = route.Color,
                        Latitude = route.Latitude,
                        Longitude = route.Longitude,
                        Created = route.Created
                    })
                    .ToListAsync();

                return resp;
            }
            catch (DbException dbEx)
            {
                throw new Exception("Erro no banco de dados ao buscar as rotas.", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as rotas.", ex);
            }
        }

        public async Task<List<TargetRoute>> GetRouteByIdAsync(int idTarget, int personId)
        {
            try
            {
                return await contexto.Routes
                    .AsNoTracking()
                    .Where(p => p.IdTarget == idTarget && p.IdPerson == personId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar o route com o ID {idTarget} para o person com o ID {personId}", ex);
            }
        }

        public async Task<int> SaveRouteAsync(TargetRoute targetRoute)
        {
            try
            {
                await contexto.Routes.AddAsync(targetRoute);
                await contexto.SaveChangesAsync();
                return targetRoute.Id;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao criar a target", ex);
            }
        }

    }
}
