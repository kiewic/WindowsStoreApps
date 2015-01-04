using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpanishQuiz
{
    class LanguageAnswer
    {
        public LanguageAnswer(string answerString)
        {
            // TODO: Complete member initialization
            Text = answerString;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            LanguageAnswer otherAnswer = obj as LanguageAnswer;
            if (otherAnswer == null)
            {
                return false;
            }

            return Text == otherAnswer.Text;
        }

        public string Text
        {
            private set;
            get;
        }
    }
}
