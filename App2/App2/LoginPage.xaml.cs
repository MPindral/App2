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
	public partial class LoginPage : ContentPage
	{

        public static string currentUsername;

        public LoginPage()
        {
            InitializeComponent();

            Label usernameLabel = new Label
            {
                Text = "Username"
            };

            Entry usernameEntry = new Entry
            {
                Placeholder = "Please enter your name"
            };

            Button btnLogin = new Button
            {
                Text = "Login",
                HorizontalOptions = LayoutOptions.Center
            };

            btnLogin.Clicked += (sender, e) =>
            {
                currentUsername = usernameEntry.Text;

                Debug.WriteLine("CurrentUserName: "+ currentUsername);
            };

            this.Content = new StackLayout
            {
                Children = {
                    usernameLabel,
                                        usernameEntry,
                                        btnLogin
                }
            };

        }
	}
}