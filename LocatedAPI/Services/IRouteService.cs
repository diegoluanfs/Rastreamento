using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Services
{
    public interface IRouteService
    {
        Task<List<TargetRoute>> GetAllRoutesAsync(PersonIdentifyReq personIdentify);
        Task<List<TargetRoute>> GetRouteByIdAsync(int id, PersonIdentifyReq personIdentify);
        Task<int> SaveRouteAsync(PersonIdentifyReq personIdentify, RouteTargetReq routeTargetReq);
    }

}
