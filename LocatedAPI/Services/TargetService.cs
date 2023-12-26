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
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        public TargetService(ITargetRepository targetRepository, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.targetRepository = targetRepository;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
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
