namespace PittJohnstownAPI
{
    public class WebHandler
    {
        private static  WebHandler Handler = null;
        private static readonly object LockObject = new object();

        private WebHandler()
        {

        }

        public static WebHandler GetInstance()
        {
            if (Handler == null)
            {
                lock (LockObject)
                {
                    if (Handler == null)
                    {
                        return Handler = new WebHandler();
                    }
                }
            }
            return Handler;
        }

        async public Task<string> GetWebsiteContent(string url)
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync(url);
            return content;
        }
    }
}
