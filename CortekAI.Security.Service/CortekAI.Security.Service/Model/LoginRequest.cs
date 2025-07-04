namespace CortekAI.Security.Service.Model
{
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public Options options { get; set; }
    }

    public class Options
    {
        public bool multiOptionalFactorEnroll { get; set; }
        public bool warnBeforePasswordExpired { get; set; }
    }

}
