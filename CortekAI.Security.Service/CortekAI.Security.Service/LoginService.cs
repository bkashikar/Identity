using CortekAI.Security.Service.Model;
using Microsoft.AspNetCore.Identity.Data;
using Newtonsoft.Json;

namespace CortekAI.Security.Service
{
    public class LoginService
    {
        public async Task<TokenResponse> GenerateSession(Model.LoginRequest loginRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://trial-7235689.okta.com/api/v1/authn");
            //request.Headers.Add("Cookie", "DT=DI109D7SuanS5OI1R_3Z9nhEQ; JSESSIONID=BD19E6FC7DA72F5315ED9A6E1A17B441; idx=eyJ6aXAiOiJERUYiLCJ2ZXIiOiIxIiwiYWxpYXMiOiJlbmNyeXB0aW9ua2V5Iiwib2lkIjoiMDBvc3NzZHB5bkVvU1hjcUg2OTciLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiZGlyIiwieHNpZCI6ImlkeHBYMXo0bWZMUVhHdDhvQjFvMXZ3YVEifQ..gDrs-hQt1h_8xHyN.1CaGcYcldBP4hlUvmGXgFIiJKvp_h01iaxuFCXizK7d6nSG4qOEKPct4uq-xAgSr-UwP0a72_XenQR9zl89vicdAWCUJUEDgAXnYcmGFCXM2-cdsMGOo2cFh9dSXCN9z_8l1ddLK4kHpaiIofs4KvViDVXg-eOu6if-s_SCItL9lI50IG_mpiFRf_ss7YGqNu-r83JiR_wsKK71kQ5qn3OAh6RUFLcMYegOMODFkS2NLo9fJi_LUHlHE-s1AaIq66zzWjixgHgMFlpi8b_Ad-GVlW4jUuqxkXO_OxfpIfDua4hBkAvk6Md6eRr4tFcrFkn580OTHO5kJyvmtxgXoIMsJxa_WOPiJ0-6OBQJeDeyFaKNqkOcLmwcUG-zpERui3EldeAhy2bf25-V2G7SCJ5GWkGYp5pnz5v5CnUS_5PtDj_Ww7sd_k1I3q-z4avbu8DcQ1Mb19-vYGJl6OXztpkd1gZJ5bpyPNPwXNrLm03HM_NRrpOF38gyCJIh2rfT9V4JcII3cix5ZopdpU2MFLI4QM1h3641c8KdKDefM2b8G6KONkRD0h7j8Ktpx4kORZ8dYLGabVu27oTs6U8QD2NMTHgn7ND1Ujpix4lHjBEwrn75dIeAgCM6sxN2x2FKByj9JuT9UcYRwus2Kp0qykrj_c_5WYRgrKnESePdoj73mZhfEK9XNoRhHD0p_09qDDnLHH1wxTSVEw3AG1Aik9A9meF5mGtLdPa9VvkeljVquSIW2MURBQQXWWi7B1tL-PWDFa4jsM5F2_ZrBINpe6UQzL2sIa2z8ahMML2P2FPPLxMNErWDo5e0kSfKArST3IrPUCVK5wCH3DOBsZomiAwFXKsvrX-zgD5b6oiHyJvGV7d9_1xmI4ZDpnTPQGiz94im02WVQaPU4-eVbq8Bk_98UGtuEfqgHH29RQCpQZijFhSEHohmwNeM-PALMRWz0bctjR7h1xqZSiC3lFw.tW9ZOIsHmHSQJbEOC1Maqw; proximity_c21836b64e017ecfc46898c9ca15cfb4=eyJ6aXAiOiJERUYiLCJwMnMiOiJJLWNUTTRyMWpFYXRfTHpoaEpDbTFBIiwicDJjIjoxMDAwLCJ2ZXIiOiIxIiwiZW5jIjoiQTI1NkdDTSIsImFsZyI6IlBCRVMyLUhTNTEyK0EyNTZLVyJ9.Il7IjlEedaM0XtMisnwIW0vbXhPdxcktquW00OpnQPxZCUu_KN5u5A.0hlAnC59VMmAMNOb.BNR49FQ5b6Qkr1PhyctYV4d4xuWDBYaxJueCzJHjANovXTE3cjjL7FDh-gocgsjAZOKVTRW3oGTT1aOnQ4UBnuNLSlIOluSXoWp-YqHo_Q92uqA5zokVUXqY3a0uIGNJjTSPvHd87g_JAi3NWBNSqu4zVLlOU-D3NvLcn_6GXS8blA.ZSaUtm7ewHtNjuYvdumeqw; sid=10219gC5rPXRSyEeUetXUWQ2g; xids=102tVRBVuYvRXG_XnyyX-_toA");
            string loginR = JsonConvert.SerializeObject(loginRequest);
            var content = new StringContent(loginR, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            AuthnResponse authnResponse = JsonConvert.DeserializeObject<AuthnResponse>(await response.Content.ReadAsStringAsync());

            var code = await GenerateAuthCode(authnResponse.sessionToken);
            if (string.IsNullOrEmpty(code)) {
                throw new Exception("Unable to Login");
            }

            return await GetToken(code);

        }

        public async Task<string?> GenerateAuthCode(string sessionToken)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://trial-7235689.okta.com/oauth2/aussxltazyM8uXG6C697/v1/authorize?client_id=0oasu49ja0Rwjm70n697&code_challenge=8tCIHBTgy_e31xpAe7tKJ_LmgQHbMrXNFfMWo-rQ7pI&code_challenge_method=S256&redirect_uri=com.okta.trial-7235689:/callback&response_type=code&scope=openid offline_access&sessionToken={sessionToken}&state=CortekIOTCloud");
            // request.Headers.Add("Cookie", "DT=DI109D7SuanS5OI1R_3Z9nhEQ; JSESSIONID=C33F6C3828C1A21BCA5E963E8C211DE1; idx=eyJ6aXAiOiJERUYiLCJ2ZXIiOiIxIiwiYWxpYXMiOiJlbmNyeXB0aW9ua2V5Iiwib2lkIjoiMDBvc3NzZHB5bkVvU1hjcUg2OTciLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiZGlyIiwieHNpZCI6ImlkeDFMb1ZtTkQwUXB5dmFCS21KYXVOeWcifQ..eUHn86c2gxs8RDLb.vRFfUc2AHu7cYwzkxugeeDW3evxIPh6Tv0TQDT8zBuLzPh7QWkbPrq9Y3qFZKqQwu6axGty3J_RXKGqo3dFR8Ic81yAzwrpNxCowkd5zpNO8JHM9H9dnP5O21L-44l_dkOMUzkiYbsL6-df9k46PCuqtv9vm2UTVC3xfgiVNpqo1eqrklllsKqimmvhw8uIl0lQou3bV6OJy4gHoh441oU3dPpzVxWt9aLKQzejaWXLr0GsPoEfXpDOsbRHZ3R7HNO7BJM8NZCxDgOjFXQ_jCuRVP9ifmqRnHty838OTrgHx9Vuqfk-c7t-96fkWI8W4JJTdfURK87h5__e8BKY3ObX5drwdpXYrNZjR6zSegqRYZHDatMI5yLbLEogzZi17oOb4lZUr7xp55js5g_XYRs6qS6OtuC9tA9JYRseEmE3nkHdRgafPsmZYWlxSkd7UKrtIdc4FTbkH8sCO7857yy8HxPuSfegxR-eSc-1p0T4aPbARQbEdp-ji64YBgh80zr09eWUy0hg3l-FPtg-264l1aMrFslTfZvnen5kX9MIAl-pyj0QRCj7vCfr8KJLD9W1cqJ22E45oaLsN-guxfGW9p4HBJZ_nI4Q2bSGe7-CwgRi_KZCLvSttknCX7A2lTrtw4gaeP5cI93ChLT303FtWe9KQZ1aDSu1pmNyjepNhGsHe8awHthsyQWTR7DcLxvUfC_gTDf5hMh6MXv-WvdDohbeOEURAI8othAW6VdYsXK0WRTgUTnFCtIZhyYO5Ii3AyNi6yk49ggm1Ti7uvILDMaZMtNOYyBzJkMdulb6G7Ylhy6sCA_-DlH-mkBs8n7PZrjWZsMeHzeGd3BpEvq9ggTA2tBuSJAcJ9eMBhe3BbKaZlo4kGIx1eNtiJX1_F2QICy63hE-kcY5G071-ftnLw525EDMca6nXl9NqqCLNjGmhfWKERnGj3F-vID9Q6i43BnvBLH5dKwpNfA.TUuBZQ0yiNzzwE6HiblmAg; proximity_c21836b64e017ecfc46898c9ca15cfb4=eyJ6aXAiOiJERUYiLCJwMnMiOiJJLWNUTTRyMWpFYXRfTHpoaEpDbTFBIiwicDJjIjoxMDAwLCJ2ZXIiOiIxIiwiZW5jIjoiQTI1NkdDTSIsImFsZyI6IlBCRVMyLUhTNTEyK0EyNTZLVyJ9.Il7IjlEedaM0XtMisnwIW0vbXhPdxcktquW00OpnQPxZCUu_KN5u5A.0hlAnC59VMmAMNOb.BNR49FQ5b6Qkr1PhyctYV4d4xuWDBYaxJueCzJHjANovXTE3cjjL7FDh-gocgsjAZOKVTRW3oGTT1aOnQ4UBnuNLSlIOluSXoWp-YqHo_Q92uqA5zokVUXqY3a0uIGNJjTSPvHd87g_JAi3NWBNSqu4zVLlOU-D3NvLcn_6GXS8blA.ZSaUtm7ewHtNjuYvdumeqw; sid=10219gC5rPXRSyEeUetXUWQ2g; xids=102tVRBVuYvRXG_XnyyX-_toA");
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Redirect)
            {
                if (response.Headers.Any(x => x.Key == "Location") && response.Headers.Contains("Location"))
                {
                    string? locationHeader = response.Headers.GetValues("Location").FirstOrDefault();
                    string? code = (locationHeader?.Split("code=")[1].Split("&")[0]);
                    return code;
                }

            }
            return null;

        }

