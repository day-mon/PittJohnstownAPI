using System.Net;
using NLog;

namespace PittJohnstownAPI
{
    public class WebHandler
    {
        private static volatile WebHandler _handler = null!;
        private static readonly object LockObject = new();
        private static readonly HttpClient Client = new();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            try
            {
                var content = await Client.GetStringAsync(url);
                return content;
            }
            catch (Exception exception)
            {
                Logger.Error($"Exception has occured, Base message {exception.Message} \n\n StackTrace: {exception.StackTrace}");
                return string.Empty;
            }
        }

        public static async Task<bool> CheckRedirects(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            request.AllowAutoRedirect = true;
            var response = await request.GetResponseAsync();

            return !response.ResponseUri.ToString().Equals(url, StringComparison.OrdinalIgnoreCase);

        }
    }
}
