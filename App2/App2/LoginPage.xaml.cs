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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}


        void txtPasswordChanged (object sender, TextChangedEventArgs e)
        {
            Entry txtPassword = sender as Entry;
            string password = txtPassword.Text;
        }
	}
}