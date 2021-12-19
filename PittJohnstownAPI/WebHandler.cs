using System.Net;

namespace PittJohnstownAPI
{
    public class WebHandler
    {
        private static WebHandler _handler;
        private static readonly object LockObject = new();

        private WebHandler()
        {

        }

        public static WebHandler GetInstance()
        {
            if (_handler == null)
            {
                lock (LockObject)
                {
                    if (_handler == null)
                    {
                        return _handler = new WebHandler();
                    }
                }
            }
            return _handler;
        }

        public static async Task<string> GetWebsiteContent(string url)
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync(url);
            return content;
        }

        public async Task<bool> CheckRedirects(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.AllowAutoRedirect = true;
            var response = await request.GetResponseAsync();

            return !response.ResponseUri.ToString().Equals(url, StringComparison.OrdinalIgnoreCase);

        }
    }
}
