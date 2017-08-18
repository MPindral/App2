﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{
		public StartPage()
		{
			InitializeComponent ();
		}

        private async void btnStartQuizClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChooseQuiz());
        }

        private async void btnLoginChangeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void btnViewHighScoresClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HighScoresPage());
        }


    }
}