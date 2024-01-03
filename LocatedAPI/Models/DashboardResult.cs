using LocatedAPI.Models;

public class DashboardResult
{
    public List<List<RouteComplete>> SeparatedRoutes { get; set; }
    public List<DashboardData> Distances { get; set; }
}