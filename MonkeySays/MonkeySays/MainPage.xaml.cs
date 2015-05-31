using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MonkeySays
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var notAwait = IpaSymbol.LoadSymbolsAsync(AddExample);
        }

        private void AddExample(IpaSymbol ipaSymbol)
        {
            TextBlock letterBlock = new TextBlock();
            letterBlock.Style = this.Resources["LetterStyle"] as Style;
            letterBlock.Text = ipaSymbol.Value;

            TextBlock exampleBlock = new TextBlock();
            exampleBlock.Style = this.Resources["ExampleStyle"] as Style;
            IpaSymbol.SetExampleBlock(exampleBlock);

            StackPanel stackPanel = new StackPanel();
            stackPanel.VerticalAlignment = VerticalAlignment.Center;
            stackPanel.Children.Add(letterBlock);
            stackPanel.Children.Add(exampleBlock);

            Grid grid = new Grid();
            grid.Background = ipaSymbol.BackgroundBrush;
            grid.Height = Window.Current.Bounds.Height;
            grid.Children.Add(stackPanel);

            WordsPanel.Children.Add(grid);
            WordsPanel.InvalidateArrange();
            Debug.WriteLine(grid.ActualHeight);
            //Debug.WriteLine(WordsPanel.Children.Count);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Debug.WriteLine(Foo.Text);
        }
    }
}
