using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonkeyOnFire
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage : Page
    {
        public LogInPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CheckPreviousLogIn();
        }

        private void CheckPreviousLogIn()
        {
            string token = LogInSettings.AccessToken;
            if (!String.IsNullOrEmpty(token))
            {
                // Cannot call Navigate within OnNavigatedTo.
                IAsyncAction action = Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal,
                    () => GoToProfilePage());
            }
        }

        private async void LogIn_Click(object sender, RoutedEventArgs e)
        {
            Uri requestUri = new Uri(String.Format(
                "https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&scope=read_stream&display=popup&response_type=token",
                LogInSettings.AppId,
                Uri.EscapeDataString(LogInSettings.RedirectUriString)));
            Uri callbackUri = new Uri(LogInSettings.RedirectUriString);

            // Hide the big fire.
            MonkeyReallyOnFire.Visibility = Visibility.Collapsed;

            WebAuthenticationResult result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.None,
                requestUri,
                callbackUri);

            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                    if (!LogInSettings.ParseResponseData(result.ResponseData))
                    {
                        ShowMonkeyReallyOnFire();
                    }
                    else
                    {
                        GoToProfilePage();
                    }
                    break;
                case WebAuthenticationStatus.ErrorHttp:
                    // TODO: The web is on fire. We got a <http error> error.
                    break;
                case WebAuthenticationStatus.UserCancel:
                    ShowMonkeyReallyOnFire();
                    break;
            }
        }

        private void GoToProfilePage()
        {
            Debug.WriteLine(Frame.Navigate(typeof(ProfilePage)));
        }

        private void ShowMonkeyReallyOnFire()
        {
            // TODO: Now, the monkey is really on fire. Please try again.
            MonkeyReallyOnFire.Visibility = Visibility.Visible;
        }
    }
}
