using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace SimpleBlogEditor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PreEditorPage : Page
    {
        PreTag preTag;

        public PreEditorPage()
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
            preTag = e.Parameter as PreTag;
            if (preTag.ComesFromFile)
            {
                contentTextBox.Text = preTag.Text.Trim();
            }
            else
            {
                contentTextBox.Text = ContentBuilder.PreToText(preTag.Text);
            }
        }

        private void RemoveExtraIdentation_Click_1(object sender, RoutedEventArgs e)
        {
            contentTextBox.Text = ContentBuilder.RemoveExtraIdentation(contentTextBox.Text);
        }

        private void AddIdentation_Click_1(object sender, RoutedEventArgs e)
        {
            contentTextBox.Text = ContentBuilder.AddIdentation(contentTextBox.Text);
        }

        private void Done_Click_1(object sender, RoutedEventArgs e)
        {
            // TODO: Investigate if Frame.DataContext is the best way to pass data on a GoBack(),
            // since GoBack does not have a "parameter".

            preTag.Text = ContentBuilder.TextToPre(contentTextBox.Text);
            Frame.DataContext = preTag;
            Frame.GoBack();
        }

        private void Cancel_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.DataContext = null;
            Frame.GoBack();
        }
    }
}
