using LocatedAPI.Models;
using LocatedAPI.Models.DTO;
using LocatedAPI.Repositories;
using System.Data.SqlTypes;
using System.Security.Claims;

namespace LocatedAPI.Services
{
    public class TargetService : ITargetService
    {
        private readonly ITargetRepository targetRepository;
        private readonly IRouteService routeService;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public TargetService(ITargetRepository targetRepository, IJWTAuthenticationManager jWTAuthenticationManager, IRouteService routeService)
        {
            this.targetRepository = targetRepository;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            this.routeService = routeService;
        }

        public async Task<List<Target>> GetAllTargetsAsync(PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                var targets = await targetRepository.GetAllTargetsAsync(personId);

                return targets;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as pessoas", ex);
            }
        }

        public async Task<DashboardResult> GetAllTargetsToDashboardAsync(PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                var targets = await targetRepository.GetAllTargetsToDashboardAsync(personId);

                List<RouteComplete> routes = new List<RouteComplete>();
                if (targets.Count > 0)
                {
                    var idsList = targets.Select(target => target.Id).ToList();
                    routes = await routeService.GetAllRoutesByIdListAsync(idsList);
                }

                var groupedRoutes = routes.GroupBy(r => r.IdTarget)
                                          .OrderBy(group => group.Key)
                                          .ToList();

                var separatedRoutes = groupedRoutes.Select(group => group.ToList()).ToList();

                List<DashboardData> distances = CalculateDistancesAndSpeeds(separatedRoutes);

                return new DashboardResult
                {
                    SeparatedRoutes = separatedRoutes,
                    Distances = distances
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as pessoas", ex);
            }
        }

        public List<DashboardData> CalculateDistancesAndSpeeds(List<List<RouteComplete>> separatedRoutes)
        {
            List<DashboardData> distances = new List<DashboardData>();

            foreach (var route in separatedRoutes)
            {
                var distanceTraveled = CalculateTheDistanceTraveled(route);
                var averageSpeed = CalculateAverageSpeed(route);

                distances.Add(new DashboardData
                {
                    IdTarget = route.First().IdTarget,
                    Distance = distanceTraveled,
                    AverageSpeed = averageSpeed
                });
            }

            return distances;
        }

        private double CalculateTheDistanceTraveled(List<RouteComplete> route)
        {
            double distance = 0;

            for (int i = 0; i < route.Count - 1; i++)
            {
                double segmentDistance = CalculateDistanceBetweenPoints(route[i].Latitude, route[i].Longitude, route[i + 1].Latitude, route[i + 1].Longitude);
                distance += segmentDistance;
            }

            return distance;
        }

        private double CalculateAverageSpeed(List<RouteComplete> route)
        {
            if (route.Count > 1)
            {
                double totalTimeInSeconds = (route.Last().Created - route.First().Created).TotalSeconds;
                double totalDistanceInKilometers = CalculateTheDistanceTraveled(route);

                double averageSpeedInKilometersPerHour = (totalDistanceInKilometers / totalTimeInSeconds) * 3600;

                return averageSpeedInKilometersPerHour;
            }

            return 0;
        }

        private const double EarthRadiusKm = 6371.0;

        private double CalculateDistanceBetweenPoints(double lat1, double lon1, double lat2, double lon2)
        {
            double radLat1 = ToRadians(lat1);
            double radLon1 = ToRadians(lon1);
            double radLat2 = ToRadians(lat2);
            double radLon2 = ToRadians(lon2);

            double deltaLat = radLat2 - radLat1;
            double deltaLon = radLon2 - radLon1;

            double a = Math.Sin(deltaLat / 2.0) * Math.Sin(deltaLat / 2.0) +
                       Math.Cos(radLat1) * Math.Cos(radLat2) *
                       Math.Sin(deltaLon / 2.0) * Math.Sin(deltaLon / 2.0);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = EarthRadiusKm * c;

            return distance;
        }

        private double ToRadians(double degree)
        {
            return degree * (Math.PI / 180.0);
        }


        public async Task<List<Target>> GetAllTargetsToMapAsync(PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                var targets = await targetRepository.GetAllTargetsToMapAsync(personId);

                if (targets.Count > 0)
                {
                    List<int> targetIds = new List<int>();
                    foreach (var target in targets)
                    {
                        targetIds.Add(target.Id);
                    }

                    var started = await targetRepository.UpdateStartedAsync(targetIds);

                    if (!started)
                    {
                        throw new Exception("Houve um erro ao atualizar para inicializados os marcadores!");
                    }
                }

                return targets;
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro ao buscar todas as pessoas", ex);
            }
        }

        public async Task<Target> GetTargetByIdAsync(int id, PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                Target target = await targetRepository.GetTargetByIdAsync(id, personId);
                if (target == null)
                {
                    return null;
                }

                return target;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {id}", ex);
            }
        }
        
        public async Task<bool> DelTargetByIdAsync(int id, PersonIdentifyReq personIdentify)
        {
            try
            {
                int personId = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int);

                bool isDeleted = await targetRepository.DelTargetByIdAsync(id, personId);

                return isDeleted;
            }
            catch (Exception ex)
            {
                throw new Exception($"Houve um erro ao buscar a pessoa com o ID {id}", ex);
            }
        }

        public async Task<int> CreateTargetAsync(PersonIdentifyReq personIdentify)
        {
            LatitudeLongitudePointsResp objLatLonRandom = await CreateLatitudeLongitudeStartEndPoints();

            Target target = new Target
            {
                IdPerson = int.TryParse(personIdentify.UserId, out int userId) ? userId : default(int),
                LongitudeStart = objLatLonRandom.LongitudeStart,
                LatitudeStart = objLatLonRandom.LatitudeStart,
                LongitudeEnd = objLatLonRandom.LongitudeEnd,
                LatitudeEnd = objLatLonRandom.LatitudeEnd,
                Created = DateTime.UtcNow,
                Color = GetRandomHexColor()
            };

            var resp = await targetRepository.CreateTargetAsync(target);
            return resp;
        }

        private async Task<LatitudeLongitudePointsResp> CreateLatitudeLongitudeStartEndPoints()
        {
            var random = new Random();

            var startPoint = new LatitudeLongitudePointsResp
            {
                LatitudeStart = GetRandomValue(-53.88273, -53.68800, random),
                LongitudeStart = GetRandomValue(-29.74429, -29.66248, random)
            };

            var endPoint = new LatitudeLongitudePointsResp
            {
                LatitudeEnd = GetRandomValue(-53.88273, -53.68800, random),
                LongitudeEnd = GetRandomValue(-29.74429, -29.66248, random)
            };

            return new LatitudeLongitudePointsResp
            {
                LatitudeStart = startPoint.LatitudeStart,
                LongitudeStart = startPoint.LongitudeStart,
                LatitudeEnd = endPoint.LatitudeEnd,
                LongitudeEnd = endPoint.LongitudeEnd
            };
        }

        private double GetRandomValue(double minValue, double maxValue, Random random)
        {
            return minValue + (maxValue - minValue) * random.NextDouble();
        }

        private string GetRandomHexColor()
        {
            var random = new Random();

            byte[] colorBytes = new byte[3];
            random.NextBytes(colorBytes);

            string hexColor = "#" +
                colorBytes[0].ToString("X2") +
                colorBytes[1].ToString("X2") +
                colorBytes[2].ToString("X2");

            return hexColor;
        }
    }
}
