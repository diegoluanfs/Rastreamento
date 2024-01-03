namespace LocatedAPI.Models
{
    public class RouteComplete
    {
        public int Id { get; set; }
        public int IdTarget { get; set; }
        public int IdPerson { get; set; }
        public string Color { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Created { get; set; }
    }
}
