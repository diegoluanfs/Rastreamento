namespace LocatedAPI.Models.DTO
{
    public struct PersonSignUpReq
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public PersonSignUpReq(PersonSignUpReq personSignUpReq)
        {
            Username = personSignUpReq.Username;
            Email = personSignUpReq.Email;
            Password = personSignUpReq.Password;
        }
    }
}
