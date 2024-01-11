namespace LocatedAPI.Models
{
    public class Item
    {
        public int Id { get; set; }
        public int IdPerson { get; set; }
        public string Name { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public Item() { }

        public Item(int id, int idPerson, string name, double? latitude, double? longitude, DateTime created, DateTime updated)
        {
            Id = id;
            IdPerson = idPerson;
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            Created = created;
            Updated = updated;
        }
    }
}
