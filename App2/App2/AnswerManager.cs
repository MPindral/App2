using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    public class AnswerManager
    {
        public int questionId { get; set; }

        public bool isMultipleChoice { get; set; }

        public bool isMultipleChoiceSelected { get; set; }

        public string answer { get; set; }


        public void WriteAnswersToDisk()
        {
            Application.Current.Properties[LoginPage.currentUsername + "," + ChooseQuiz.quizIdClicked] = RecordQuizAnswers.Answers;
        }

    }
}
