using System.Collections.Generic;

namespace LocatedAPI.Models.DTO
{
    public class RouteTargetReq
    {
        public int? Id { get; set; }
        public int? IdTarget { get; set; }
        public string? Color { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set;}
    }

}