        public async Task<TokenResponse> GetToken(string code)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://trial-7235689.okta.com/oauth2/aussxltazyM8uXG6C697/v1/token");
            request.Headers.Add("Accept", "application/json");
            //request.Headers.Add("Cookie", "DT=DI109D7SuanS5OI1R_3Z9nhEQ; JSESSIONID=6BCEE7E438EA75C03AC1C14F2DC0B757; idx=eyJ6aXAiOiJERUYiLCJ2ZXIiOiIxIiwiYWxpYXMiOiJlbmNyeXB0aW9ua2V5Iiwib2lkIjoiMDBvc3NzZHB5bkVvU1hjcUg2OTciLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiZGlyIiwieHNpZCI6ImlkeDFMb1ZtTkQwUXB5dmFCS21KYXVOeWcifQ..eUHn86c2gxs8RDLb.vRFfUc2AHu7cYwzkxugeeDW3evxIPh6Tv0TQDT8zBuLzPh7QWkbPrq9Y3qFZKqQwu6axGty3J_RXKGqo3dFR8Ic81yAzwrpNxCowkd5zpNO8JHM9H9dnP5O21L-44l_dkOMUzkiYbsL6-df9k46PCuqtv9vm2UTVC3xfgiVNpqo1eqrklllsKqimmvhw8uIl0lQou3bV6OJy4gHoh441oU3dPpzVxWt9aLKQzejaWXLr0GsPoEfXpDOsbRHZ3R7HNO7BJM8NZCxDgOjFXQ_jCuRVP9ifmqRnHty838OTrgHx9Vuqfk-c7t-96fkWI8W4JJTdfURK87h5__e8BKY3ObX5drwdpXYrNZjR6zSegqRYZHDatMI5yLbLEogzZi17oOb4lZUr7xp55js5g_XYRs6qS6OtuC9tA9JYRseEmE3nkHdRgafPsmZYWlxSkd7UKrtIdc4FTbkH8sCO7857yy8HxPuSfegxR-eSc-1p0T4aPbARQbEdp-ji64YBgh80zr09eWUy0hg3l-FPtg-264l1aMrFslTfZvnen5kX9MIAl-pyj0QRCj7vCfr8KJLD9W1cqJ22E45oaLsN-guxfGW9p4HBJZ_nI4Q2bSGe7-CwgRi_KZCLvSttknCX7A2lTrtw4gaeP5cI93ChLT303FtWe9KQZ1aDSu1pmNyjepNhGsHe8awHthsyQWTR7DcLxvUfC_gTDf5hMh6MXv-WvdDohbeOEURAI8othAW6VdYsXK0WRTgUTnFCtIZhyYO5Ii3AyNi6yk49ggm1Ti7uvILDMaZMtNOYyBzJkMdulb6G7Ylhy6sCA_-DlH-mkBs8n7PZrjWZsMeHzeGd3BpEvq9ggTA2tBuSJAcJ9eMBhe3BbKaZlo4kGIx1eNtiJX1_F2QICy63hE-kcY5G071-ftnLw525EDMca6nXl9NqqCLNjGmhfWKERnGj3F-vID9Q6i43BnvBLH5dKwpNfA.TUuBZQ0yiNzzwE6HiblmAg; proximity_c21836b64e017ecfc46898c9ca15cfb4=eyJ6aXAiOiJERUYiLCJwMnMiOiJJLWNUTTRyMWpFYXRfTHpoaEpDbTFBIiwicDJjIjoxMDAwLCJ2ZXIiOiIxIiwiZW5jIjoiQTI1NkdDTSIsImFsZyI6IlBCRVMyLUhTNTEyK0EyNTZLVyJ9.Il7IjlEedaM0XtMisnwIW0vbXhPdxcktquW00OpnQPxZCUu_KN5u5A.0hlAnC59VMmAMNOb.BNR49FQ5b6Qkr1PhyctYV4d4xuWDBYaxJueCzJHjANovXTE3cjjL7FDh-gocgsjAZOKVTRW3oGTT1aOnQ4UBnuNLSlIOluSXoWp-YqHo_Q92uqA5zokVUXqY3a0uIGNJjTSPvHd87g_JAi3NWBNSqu4zVLlOU-D3NvLcn_6GXS8blA.ZSaUtm7ewHtNjuYvdumeqw; sid=10219gC5rPXRSyEeUetXUWQ2g; xids=102tVRBVuYvRXG_XnyyX-_toA");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("grant_type", "authorization_code"));
            collection.Add(new("redirect_uri", "com.okta.trial-7235689:/callback"));
            collection.Add(new("scope", "openid"));
            collection.Add(new("code_verifier", "Z2NWJe1hYLMMXwGAe3ev9NKn8r3QmXE1k6fnvzz126kQGVlRnxrGwGJomkchV1OYfBb230"));
            collection.Add(new("client_secret", "fnPNT9Q066bbeKB3zEUN_0qQ0Ybj5H3pLUYYsSg9D0fgxVqf5Q2F8ApUuPRe5asq"));
            collection.Add(new("client_id", "0oasu49ja0Rwjm70n697"));
            collection.Add(new("code", code));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(await response.Content.ReadAsStringAsync());

            return tokenResponse;

        }
    }
}
