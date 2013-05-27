using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SwipeApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const long ticksPerInterval = 100000; // 10 times per second
        private List<double> historyX;
        private List<double> historyY;
        private DispatcherTimer timer;
        private PointerPoint rectanglePoint;
        private Rectangle rectangle;
        private uint currentPointerId;
        private double averageX;
        private double averageY;

        private DateTime initialTime;
        private DateTime finalTime;
        private PointerPoint initialPoint;
        private PointerPoint finalPoint;

        public MainPage()
        {
            this.InitializeComponent();

            historyX = new List<double>();
            historyY = new List<double>();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors.HotPink);
            rectangle.Width = 100;
            rectangle.Height = 100;

            rectangle.PointerPressed += new PointerEventHandler(OnRectanglePointerPressed);
            rectangle.PointerMoved += new PointerEventHandler(OnRectanglePointerMove);
            rectangle.PointerReleased += new PointerEventHandler(OnRectanglePointerReleased);

            rectangle.SetValue(Canvas.LeftProperty, 0);
            rectangle.SetValue(Canvas.TopProperty, 0);

            myCanvas.Children.Add(rectangle);
        }

        private void OnRectanglePointerPressed(object sender, PointerRoutedEventArgs e)
        {
            // If there is a timer moving the rectangle, stop it.
            if (timer != null)
            {
                timer.Stop();
                timer = null;
                initialPoint = null;
            }

            // Reset the last point saved.
            finalPoint = null;

            rectangle.Fill = new SolidColorBrush(Colors.HotPink);

            currentPointerId = e.Pointer.PointerId;
            rectanglePoint = e.GetCurrentPoint(rectangle);
        }

        private void OnRectanglePointerMove(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerId != currentPointerId)
            {
                return;
            }

            PointerPoint point = e.GetCurrentPoint(myCanvas);

            if (initialPoint == null)
            {
                initialPoint = point;
                initialTime = DateTime.Now;
            }

            finalPoint = point;
            finalTime = DateTime.Now;

            //IList<PointerPoint> points = e.GetIntermediatePoints(myCanvas);
            //Debug.WriteLine(points.Count);

            rectangle.SetValue(Canvas.LeftProperty, point.Position.X - rectanglePoint.Position.X);
            rectangle.SetValue(Canvas.TopProperty, point.Position.Y - rectanglePoint.Position.Y);
        }

        private void OnRectanglePointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Debug.WriteLine("Pointer released.");
            currentPointerId = 0;

            rectangle.Fill = new SolidColorBrush(Colors.Goldenrod);

            double distanceX = finalPoint.Position.X - initialPoint.Position.X;
            double distanceY = finalPoint.Position.Y - initialPoint.Position.Y;
            TimeSpan deltaTime = finalTime - initialTime;

            // 1 ms = 10 000 ticks

            Debug.WriteLine("Distance: " + distanceX + " " + distanceY);
            Debug.WriteLine("Ticks: " + deltaTime.Ticks);
            Debug.WriteLine("Miliseconds: " + deltaTime.Ticks / 10000);

            double intervals = deltaTime.Ticks / ticksPerInterval;

            averageX = distanceX / intervals;
            averageY = distanceY / intervals;

            Debug.WriteLine("Average: " + averageX + " " + averageY);

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(ticksPerInterval);
            timer.Tick += new EventHandler<object>(OnTimerTick);
            timer.Start();
        }

        private double GetAverage(List<double> history)
        {
            double total = 0;
            foreach (double value in history)
            {
                total += value;
            }
            return total / (double)history.Count;
        }

        private void OnTimerTick(object sender, object e)
        {
            if (averageX > -0.1 && averageX < 0.1 && averageY > -0.1 && averageY < 0.1)
            {
                DispatcherTimer timer = sender as DispatcherTimer;
                timer.Stop();
                timer = null;
                initialPoint = null;
                return;
            }

            double left = (double)rectangle.GetValue(Canvas.LeftProperty);
            double top = (double)rectangle.GetValue(Canvas.TopProperty);

            rectangle.SetValue(Canvas.LeftProperty, left + averageX);
            rectangle.SetValue(Canvas.TopProperty, top + averageY);

            //averageX += 0.1;
            //averageY += 0.1;
        }
    }
}
