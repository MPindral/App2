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

            string json = JsonConvert.SerializeObject(Answers, Formatting.Indented);
            
            Application.Current.Properties[LoginPage.currentUsername + "_" + ChooseQuiz.quizIdClicked] = json;

        }


        public void populateAnswers()
        {

            Answers.Clear();

            Debug.WriteLine("Opening record quiz: " + LoginPage.currentUsername + "_" + ChooseQuiz.quizIdClicked);

            ////If they already have answers recorded then go and get them
            if (Application.Current.Properties.ContainsKey(LoginPage.currentUsername + "_" + ChooseQuiz.quizIdClicked))
            {
                Debug.WriteLine("Found an existing record");

                string json = Application.Current.Properties[LoginPage.currentUsername + "_" + ChooseQuiz.quizIdClicked] as string;

                Debug.WriteLine("JSON Read: " + json);

                Answers = JsonConvert.DeserializeObject<List<AnswerManager>>(json);

                Debug.WriteLine("1 Count of Answers: " + Answers.Count);

                foreach (AnswerManager a in Answers)
                {
                    Debug.WriteLine("Cycling existing record " + a.questionId + ":" + a.answer);
                }

            }

        }

        public string ProvideAnswerText(int qId)
        {
            string answerText = "";

            Debug.WriteLine("I am in the method. qID = " + qId);

            Debug.WriteLine(Answers.Count);

            foreach (AnswerManager a in Answers)
            {

                Debug.WriteLine("looping");

                Debug.WriteLine("Cycling " + a.questionId + ":" + a.isMultipleChoice + ":" + a.isMultipleChoiceSelected + ":" + a.answer);

                if (qId == a.questionId)
                {
                    Debug.WriteLine("Found");
                    answerText = a.answer;
                }
            }

            return answerText;
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
                        Debug.WriteLine("Found and updated " + a.questionId + ":" + a.isMultipleChoice + ":" + a.isMultipleChoiceSelected + ":" + a.answer);
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
                foreach (AnswerManager a in Answers)
                {
                    Debug.WriteLine("Wasn't found not yet created " + a.questionId + ":" + a.isMultipleChoice + ":" + a.isMultipleChoiceSelected + ":" + a.answer);
                }

                AnswerManager anotherAnswer = new AnswerManager();
                anotherAnswer.questionId = qId;
                anotherAnswer.isMultipleChoice = isMultiChc;
                anotherAnswer.answer = ans;
                anotherAnswer.isMultipleChoiceSelected = isSelected;
                Answers.Add(anotherAnswer);

                foreach (AnswerManager a in Answers)
                {
                    Debug.WriteLine("Wasn't found now created " + a.questionId + ":" + a.isMultipleChoice + ":" + a.isMultipleChoiceSelected + ":" + a.answer);
                }
            }
        }
    }
}
