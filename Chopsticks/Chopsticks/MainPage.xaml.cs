using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Chopsticks
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //PopInStoryboard.Begin();

        }

        private void Close_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageStoryboard.Begin();
        }

        private void PasscodeFlyout_Opened(object sender, object e)
        {
            PasscodeText.Text = PasscodeManager.Passcode;
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            PasscodeManager.GenerateNewPasscode();
            PasscodeText.Text = PasscodeManager.Passcode;
        }
    }
}
