using System.Net.Http.Headers;

namespace Event_ui.Util
{
    public static class HttpClientHelper
    {
        public static bool AddAuthorizationHeader(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            var jwt = httpContextAccessor.HttpContext.Request.Cookies["JWT"];

            if (string.IsNullOrEmpty(jwt))
            {
                return false;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            return true;
        }
    }

}
