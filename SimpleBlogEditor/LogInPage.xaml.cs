using SimpleBlogEditor.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SimpleBlogEditor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage : LayoutAwarePage
    {
        public LogInPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            webView.Navigate(BloggerLogic.GetOAuthUri());

            // LayoutAwarePage needs to do something too.
            base.OnNavigatedTo(e);

            SettingsPane.GetForCurrentView().CommandsRequested += CommandsRequested;
        }

        void CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            UICommandInvokedHandler handler = new UICommandInvokedHandler(onSettingsCommand);

            SettingsCommand policyCommand = new SettingsCommand("policyCommand", "Privacy Policy", handler);
            args.Request.ApplicationCommands.Add(policyCommand);
        }

        private void onSettingsCommand(IUICommand command)
        {
            // It's fine not to await.
            Launcher.LaunchUriAsync(new Uri("http://kiewic.com/privacypolicy"));
        }

        private void ReproStackOverflowQuestion()
        {
            try
            {
                webView.Navigate(new Uri("http://www.amazon.com"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void webView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            WebView webView = sender as WebView;

            // If "eval" is called with "alert('hello');" we get "Exception from
            // HRESULT: 0x80020101".

            // To get the whole HTML document use:
            //string html = webView.InvokeScript("eval", new string[] {"document.documentElement.outerHTML;"});

            string scriptString = "var tags = document.getElementsByTagName('title'); tags[0].innerHTML;";
            string pageTitle = webView.InvokeScript("eval", new string[] { scriptString });

            if (BloggerLogic.DidUserLogInSuccessfully(pageTitle))
            {
                Frame.Navigate(typeof(PostsListPage));
            }
        }

    }
}
