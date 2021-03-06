﻿using System;
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

namespace MonkeySays
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GridViewPage : Page
    {
        public GridViewPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            List<IpaSymbol> symbols = new List<IpaSymbol>();
            await IpaSymbol.LoadSymbolsAsync((ipaSymbol) => {
                symbols.Add(ipaSymbol);
            });

            SymbolsView.ItemsSource = symbols;

            if (symbols.Count > 0)
            {
                DisplaySymbol(symbols[0]);
            }
        }

        private void SymbolsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            if (gridView.SelectedItem == null)
            {
                return;
            }

            IpaSymbol ipaSymbol = gridView.SelectedItem as IpaSymbol;
            DisplaySymbol(ipaSymbol);

            TheViewer.ScrollToHorizontalOffset(TheViewer.HorizontalOffset + TheViewer.ActualWidth);
        }

        private void DisplaySymbol(IpaSymbol ipaSymbol)
        {
            SymbolGrid.DataContext = ipaSymbol;
            IpaSymbol.SetExampleBlock(ExampleBlock);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null);
        }
    }
}
