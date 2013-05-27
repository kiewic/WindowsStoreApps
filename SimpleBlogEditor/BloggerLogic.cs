using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace SimpleBlogEditor
{
    class BloggerLogic
    {
        private static string googleAccessToken;
        private static string googleRefreshToken;
        private static string settingsFileName = "settings.json";
        private static StorageFile settingsFile;

        // TODO: Add a log-out button in the setings bar.

        public static bool IsLogged
        {
            get
            {
                return !String.IsNullOrEmpty(googleAccessToken);
            }
        }

        public static Uri GetOAuthUri()
        {
            string uriString = "https://accounts.google.com/o/oauth2/auth?" +
                "response_type=code&" +
                "client_id=" + GoogleClient.Id + "&" +
                "redirect_uri=urn:ietf:wg:oauth:2.0:oob&" +
                "scope=" + Uri.EscapeDataString("https://www.googleapis.com/auth/blogger") + "&" +
                "state=IAmTesting";

            return new Uri(uriString);
        }

        public static bool DidUserLogInSuccessfully(string pageTitle)
        {
            if (pageTitle.StartsWith("Success "))
            {
                string[] fields = pageTitle.Substring("Success ".Length).Split(
                    new string[] { "&amp;" },
                    StringSplitOptions.RemoveEmptyEntries);

                foreach (string field in fields)
                {
                    string[] keyValuePair = field.Split('=');
                    if (keyValuePair[0] == "code")
                    {
                        ExchangeAuthCodeForAccessToken(keyValuePair[1]);
                        return true;
                    }
                }
            }

            return false;
        }

        private static async Task<JsonArray> GetListOfBlogs()
        {
            string uriString = "https://www.googleapis.com/blogger/v3/users/self/blogs";

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.Add("Authorization", "Bearer " + googleAccessToken);

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseString = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(responseString);

            JsonObject responseObject;
            if (JsonObject.TryParse(responseString, out responseObject))
            {
                if (responseObject.Keys.Contains("items"))
                {
                    return responseObject.GetNamedArray("items");
                }
            }

            return new JsonArray();
        }

        private static async void PostANewPost(string blogId)
        {
            if (String.IsNullOrEmpty(googleAccessToken))
            {
                throw new Exception("Missing access token.");
            }

            string uriString = "https://www.googleapis.com/blogger/v3/blogs/" + blogId + "/posts/?" +
                "access_token=" + googleAccessToken;

            JsonObject blog = new JsonObject();
            blog["id"] = JsonValue.CreateStringValue(blogId);

            JsonObject addPostRequest = new JsonObject();
            addPostRequest["kind"] = JsonValue.CreateStringValue("blogger#post");
            addPostRequest["blog"] = blog; // Maybe: JsonValue.CreateObjectValue( ... );
            addPostRequest["title"] = JsonValue.CreateStringValue("This is an automatic test 2.");
            addPostRequest["content"] = JsonValue.CreateStringValue("<pre>&lt;pre&gt;var somethig;&lt;/pre&gt;</pre>");

            string requestContent = addPostRequest.Stringify();
            HttpClient httpClient = new HttpClient();
            HttpContent content = new StringContent(requestContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uriString, content);
            string responseContent = await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> GetBlogId()
        {
            if (String.IsNullOrEmpty(googleAccessToken))
            {
                throw new Exception("Missing access token.");
            }

            string uriString = "https://www.googleapis.com/blogger/v3/blogs/byurl?" +
                "url=http://blog.kiewic.com/&" +
                "access_token=" + googleAccessToken;

            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(uriString);
            string responseContent = await response.Content.ReadAsStringAsync();

            JsonObject blogByUrlResponse = JsonObject.Parse(responseContent);
            return blogByUrlResponse["id"].GetString();
        }

        private static async void ExchangeAuthCodeForAccessToken(string authorizationCode)
        {
            Uri uri = new Uri("https://accounts.google.com/o/oauth2/token");

            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields["code"] = authorizationCode;
            fields["client_id"] = GoogleClient.Id;
            fields["client_secret"] = GoogleClient.Secret;
            fields["redirect_uri"] = "urn:ietf:wg:oauth:2.0:oob";
            fields["grant_type"] = "authorization_code";

            HttpClient httpClient = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = new FormUrlEncodedContent(fields);
            HttpResponseMessage response = await httpClient.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            JsonObject accessTokenResponse = JsonObject.Parse(responseContent);

            if (responseContent.Contains("access_token"))
            {
                googleAccessToken = accessTokenResponse.GetNamedString("access_token");
                googleRefreshToken = accessTokenResponse.GetNamedString("refresh_token");
                int expiresIn = (int)accessTokenResponse.GetNamedNumber("expires_in");
                SaveSettings();
            }
        }

        // NOTE: 'async void Foo()' gets splitted in two, but is not awaitable,
        // 'async Task Foo()' gets splitted and is awaitable.
        // This can be awaitable.
        public static async Task LoadSettings()
        {
            // Set default values.
            googleAccessToken = "";

            // Read file.
            try
            {
                Debug.WriteLine("Settings file location: " + ApplicationData.Current.LocalFolder.Path);
                settingsFile = await ApplicationData.Current.LocalFolder.GetFileAsync(settingsFileName);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Settings file not found.");
                CreateSettingsFile();

                // Nothing else to do, we can return.
                return;
            }

            string jsonString = await FileIO.ReadTextAsync(settingsFile);
            JsonObject jsonObject;
            if (!JsonObject.TryParse(jsonString, out jsonObject))
            {
                // Nothing else to do, we can return.
                return;
            }

            if (jsonObject.Keys.Contains("googleAccessToken"))
            {
                googleAccessToken = jsonObject["googleAccessToken"].GetString();
            }

            if (jsonObject.Keys.Contains("googleRefreshToken"))
            {
                googleRefreshToken = jsonObject["googleRefreshToken"].GetString();
            }
        }

        private static async void CreateSettingsFile()
        {
            StorageFile settingsFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(settingsFileName);
        }

        public static async void SaveSettings()
        {
            if (settingsFile == null)
            {
                // TODO: Be careful, settings file is not ready and settings won't be saved.
                return;
            }

            JsonObject jsonObject = new JsonObject();

            jsonObject["googleAccessToken"] = JsonValue.CreateStringValue(googleAccessToken);
            jsonObject["googleRefreshToken"] = JsonValue.CreateStringValue(googleRefreshToken);

            await FileIO.WriteTextAsync(settingsFile, jsonObject.Stringify());
        }

    }
}
