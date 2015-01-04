using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Notifications;
using System.Threading;

namespace WordsGenerator
{
    class Program
    {
        private static AutoResetEvent autoResetEvent;

        // Tutorial at http://azure.microsoft.com/en-us/documentation/articles/notification-hubs-windows-store-dotnet-get-started/
        static void Main(string[] args)
        {
            autoResetEvent = new AutoResetEvent(false);
            SendNotificationAsync();
            autoResetEvent.WaitOne();
        }

        private static async void SendNotificationAsync()
        {
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(
                "Endpoint=sb://chinesenotificationhub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=jsuZEDY9/7isT/pCdJTEYNEiTQXd4MCy6yY912xyMF4=",
                "chinesenotificationhub");
            var toast = @"<toast><visual><binding template=""ToastImageAndText01""><image id=""1"" src=""http://kiewic.com/Content/Home/Icons/favicon.png"" alt=""image1""/><text id=""1"">你聞起來像醬油</text></binding></visual></toast>";
            await hub.SendWindowsNativeNotificationAsync(toast);

            autoResetEvent.Set();
        }
    }
}
