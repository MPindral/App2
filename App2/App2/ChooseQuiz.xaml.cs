using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Diagnostics;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseQuiz : ContentPage
	{

        private TableView tblQuizes;

        public ChooseQuiz ()
		{
			InitializeComponent ();

            Label header = new Label
            {
                Text = "Choose Quiz",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)), //I utilised this URL for this line https://forums.xamarin.com/discussion/30207/whats-the-deal-with-xamarin-forms-label-font-is-obsolete
                HorizontalTextAlignment = TextAlignment.Center,
                BackgroundColor = Color.DarkBlue,
                TextColor = Color.White
            };

            tblQuizes = new TableView
            {
                Intent = TableIntent.Data,
                Root = new TableRoot("Questions")
            };

            this.Content = new StackLayout
            {
                Children = {
                    header,
                    tblQuizes
                }
            };

            Debug.WriteLine("Before ReadQuiz:");

            //Go and get the json data
            ReadQuiz();

        }

        private void ReadQuiz()
        {

            string jsonString = returnJsonString();

            List<RootObject> result = (List<RootObject>)JsonConvert.DeserializeObject(jsonString,typeof (List<RootObject>));

            TableSection section = new TableSection("Questions");

            foreach (RootObject ro in result)
            {
                section.Add(new TextCell
                {
                    Text = (ro.id).ToString(),
                    TextColor = Color.Black,
                    Detail = ro.title,
                    DetailColor = Color.Navy
                }
                
                );
                
            }


            tblQuizes.Root.Add(new TableSection[] { section });
        }


        public string returnJsonString()
        {
            string jsonString;
            
            /**
             * If you want to enter a new json string, then you paste it below.
             * You will need to have double quotation marks for the string to read properly.
             * Simply highlight your new string and do find and replace all single quotations with a double.
             **/

            jsonString = @"
            
                    [
          {
            ""id"": ""quiz01"",
            ""title"": ""Mood Survey"",
            ""questions"": [
              {
                ""id"": 1,
                ""text"": ""Date"",
                ""type"": ""date"",
                ""help"": ""The date you started this quiz.""
              },
              {
                ""id"": 2,
                ""text"": ""Name"",
                ""type"": ""textbox"",
                ""help"": ""Your full name""
              },
              {
                ""id"": 3,
                ""text"": ""Diary"",
                ""type"": ""textarea"",
                ""help"": ""Write 4 paragraphs""
              },
              {
                ""id"": 4,
                ""text"": ""Gender"",
                ""type"": ""choice"",
                ""options"": [ ""Male"", ""Female"", ""Depends what day it is"" ]
              },
              {
                ""id"": 5,
                ""text"": ""Mood"",
                ""type"": ""slidingoption"",
                ""options"": [ ""Sad"", ""Happy"", ""Laughing"" ],
                ""optionVisuals"": [ ""😭"", ""☺️"", ""😆"" ]
              },
              {
                ""id"": 6,
                ""text"": ""Happiness Today"",
                ""type"": ""scale"",
                ""start"": 0,
                ""end"": 10,
                ""increment"": 1,
                ""gradientStart"": ""#ff0000"",
                ""gradientEnd"": ""#00ff00""
              },
              {
                ""id"": 7,
                ""text"": ""Blood Alcohol"",
                ""type"": ""scale"",
                ""start"": 0,
                ""end"": 0.5,
                ""increment"": 0.01
              }
            ]
          },

          {
            ""id"": ""quiz02"",
            ""title"": ""Exam Grade"",
            ""questionsPerPage"": [ 2, 4 ],
            ""score"": 20,
            ""questions"": [
              {
                ""id"": 1,
                ""text"": ""Sid"",
                ""type"": ""textbox"",
                ""validate"": ""/[0-9]+/""
              },
              {
                ""id"": 2,
                ""text"": ""Name"",
                ""type"": ""textbox"",
                ""help"": ""Your full name""
              },
              {
                ""id"": 3,
                ""text"": ""What is the capital of Australia?"",
                ""type"": ""textbox"",
                ""answer"": ""Australia"",
                ""weighting"": 5
              },
              {
                ""id"": 4,
                ""text"": ""What is the largest state in Australia?"",
                ""type"": ""textbox"",
                ""answer"": [ ""Western Australia"", ""WA"" ],
                ""weighting"": 5
              },
              {
                ""id"": 5,
                ""text"": ""What is the capital of Victoria?"",
                ""type"": ""choice"",
                ""options"": [ ""Sydney"", ""Brisbane"", ""Melbourne"" ],
                ""answer"": ""Melbourne"",
                ""weighting"": 5
              },
              {
                ""id"": 6,
                ""text"": ""Which are the territories of Australia?"",
                ""type"": ""multiplechoice"",
                ""options"": [ ""ACT"", ""NSW"", ""NT"", ""QLD"", ""SA"", ""TAS"", ""VIC"", ""WA"" ],
                ""answer"": [ ""ACT"", ""NT"" ],
                ""weighting"": 5
              }

            ]
          }
        ]";

            return jsonString;//""😭"",""☺️"",""😆""
        }



    }
}