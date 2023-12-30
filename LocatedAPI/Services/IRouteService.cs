using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Services
{
    public interface IRouteService
    {
        Task<List<TargetRoute>> GetAllRoutesAsync(PersonIdentifyReq personIdentify);
        Task<TargetRoute> GetRouteByIdAsync(int id, PersonIdentifyReq personIdentify);
    }

}
