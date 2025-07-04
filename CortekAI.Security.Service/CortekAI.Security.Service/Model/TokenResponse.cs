﻿namespace CortekAI.Security.Service.Model
{
    public class TokenResponse
    {

        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }

}
