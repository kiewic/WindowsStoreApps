using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BadBunny
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static public SplashScreen PageSplashScreen
        {
            get;
            set;
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            InitializeCanvas();
            MainPageStoryboard.Begin();
            base.OnNavigatedTo(e);
        }

        private void InitializeCanvas()
        {
            SplashScreen splashScreen = PageSplashScreen;
            if (splashScreen == null)
            {
                return;
            }

            double left = splashScreen.ImageLocation.Left + splashScreen.ImageLocation.Width / 2 - InitialRectangle.Width / 2;
            double top = splashScreen.ImageLocation.Top + splashScreen.ImageLocation.Height / 2 - InitialRectangle.Height / 2;

            InitialRectangle.SetValue(Canvas.LeftProperty, left);
            InitialRectangle.SetValue(Canvas.TopProperty, top);

            RectanglePositionAnimation.From = top;
            RectanglePositionAnimation.To = Window.Current.Bounds.Height;

            RectanglePositionAnimation.EasingFunction = new ExponentialEase();
            RectanglePositionAnimation.EasingFunction.EasingMode = EasingMode.EaseIn;
        }
    }
}
