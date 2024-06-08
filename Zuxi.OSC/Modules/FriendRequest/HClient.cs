using System.Net;
using System.Text;
using Zuxi.OSC.utility;

namespace Zuxi.OSC.Modules.FriendRequests
{
    internal class HClient
    {

        public HttpClient AHClient { get; set; }

        public HClient()
        {
            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true
            };

            AHClient = new HttpClient(httpClientHandler);
            // Add the cookie to the CookieContainer
            httpClientHandler.CookieContainer.Add(new Cookie("auth", Config.AuthCookie) { Domain = "api.vrchat.cloud", Path = "/" });
            httpClientHandler.CookieContainer.Add(new Cookie("twoFactorAuth", Config.twoFactorAuthCookie) { Domain = "api.vrchat.cloud", Path = "/" });
            AHClient.DefaultRequestHeaders.UserAgent.ParseAdd("ZuxiJapi%2F4.0.0%20vrchat%40mail.imzuxi.com");

        }

        public string CheckAuthStatus() => MakeAPIGetRequest("auth");
        public string CrawlForUserInfo() => MakeAPIGetRequest("auth/user");
        public string GetUserNotis() => MakeAPIGetRequest("auth/user/notifications");

        public bool AcceptRequest(string frid)
        {
            if (!frid.ToLower().Contains("frq_"))
                return false;

            if (MakeAPIPutRequest($"auth/user/notifications/{frid}/accept", null) != "[]")
                return true;

            return false;
        }

        public string GetVRCUserByID(string userId) => MakeAPIGetRequest($"users/{userId}");

        public string MakeAPIGetRequest(string apiendpoint)
        {
            string Output = "[]";
            string url = "https://api.vrchat.cloud/api/1/" + apiendpoint; // Replace this with the URL you want to request


            var response = this.AHClient.GetAsync(url).Result; // Synchronous call (you may use await/async in asynchronous context)

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                // Successful response
                string content = response.Content.ReadAsStringAsync().Result; // Synchronous read (you may use await/async in asynchronous context)

                Output = content;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Made Request to: {0} ", apiendpoint);
                Console.ForegroundColor = ConsoleColor.Cyan;

                //    Console.WriteLine("Response content: " + content);
            }
            else
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Failed Request to: {0} ", apiendpoint);

                Console.WriteLine("Request failed with status code: " + response.StatusCode);



                string content = response.Content.ReadAsStringAsync().Result; // Synchronous read (you may use await/async in asynchronous context)
                Console.WriteLine("Response content: " + content);

                Console.ForegroundColor = ConsoleColor.Cyan;
                // Handle unsuccessful response

            }

            return Output;

        }

        public string MakeAPIPutRequest(string apiendpoint, string data)
        {
            string Output = "[]";
            string url = "https://api.vrchat.cloud/api/1/" + apiendpoint; // Replace this with the URL you want to request

            StringContent SendData = null;

            if (!string.IsNullOrEmpty(data))
            {
                SendData = new StringContent(data, Encoding.UTF8, "application/json");
            }

            var response = this.AHClient.PutAsync(url, SendData).Result; // Synchronous call (you may use await/async in asynchronous context)

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                // Successful response
                string content = response.Content.ReadAsStringAsync().Result; // Synchronous read (you may use await/async in asynchronous context)

                Output = content;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successfully Made Request to: {0} ", apiendpoint);
                Console.ForegroundColor = ConsoleColor.Cyan;

                ///     Console.WriteLine("Response content: " + content);
            }
            else
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed Request to: {0} ", apiendpoint);

                Console.WriteLine("Request failed with status code: " + response.StatusCode);



                string content = response.Content.ReadAsStringAsync().Result; // Synchronous read (you may use await/async in asynchronous context)
                Console.WriteLine("Response content: " + content);

                Console.ForegroundColor = ConsoleColor.Cyan;
                // Handle unsuccessful response

            }

            return Output;

        }

    }
}

