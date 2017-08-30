using System;
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

            //Application.Current.Properties["id"] = "Over here";
            if (Application.Current.Properties.ContainsKey("id"))
            {
                var id = Application.Current.Properties["id"] as string;
                Console.WriteLine("this is my saved id: "+id);
            }





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