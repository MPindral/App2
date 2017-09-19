using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace App2
{
    public class AnswerManager
    {

        
        public static List<AnswerManager> Answers = new List<AnswerManager>();

        public int questionId { get; set; }

        public bool isMultipleChoice { get; set; }

        public bool isMultipleChoiceSelected { get; set; }

        public string answer { get; set; }

        //Used from
        //https://developer.xamarin.com/guides/xamarin-forms/application-fundamentals/application-class/
        public List<string> GetAllUserNames()
        {
            List<string> userNames = new List<string>();

            foreach (KeyValuePair<string, object> kvp in Application.Current.Properties)
            {
                userNames.Add(kvp.Key);
            }

            return userNames;
        }


        
        public void WriteAnswersToDisk()
        {

            //https://stackoverflow.com/questions/7947005/how-to-turn-on-indentation-when-writing-json-using-json-net
            string json = JsonConvert.SerializeObject(Answers, Formatting.Indented);

            //https://www.newtonsoft.com/json/help/html/SerializingJSON.htm
            Application.Current.Properties[LoginPage.currentUsername + "_" + ChooseQuiz.quizIdClicked] = json;
        }


        public void populateAnswers()
        {

            Answers.Clear();

            ////If they already have answers recorded then go and get them
            if (Application.Current.Properties.ContainsKey(LoginPage.currentUsername + "_" + ChooseQuiz.quizIdClicked))
            {
                string json = Application.Current.Properties[LoginPage.currentUsername + "_" + ChooseQuiz.quizIdClicked] as string;
                Answers = JsonConvert.DeserializeObject<List<AnswerManager>>(json);//
            }

        }

        public string ProvideAnswerText(int qId)
        {
            string answerText = "";

            foreach (AnswerManager a in Answers)
            {
                if (qId == a.questionId)
                {
                    answerText = a.answer;
                }
            }

            return answerText;
        }

        public bool ProvideAnswerMultipleChoice(int qId, string choice)
        {
            bool answerToggle = false;

            foreach (AnswerManager a in Answers)
            {

                if (qId == a.questionId && choice == a.answer)
                {
                    answerToggle = a.isMultipleChoiceSelected;
                }
            }

            return answerToggle;
        }


        public void UpdateAnswerList(int qId, bool isMultiChc, bool isSelected, string ans)
        {
            bool wasfound = false;

            if (!isMultiChc)
            {
                foreach (AnswerManager a in Answers)
                {
                    //if the question already exists in the list
                    if (a.questionId == qId)
                    {
                        //It was found and updated
                        wasfound = true;
                        a.answer = ans;
                    }
                }
            }

            if (isMultiChc)
            {
                foreach (AnswerManager a in Answers)
                {
                    if (a.questionId == qId && a.answer == ans)
                    {
                        wasfound = true;
                        a.isMultipleChoiceSelected = isSelected;
                    }
                }
            }

            //if the question does not exist then create it and add to the list
            if (!wasfound)
            {

                AnswerManager anotherAnswer = new AnswerManager();
                anotherAnswer.questionId = qId;
                anotherAnswer.isMultipleChoice = isMultiChc;
                anotherAnswer.answer = ans;
                anotherAnswer.isMultipleChoiceSelected = isSelected;
                Answers.Add(anotherAnswer);

            }
        }
    }
}
