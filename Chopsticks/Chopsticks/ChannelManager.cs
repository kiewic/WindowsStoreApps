using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.Web.Http;

namespace Chopsticks
{
    class ChannelManager
    {
        private const string ChannelUriKey = "ChannelUri";
        private const string MessageKey = "Message";
        private const string UriKey = "Uri";
        private const string PasscodeKey = "Passcode";
        //private const string InfoUriString = "http://www.kiewic.com/chopsticks/settings";
        private const string InfoUriString = "http://localhost:2036/chopsticks/settings";
        private const string RegisterPath = "/register";

        private static string serverUriString;
        private static HttpClient client;

        public static async void Load()
        {
            IPropertySet localValues = ApplicationData.Current.LocalSettings.Values;
            string channelUriString = localValues[ChannelUriKey] as string;
            if (channelUriString == null)
            {
                PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                channelUriString = channel.Uri;
                localValues[ChannelUriKey] = channelUriString;
                // TODO: Save expiration date (or decie if we want to renew the channel every time app is launched).
            }

            if (String.IsNullOrEmpty(channelUriString))
            {
                throw new COMException("channelUriString", unchecked((int)0x80070057));
            }

            GetServerInfo(channelUriString);
        }

        // Get server URI.
        private static async void GetServerInfo(string channelUriString)
        {
            client = new HttpClient();

            string jsonString = null;
            try
            {
                jsonString = await client.GetStringAsync(new Uri(InfoUriString));
            }
            catch (Exception ex)
            {
                // TODO: Handle connection exceptions.
                // Netowrk exception.
                Debug.WriteLine(ex.Message);
                return;
            }

            JsonObject jsonObject;
            if (!JsonObject.TryParse(jsonString, out jsonObject))
            {
                // Wrong formatted response.
                Debug.WriteLine("No JSON format. Swallowing error.");
                return;
            }

            if (jsonObject.ContainsKey(MessageKey))
            {
                // TODO: Display the message.
            }

            serverUriString = "";
            if (jsonObject.ContainsKey(UriKey))
            {
                serverUriString = jsonObject.GetNamedString(UriKey);
            }

            RegisterClient(channelUriString);
        }

        private static async void RegisterClient(string channelUriString)
        {
            // Send passcode and channel URI to server.
            Uri registerUri = new Uri(serverUriString + "/register");

            JsonObject registerObject = new JsonObject();
            registerObject.SetNamedValue(ChannelUriKey, JsonValue.CreateStringValue(channelUriString));
            registerObject.SetNamedValue(PasscodeKey, JsonValue.CreateStringValue(PasscodeManager.Passcode));
            HttpStringContent content = new HttpStringContent(registerObject.Stringify());

            HttpResponseMessage response = await client.PostAsync(registerUri, content);
            string jsonString = await response.Content.ReadAsStringAsync();

            JsonObject jsonObject;
            if (!JsonObject.TryParse(jsonString, out jsonObject))
            {
                // Wrong formatted response.
                Debug.WriteLine("No JSON format. Swallowing error.");
                return;
            }

            // TODO: Verify the response is successful.
        }
    }
}
