using SimpleBlogEditor.Common;
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
    public sealed partial class PostsListPage : LayoutAwarePage
    {
        public PostsListPage()
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

            DisplayBlogList();
        }

        private void DisplayBlogList()
        {
            blogsListView.Items.Add(new {
                Name = "My Blog 1",
                Url = "http://google.com",
                Id = "Lalalala"
            });
        }

        private void blogsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            popUpGrid.Visibility = Visibility.Collapsed;
            popUpBackgroundRect.Visibility = Visibility.Collapsed;
        }

        private void postsListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            bottomBar.IsOpen = true;
        }

        private void newButton_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PostEditorPage));
        }
    }
}
