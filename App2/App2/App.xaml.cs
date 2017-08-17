using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace App2
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();


            //I ran out of time and I was unable to wire the activities together with buttons
            //So to test the various separate screens I would comment out the relevant lines
            //to run the particular activity.

            MainPage = new NavigationPage(new StartPage());
            //MainPage = new App2.LoginPage();
            //MainPage = new App2.HighScoresPage();
            //MainPage = new App2.ChooseQuiz();
            //MainPage = new App2.RecordQuizAnswers();
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
