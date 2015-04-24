using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OneDriveHackaton2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {


            Uri StartUri = new Uri(String.Format(
                "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope={1}&response_type=token&redirect_uri={2}",
                "000000004812E450",
                "wl.signin onedrive.readwrite",
                WebUtility.UrlEncode("http://kiewic.com/")));

            Uri EndUri = new Uri("http://kiewic.com/");

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None,
                StartUri,
                EndUri);

            string testData = "http://kiewic.com/#access_token=EwCQAq1DBAAUGCCXc8wU/zFu9QnLdZXy%2bYnElFkAASu2H0Plv73qlsNgaAsNtK931pWT/wMdIa4zJzNKDNz9Er%2b97p7qo0XGR4%2bkfr%2bTCh4EV5wQ5NTYHufWeIdlvlm9lkdt3S6SotwygCJtUJqoVBI4fGJxXdTz6tCiqsC2NbZNxGR9od4osWN33ZuNIHp2f3%2b0KiX0fs%2bLJK8wIHW22LtUsj%2bCZY/K9qA%2bEbxAfgIpZDsAzV2U1NdjUobSKkvA3SEaARx/exIm0jZTRvM0XRoNHn/ZhlcMrd1nehEpOM3SFp3%2bkIfKiFiHXVm0P/vRS/VJFHXyNyCDFqGuw63vOklnqT4w2LhQyGu3G/%2bUIJBpBfuSHNwikthHPm3xjYoDZgAACO1iT9UawnDVYAFVZk/mv7wwb58vrJc1l0v7UClfth7j5bm/%2b7yYB0Rr9WHdquSqrotvovjR//V2Kmtgn3rzLQYg8ma5/2t6VgtyPIwbQm6kRP382CYpS3n5ZdvyX7nNWHwM9RlsKkZdS9zT7kkwxVsbW7MJaqSlcnbnkWaad84KzfrjSyxr9ceUb/eajRu3ltRy9Tbtkt2A8QjXtKw2Th82WjIrZjDg10JoeRqvFtfu1IeEBlofUgAPUK7VkbDdKVbgDIl97TZD5qW9m8JTQkhlbq6%2btZhiqFN/JZLOPum6a4sEOAf46v1sw70UDv0raxewMA6y2j6gGMGFojFse6vWHXTLQRpqnBwpc3iOnaaqcMGGCRimdMhtCmKITW9%2bJ/NbKo8DbTk65ancQYmBgNpNHNVStZTGex3MwcxEY9mQ1p69aXN0fXhWY7GL%2bB0wEuwJn50H04s4WtlepIv2Ww0QhfZ1vxkd1HIRdwE%3d&authentication_token=eyJhbGciOiJIUzI1NiIsImtpZCI6IjEiLCJ0eXAiOiJKV1QifQ.eyJ2ZXIiOjEsImlzcyI6InVybjp3aW5kb3dzOmxpdmVpZCIsImV4cCI6MTQyNTYxMjU0NCwidWlkIjoiNmM0NzY5YjcxMmZlNDBjYTY0MDAyYTg4MDI1NjBkMjgiLCJhdWQiOiJraWV3aWMuY29tIiwidXJuOm1pY3Jvc29mdDphcHB1cmkiOiJhcHBpZDovLzAwMDAwMDAwNDgxMkU0NTAiLCJ1cm46bWljcm9zb2Z0OmFwcGlkIjoiMDAwMDAwMDA0ODEyRTQ1MCJ9.Xzw94LXFH3wIwwSWpQmxPH9HAB9H-qyLLW4WZfcSY9o&token_type=bearer&expires_in=3600&scope=wl.signin%20onedrive.readwrite%20wl.basic%20wl.contacts_skydrive%20wl.skydrive%20wl.skydrive_update&user_id=6c4769b712fe40ca64002a8802560d28";

            testData = result.ResponseData;

            var urlDecoder = new WwwFormUrlDecoder(testData);
            foreach (var decoderEntry in urlDecoder)
            {
                Debug.WriteLine(decoderEntry.Name + " " + decoderEntry.Value);
            }

            string token = urlDecoder.GetFirstValueByName("http://kiewic.com/#access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("bearer {0}", token));

            HttpStringContent createSessionContent = new HttpStringContent(
                "{ \"@name.conflictBehavior\": \"rename\" }",
                UnicodeEncoding.Utf8,
                "application/json");

            Uri createSessionUri = new Uri("https://api.onedrive.com/v1.0/drive/root:/Foo/bar2.txt:/upload.createSession");
            var response = await client.PostAsync(createSessionUri, createSessionContent);
            var responseString = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(responseString);

            JsonObject jsonObject = JsonObject.Parse(responseString);
            Uri uploadUrl = new Uri(jsonObject.GetNamedString("uploadUrl"));

            HttpStringContent putContent1 = new HttpStringContent(
                "hello ",
                UnicodeEncoding.Utf8,
                "text/plain");
            putContent1.Headers.Add("Content-Range", "bytes 0-5/11");

            HttpStringContent putContent2 = new HttpStringContent(
                "world",
                UnicodeEncoding.Utf8,
                "text/plain");
            putContent2.Headers.Add("Content-Range", "bytes 6-10/11");

            response = await client.PutAsync(uploadUrl, putContent2);
            responseString = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(responseString);

            response = await client.PutAsync(uploadUrl, putContent1);
            responseString = await response.Content.ReadAsStringAsync();

            Debug.WriteLine(responseString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            
        }
    }
}
