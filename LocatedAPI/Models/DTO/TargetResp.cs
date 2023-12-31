﻿namespace LocatedAPI.Models.DTO
{
    public class TargetResp
    {
        public int Id { get; set; }
        public double LatitudeStart { get; set; }
        public double LongitudeStart { get; set; }
        public double LatitudeEnd { get; set; }
        public double LongitudeEnd { get; set; }
        public int IdPerson { get; set; }
        public string Color { get; set; }
        public DateTime Created { get; set; }
    }
}
