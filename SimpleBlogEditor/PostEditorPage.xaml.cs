using SimpleBlogEditor.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
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
    public sealed partial class PostEditorPage : LayoutAwarePage
    {
        public PostEditorPage()
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
            // LayoutAwarePage needs to do something too.
            base.OnNavigatedTo(e);

            UpdatePreTag(Frame.DataContext as PreTag);
        }

        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            base.LoadState(navigationParameter, pageState);

            if (pageState != null)
            {
                contentTextBox.Text = pageState["contentTextBox.Text"] as string;
                contentTextBox.SelectionStart = (int)pageState["contentTextBox.SelectionStart"];
                titleTextBox.Text = pageState["titleTextBox.Text"] as string;
                tagsTextBox.Text = pageState["tagsTextBox.Text"] as string;
            }
        }

        protected override void SaveState(Dictionary<string, object> pageState)
        {
            base.SaveState(pageState);

            pageState.Add("contentTextBox.Text", contentTextBox.Text);
            pageState.Add("contentTextBox.SelectionStart", contentTextBox.SelectionStart);
            pageState.Add("titleTextBox.Text", titleTextBox.Text);
            pageState.Add("tagsTextBox.Text", tagsTextBox.Text);
        }

        private void UpdatePreTag(PreTag preTag)
        {
            if (preTag == null)
            {
                return;
            }

            int position = contentTextBox.SelectionStart;

            if (preTag.ComesFromFile)
            {
                if (!contentTextBox.Text.EndsWith("\r\n\r\n"))
                {
                    contentTextBox.Text += "\r\n\r\n";
                }
                contentTextBox.Text += preTag.Text;
            }
            else
            {
                contentTextBox.Text = contentTextBox.Text.Substring(0, preTag.Start) +
                    preTag.Text +
                    contentTextBox.Text.Substring(preTag.Start + preTag.Length);
            }

            contentTextBox.SelectionStart = position;
        }

        private async void insertFileButton_Click_1(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            string[] fileExtensions = new string[] {
                ".txt",
                ".cpp",
                ".c",
                ".h",
                ".cs",
                ".vb",
                ".xaml",
                ".js",
                ".htm",
                ".html",
                ".css",
                ".htm",
                ".sql",
                ".py" };
            foreach (string fileExtension in fileExtensions)
            {
                openPicker.FileTypeFilter.Add(fileExtension);
            }

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                ReadFileAndAddTextBox(file);
            }
        }

        private async void ReadFileAndAddTextBox(StorageFile file)
        {
            string fileContent = await FileIO.ReadTextAsync(file);

            PreTag preTag = new PreTag();
            preTag.ComesFromFile = true;
            preTag.Text = fileContent;

            Frame.Navigate(typeof(PreEditorPage), preTag);
        }

        private async void TextBox_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            PreTag preTag = ContentBuilder.GetPreTag(textBox);
            if (preTag != null)
            {
                e.Handled = true;

                // Create a menu and add commands specifying an id value for each instead of a delegate.
                var menu = new PopupMenu();
                menu.Commands.Add(new UICommand("Edit <pre> tag.", null, 2));
                var chosenCommand = await menu.ShowAsync(new Point(e.CursorLeft, e.CursorTop));
                if (chosenCommand != null)
                {
                    if (chosenCommand.Label.StartsWith("Edit"))
                    {
                        Frame.Navigate(typeof(PreEditorPage), preTag);
                    }
                }
            }
        }

        private void contentTextBox_Loaded_1(object sender, RoutedEventArgs e)
        {
            contentTextBox.ScrollToEnd();
            //contentTextBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }
    }

    public static class TextBlockExtensions
    {
        public static void ScrollToEnd(this TextBox tb)
        {
            var scrollViewer = tb.GetFirstDescendantOfType<ScrollViewer>();
            scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
        }
    }

    public static class VisualTreeHelperExtensions
    {
        public static T GetFirstDescendantOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            return start.GetDescendantsOfType<T>().First();
        }

        public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject start) where T : DependencyObject
        {
            var queue = new Queue<DependencyObject>();
            var count = VisualTreeHelper.GetChildrenCount(start);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(start, i);

                if (child is T)
                    yield return (T)child;

                queue.Enqueue(child);
            }

            while (queue.Count > 0)
            {
                var parent = queue.Dequeue();
                var count2 = VisualTreeHelper.GetChildrenCount(parent);

                for (int i = 0; i < count2; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);

                    if (child is T)
                        yield return (T)child;

                    queue.Enqueue(child);
                }
            }
        }
    }
}
