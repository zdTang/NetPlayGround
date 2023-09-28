using Microsoft.Identity.Client;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Net_5.CallAPI.GraphAPI
{
    public class TestCallGraph
    {
        public static async Task<string> GetToken()
        {
            string clientId = "85f87650-66e3-4b1d-a3ad-1aeabbaf3000";
            string clientSecret = "sKR8Q~dxuWtrqom7BNcS7JB9DNl6iwrgR.X-Ednd";
            string[] scopes = new[] { "https://graph.microsoft.com/.default" }; // Scope for Microsoft Graph API

            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                .Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri("https://login.microsoftonline.com/5950d41d-9b21-4de2-bd66-bd8e54f0bd86"))
                .Build();

            var authResult = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            string accessToken = authResult.AccessToken;

            return accessToken;

            // Now, you have the access token to use for Graph API requests
            // You can make API requests using the Microsoft Graph SDK or by sending HTTP requests directly.
        }


        public static async Task TestApi()
        {
            
            var accessToken= await GetToken();

            // Set the API endpoint
            string apiUrl = "https://graph.microsoft.com/v1.0/me";

            // Create an HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                // Set the Authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Send the GET request
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Display the response
                    Console.WriteLine(responseBody);
                }
                else
                {
                    // Display the error message
                    Console.WriteLine("Error: " + response.StatusCode);
                }
            }
        }


    }
}
