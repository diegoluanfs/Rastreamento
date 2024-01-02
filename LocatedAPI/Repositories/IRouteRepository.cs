using LocatedAPI.Models;
using LocatedAPI.Models.DTO;

namespace LocatedAPI.Repositories
{
    public interface IRouteRepository
    {
        Task<List<TargetRoute>> GetAllRoutesAsync(int personId);
        Task<List<TargetRoute>> GetRouteByIdAsync(int id, int personId);
        Task<int> SaveRouteAsync(TargetRoute targetRoute);
    }

}
