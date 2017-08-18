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
	public partial class HighScoresPage : ContentPage
	{
		public HighScoresPage ()
		{
			InitializeComponent ();

            List<string> listItems = new List<string>
            {
                "1000 Jane Smith",
                "860 Graeme Barton",
                "450 Sarah McCormack",
                "230 Daniel McCabe"
            };

            ListView listView = new ListView
            {
                ItemsSource = listItems,
            };

            // Build the page.
            this.Content = new StackLayout
            {
                Padding = new Thickness(30,30,30,30),
                Children = {
                    listView
                }
            };

        }
	}
}