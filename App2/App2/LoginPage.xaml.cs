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
        private TableView tblExistingUsers;
        public static string currentUsername;
        ViewCell ViewCellHeader;

        public LoginPage()
        {
            InitializeComponent();

            Label usernameLabel = new Label
            {
                Text = "Create new user"
            };

            Entry usernameEntry = new Entry
            {
                Placeholder = "Please enter your name"
            };

            Button btnLogin = new Button
            {
                Text = "Login",
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromHex("fa00ff")
            };

            btnLogin.Clicked += (sender, e) =>
            {
                currentUsername = usernameEntry.Text;

                //Take them to the choose quiz
                openChooseQuiz();
            };

            Label existingUserLabel = new Label
            {
                Text = "Continue with an existing user"
            };


            tblExistingUsers = new TableView
            {
                Intent = TableIntent.Data,
                Root = new TableRoot()
            };

            this.Content = new StackLayout
            {
                Children = {
                    usernameLabel,
                    usernameEntry,
                    btnLogin,
                    existingUserLabel,
                    tblExistingUsers
                }
            };

            //Populate the list of users
            PopulateUserList();

        }

        private void PopulateUserList()
        {
            AnswerManager myAnswerManager = new AnswerManager();

            List<string> existingUsers = myAnswerManager.GetAllUserNames();

            TableSection section = new TableSection();

            foreach (String user in existingUsers)
            {

                Label lblUser = new Label();
                lblUser.Text = user;
                lblUser.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

                ViewCellHeader = new ViewCell()
                {
                    View = new StackLayout
                    {
                        Margin = new Thickness(0, 10, 0, 0),
                        Orientation = StackOrientation.Vertical,
                        VerticalOptions = LayoutOptions.Center,
                        Children =
                                {
                                    new StackLayout
                                    {
                                        Orientation = StackOrientation.Horizontal,
                                        Children =
                                        {
                                            lblUser
                                        }
                                    }
                                }
                    }
                };

                ViewCellHeader.Tapped += (sender, e) =>
                {
                    //record which row item was clicked.
                    //I beleieve that there should be a method of passing data to a new page
                    //Like you can in iOS and Android but I was unable to find the Xamarin equivalent.

                    List<string> detailsClicked = user.Split('_').ToList<string>();

                    currentUsername = detailsClicked[0];
                    ChooseQuiz.quizIdClicked = detailsClicked[1];

                    //Open the record quiz page
                    openRecordQuizAnswers();
                };

                //Add the viewcell to the section
                section.Add(ViewCellHeader);

            }

            //Add the section to the tableview
            tblExistingUsers.Root.Add(new TableSection[] { section });

        }

        private async void openRecordQuizAnswers()
        {

            //I have to put this text here to make sure the Answer List is ready when the RecordQuiz Answers page is opened.
            AnswerManager myAnswerManager = new AnswerManager();
            myAnswerManager.populateAnswers();

            await Navigation.PushAsync(new RecordQuizAnswers());

        }


        private async void openChooseQuiz()
        {
            await Navigation.PushAsync(new ChooseQuiz());
        }


    }
}