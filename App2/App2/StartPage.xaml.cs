using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartPage : ContentPage
	{

        

		public StartPage()
		{
			InitializeComponent ();

            //Image taken from https://psmag.com/.image/t_share/MTM2NjAzNjAwNTg2NTQ4ODMx/5396413821_78527b3335_bjpg.jpg
            BackgroundImage = "School.jpg";
        }


        private async void btnGetStartedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private async void btnViewHighScoresClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HighScoresPage());
        }


    }
}