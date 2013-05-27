using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SensorsApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool initialized = false;

        private double speedX;
        private double speedY;

        private double positionX;
        private double positionY;

        private Rectangle rectangle;
        private Accelerometer accelerometer;

        public MainPage()
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
            positionX = myCanvas.ActualWidth / 2;
            positionY = myCanvas.Height / 2;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += OnTick;

            accelerometer = Accelerometer.GetDefault();
            if (accelerometer != null)
            {
                uint interval = accelerometer.MinimumReportInterval * 2;
                Debug.WriteLine("interval: " + interval);

                timer.Interval = new TimeSpan(0, 0, 0, 0, (int)interval);
            }

            timer.Start();
        }

        private void OnTick(object sender, object e)
        {
            Initialize();

            if (accelerometer != null)
            {
                AccelerometerReading reading = accelerometer.GetCurrentReading();

                if (reading != null)
                {
                    //Debug.WriteLine(reading.AccelerationX + ", " + reading.AccelerationY);

                    const double multiplier = 15;

                    // Increment speed and move object.
                    speedX += reading.AccelerationX;
                    positionX += speedX;

                    if (positionX < 0)
                    {
                        positionX = 0;

                        // Invert direction.
                        speedX *= -1;
                    }
                    else if (positionX > myCanvas.ActualWidth - rectangle.Width)
                    {
                        positionX = myCanvas.ActualWidth - rectangle.Width;

                        // Invert direction.
                        speedX *= -1;
                    }

                    // Increment speed and move object.
                    speedY += reading.AccelerationY;
                    positionY -= speedY;

                    if (positionY < 0)
                    {
                        positionY = 0;

                        // Invert direction.
                        speedY *= -1;
                    }
                    else if (positionY > myCanvas.ActualHeight - rectangle.Height)
                    {
                        positionY = myCanvas.ActualHeight - rectangle.Height;

                        // Invert direction.
                        speedY *= -1;
                    }

                    rectangle.SetValue(Canvas.LeftProperty, positionX);
                    rectangle.SetValue(Canvas.TopProperty, positionY);
                }
            }
        }

        private void Initialize()
        {
            if (initialized)
            {
                return;
            }
            initialized = true;

            // Put rectangle in the middle of the screen.
            positionX = myCanvas.ActualWidth / 2;
            positionY = myCanvas.ActualHeight / 2;

            rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors.White);
            rectangle.Width = 15;
            rectangle.Height = 15;
            myCanvas.Children.Add(rectangle);
        }
    }
}
