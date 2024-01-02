namespace LocatedAPI.Models
{
    public class Target
    {
        public int Id { get; set; }
        public double LatitudeStart { get; set; }
        public double LongitudeStart { get; set; }
        public double LatitudeEnd { get; set; }
        public double LongitudeEnd { get; set; }
        public int IdPerson { get; set; }
        public string Color { get; set; }
        public bool Started { get; set; }
        public DateTime Created { get; set; }
    }
}
