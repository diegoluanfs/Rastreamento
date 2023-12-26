namespace LocatedAPI.Models.DTO
{
    public class PersonResp
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public PersonResp()
        {

        }
        public PersonResp(Person person)
        {
            Id = person.Id;
            Username = person.Username;
            Email = person.Email;
        }
    }
}
