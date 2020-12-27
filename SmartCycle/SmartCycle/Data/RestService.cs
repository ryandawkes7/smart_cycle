using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using SmartCycle.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmartCycle.Data
{
    // REST API Service Setup 
    public class RestService
    {
        HttpClient client;
        string grant_type = "password";

        // Set defaults for server
        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        // Login function
        public async Task<Token> Login(User user)
        {
            // POST data with user credentials as a list
            var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", grant_type)); 
            postData.Add(new KeyValuePair<string, string>("username", user.Username)); // e.g. username: test_user
            postData.Add(new KeyValuePair<string, string>("password", user.Password)); // e.g. password: password123

            // Use body to send content formatted as a query string
            var content = new FormUrlEncodedContent(postData);

            // Assign POST response content
            var response = await PostResponseLogin<Token>(Constants.LoginUrl, content);

            // Require user to login again after set amount of time
            DateTime dt = new DateTime();
            dt = DateTime.Today;
            response.expire_date = dt.AddSeconds(response.expire_in);

            return response;
        }

        // POST response function for logging in
        public async Task<T> PostResponseLogin<T>(string weburl, FormUrlEncodedContent content) where T : class
        {
            // Send POST request with content to server asynchronously
            var response = await client.PostAsync(weburl, content);

            // Extract object sent from API as JSON object
            var jsonResult = response.Content.ReadAsStringAsync().Result;

            // Deserialize content from JSON object & convert to .NET object
            var responseObject = JsonConvert.DeserializeObject<T>(jsonResult);

            // Return response
            return responseObject;
        }

        // 
        public async Task<T> PostResponse<T>(string weburl, string jsonstring) where T : class
        {
            // Retrieve & assign token from token DB
            var Token = App.TokenDatabase.GetToken();

            // Gather content in JSON format
            string ContentType = "application/json";

            // Authorise user with ACCESS_TOKEN in header
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.access_token);

            try
            {
                // Await new POST to be made to server with content in UTF8 & JSON format
                var Result = await client.PostAsync(weburl, new StringContent(jsonstring, Encoding.UTF8, ContentType));

                // If there is no error when logging in
                if(Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Extract object sent from API as JSON object
                    var JsonResult = Result.Content.ReadAsStringAsync().Result;

                    try
                    {
                        // Deserialize content from JSON object & convert to .NET object
                        var ContentResp = JsonConvert.DeserializeObject<T>(JsonResult);
                        return ContentResp;
                    }
                    catch { return null; }
                }
            }
            catch { return null; }
            
            // If login is unsuccessful, do not send POST request/do nothing
            return null;
        }

        // 
        public async Task<T> GetResponse<T>(string weburl) where T : class
        {
            // Retrieve & assign token from token DB
            var Token = App.TokenDatabase.GetToken();

            // Authorise user with ACCESS_TOKEN in header
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.access_token);

            try
            {
                // Send GET reuqest to server aysnchronously/in background
                var response = await client.GetAsync(weburl);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    // Extract object sent from API as JSON object
                    var JsonResult = response.Content.ReadAsStringAsync().Result;

                    try
                    {
                        // Deserialize content from JSON object & convert to .NET object
                        var ContentResp = JsonConvert.DeserializeObject<T>(JsonResult);

                        return ContentResp;
                    }
                    catch { return null; }
                }
            }
            catch { return null; }

            // If login is unsuccessful, do not send GET request/do nothing
            return null;

        }
    }
}
