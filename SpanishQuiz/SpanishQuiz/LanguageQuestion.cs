using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;

namespace SpanishQuiz
{
    class LanguageQuestion
    {
        public LanguageQuestion(string question, JsonArray jsonAnswers)
        {
            // TODO: Complete member initialization
            Text = question;
            LoadAnswers(jsonAnswers);
        }

        private void LoadAnswers(JsonArray jsonAnswers)
        {
            List<LanguageAnswer> answers = new List<LanguageAnswer>();
            foreach (var jsonAnswer in jsonAnswers)
            {
                string answerString = jsonAnswer.GetString();
                answers.Add(new LanguageAnswer(answerString));
            }
            Answers = answers.ToArray();
        }

        public static async Task<IList<LanguageQuestion>> LoadAsync()
        {
            Uri uri = new Uri("ms-appx:///week1.json");
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(uri);
            string jsonString = await FileIO.ReadTextAsync(file);
            JsonArray jsonArray = JsonArray.Parse(jsonString);

            List<LanguageQuestion> questions = new List<LanguageQuestion>();

            foreach (var jsonQuestion in jsonArray)
            {
                JsonObject jsonObject = jsonQuestion.GetObject();

                // TODO: Validate valus exist.
                string question = jsonObject.GetNamedString("Question");
                JsonArray jsonAnswers = jsonObject.GetNamedArray("Answer");

                questions.Add(new LanguageQuestion(question, jsonAnswers));
            }

            return questions.ToArray();
        }

        public string Text
        {
            get;
            private set;
        }

        public IList<LanguageAnswer> Answers
        {
            private set;
            get;
        }
    }
}
