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

        public async Task<TargetRoute> GetRouteByIdAsync(int id, PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                TargetRoute route = await routeRepository.GetRouteByIdAsync(id, personId);
                if (route == null)
                {
                    return null;
                }

                return route;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {id}", ex);
            }
        }
        
    }
}
