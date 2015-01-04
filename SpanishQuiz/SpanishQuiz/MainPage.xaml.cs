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

namespace SpanishQuiz
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LanguageQuiz quiz;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            quiz = new LanguageQuiz();
            await quiz.LoadAsync();

            QuestionBlock.Text = quiz.Current.Text;
            Answer1Button.Content = quiz.Current.Answers[0].Text;
            Answer2Button.Content = quiz.RandomAnswer.Text;
        }

        private void Answer1Button_Click(object sender, RoutedEventArgs e)
        {
            // If correct:
            int count = Int32.Parse(CorrectBlock.Text);
            CorrectBlock.Text = (count + 1).ToString();
        }

        private void Answer2Button_Click(object sender, RoutedEventArgs e)
        {
            // If incorrect.
            int count = Int32.Parse(WrongBlock.Text);
            WrongBlock.Text = (count + 1).ToString();
        }
    }
}
