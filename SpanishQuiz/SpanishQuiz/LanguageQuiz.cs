using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpanishQuiz
{
    class LanguageQuiz
    {
        private IList<LanguageQuestion> questions;
        private IList<LanguageAnswer> answers;
        private IEnumerator<LanguageQuestion> enumerator;

        public async Task LoadAsync()
        {
            questions = await LanguageQuestion.LoadAsync();
            JoinAllAnswers();

            Debug.WriteLine(questions.Count);
            enumerator = questions.GetEnumerator();
            enumerator.MoveNext(); // TODO: Assert.
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public bool IsCorrect(string answerString)
        {
            if (Current.Answers.Contains(new LanguageAnswer(answerString)))
            {
                return true;
            }
            return false;
        }


        private void JoinAllAnswers()
        {
            List<LanguageAnswer> localAnswers = new List<LanguageAnswer>();
            foreach (LanguageQuestion question in questions)
            {
                localAnswers.AddRange(question.Answers);
                Debug.WriteLine(localAnswers.Count);
            }
            answers = localAnswers.ToArray();
        }

        public LanguageQuestion Current
        {
            get
            {
                return enumerator.Current;
            }
        }


        public LanguageAnswer RandomAnswer
        {
            get
            {
                Random random = new Random();
                int randomNumber = random.Next(0, answers.Count);
                return answers[randomNumber];
            }
        }

    }
}
