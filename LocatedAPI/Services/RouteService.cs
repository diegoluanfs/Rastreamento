using LocatedAPI.Models;
using LocatedAPI.Models.DTO;
using LocatedAPI.Repositories;
using System.Data.SqlTypes;
using System.Security.Claims;

namespace LocatedAPI.Services
{
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository routeRepository;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public RouteService(IRouteRepository routeRepository, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.routeRepository = routeRepository;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }

        public async Task<List<TargetRoute>> GetAllRoutesAsync(PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                var routes = await routeRepository.GetAllRoutesAsync(personId);

                return routes;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as pessoas", ex);
            }
        }
        
        public async Task<List<RouteComplete>> GetAllRoutesByIdListAsync(List<int> idsList)
        {
            try
            {
                var routes = await routeRepository.GetAllRoutesByIdListAsync(idsList);

                return routes;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as pessoas", ex);
            }
        }

        public async Task<List<TargetRoute>> GetRouteByIdAsync(int idTarget, PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                var route = await routeRepository.GetRouteByIdAsync(idTarget, personId);
                
                return route;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {idTarget}", ex);
            }
        }

        public async Task<int> SaveRouteAsync(PersonIdentifyReq personIdentify, RouteTargetReq routeTargetReq)
        {
            var targetRoute = new TargetRoute();

            targetRoute.IdPerson = int.TryParse(personIdentify.UserId, out int userId) ? userId : 0;
            targetRoute.IdTarget = routeTargetReq.IdTarget ?? 0;
            targetRoute.Color = routeTargetReq.Color;
            targetRoute.Latitude = routeTargetReq.Latitude ?? 0;
            targetRoute.Longitude = routeTargetReq.Longitude ?? 0;
            targetRoute.Created = DateTime.UtcNow;

            var resp = await routeRepository.SaveRouteAsync(targetRoute);
            return resp;
        }

    }
}
