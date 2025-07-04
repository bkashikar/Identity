namespace CortekAI.Security.Service.Model
{
    public class AuthnResponse
    {
        public DateTime expiresAt { get; set; }
        public string status { get; set; }
        public string sessionToken { get; set; }
        public _Embedded _embedded { get; set; }
        public _Links _links { get; set; }
    }

    public class _Embedded
    {
        public User user { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public DateTime passwordChanged { get; set; }
        public Profile profile { get; set; }
    }

    public class Profile
    {
        public string login { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string locale { get; set; }
        public string timeZone { get; set; }
    }

    public class _Links
    {
        public Cancel cancel { get; set; }
    }

    public class Cancel
    {
        public string href { get; set; }
        public Hints hints { get; set; }
    }

    public class Hints
    {
        public string[] allow { get; set; }
    }

}
