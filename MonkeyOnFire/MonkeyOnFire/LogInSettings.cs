using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace MonkeyOnFire
{
    class LogInSettings
    {
        private static IAsyncOperation<IUICommand> showOperation;

        public static string AppId
        {
            get
            {
                return "1431183373786250";
            }
        }

        public static string AppName
        {
            get
            {
                return "\uD83D\uDC12 on \uD83D\uDD25 App";
            }
        }

        public static string RedirectUriString
        {
            get
            {
                return "http://kiewic.com";
            }
        }

        public static string AccessToken
        {
            get
            {
                try
                {
                    PasswordVault vault = new PasswordVault();
                    vault.FindAllByResource(AppName);
                    PasswordCredential credential = vault.FindAllByResource(AppName).FirstOrDefault();
                    if (credential != null)
                    {
                        return vault.Retrieve(AppName, credential.UserName).Password;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return String.Empty;
            }
            set
            {
                PasswordVault vault = new PasswordVault();
                vault.Add(new PasswordCredential(AppName, "root", value));
            }
        }

        public static bool ParseResponseData(string responseData)
        {
            try
            {
                Uri uri = new Uri(responseData);

                if (!String.IsNullOrEmpty(uri.Query))
                {
                    // Error example:
                    // http://kiewic.com/?error_code=1349126&error_message=App%20Not%20Setup%3A%20The%20developers%20of%20this%20app%20have%20not%20set%20up%20this%20app%20properly%20for%20Facebook%20Login.#_=_
                    WwwFormUrlDecoder queryString = new WwwFormUrlDecoder(uri.Query);

                    string errorCode = queryString.GetFirstValueByName("error_code");
                    if (!String.IsNullOrEmpty(errorCode))
                    {
                        string errorMessage = queryString.GetFirstValueByName("error_message");
                        ShowError(errorMessage, String.Format("Error {0}", errorCode));
                        return false;
                    }
                }

                if (!String.IsNullOrEmpty(uri.Fragment))
                {
                    // Token example:
                    // http://kiewic.com/#access_token=CAAUVp1iaXIoBADZA6TV0BPB7hUGkqF7I6ZAcvWWlabBznOWzdicE4ZBeF1c2G3IOsHbaKiSedCr7PpQxAZAqmhPiIGV6dYndtqGBIaIXwcWc3Rj6lLfYQlGDdtRwt0ZBEu4KuZCYIel1S4FVeND5q2lfH0gYMFpU2ncmdTN6BTLDA0HVmKvKWLKH71YzmrFP3mdB7CR5TcHAZDZD&expires_in=4774
                    WwwFormUrlDecoder queryString = new WwwFormUrlDecoder(uri.Fragment.Substring(1));

                    string accessToken = queryString.GetFirstValueByName("access_token");
                    AccessToken = accessToken;
                    Debug.WriteLine(AccessToken);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message, String.Format("Error 0x{0:X}", ex.HResult));
            }

            return false;
        }

        private static void ShowError(string message, string title)
        {
            MessageDialog dialog = new MessageDialog(message, title);
            if (showOperation != null)
            {
                showOperation.Cancel();
            }
            showOperation = dialog.ShowAsync();
        }
    }
}
