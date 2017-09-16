using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Diagnostics;

using System.Drawing;
using System.IO;

namespace App2
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecordQuizAnswers : ContentPage
    {

        private TableView tblQuizes;
        TableSection section;
        ViewCell ViewCellHeader;
        private Slider sliderView;
        Label sliderPositionText;
        Label sliderPositionImage;
        List<string> OptionItems;
        List<string> OptionItemsImage;

        List<MultipleChoiceAnswer> multipleChoiceAnswers;

        AnswerManager myAnswerManager = new AnswerManager();

        public RecordQuizAnswers ()
		{
 
            InitializeComponent ();

            tblQuizes = new TableView
            {
                Intent = TableIntent.Data,
                Root = new TableRoot("Questions"),
                MinimumHeightRequest = 100,
                // I have to put this line below so that Xamarin can render rows of different heights.
                //Taken from https://forums.xamarin.com/discussion/17471/can-you-have-dynamic-cell-heights-with-either-the-listview-or-tableview-views
                HasUnevenRows = true             
            };

            Button btnSaveProgress = new Button
            {
                Text = "Save Progress",
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Xamarin.Forms.Color.FromHex("fa00ff")
            };

            btnSaveProgress.Clicked += (sender, e) =>
            {
                AnswerManager myAnswerManager = new AnswerManager();
                myAnswerManager.WriteAnswersToDisk();
            };

            Button btnSubmit = new Button
            {
                Text = "Submit",
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Xamarin.Forms.Color.FromHex("fa00ff")
            };

            section = new TableSection();
            
            this.Content = new StackLayout
            {
                Children = {
                    tblQuizes,
                    btnSaveProgress,
                    btnSubmit
                }
            };
            
            //Go and get the json data
            ReadQuiz();

        }


        private void ReadQuiz()
        {

            string jsonString = returnJsonString();

            List<RootObject> result = (List<RootObject>)JsonConvert.DeserializeObject(jsonString, typeof(List<RootObject>));

            foreach (var question in result)
            {

                if (question.id == ChooseQuiz.quizIdClicked)
                {

                    //This foreach populates only the quesion header
                    foreach (var item in question.questions)
                    {

                        Label questionId = new Label();
                        questionId.Text = ("Question: " + item.id + " - " + item.text).ToString();
                        questionId.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                        questionId.FontAttributes = FontAttributes.Bold;

                        Label help = new Label();
                        help.Text = ("Hint: " + item.help);

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
                                        Orientation = StackOrientation.Vertical,
                                        Children =
                                        {
                                            questionId,
                                            help
                                        }
                                    }
                                }
                            }
                        };


                        if (item.type == "date" || item.type == "Date" || item.type == "textbox")
                        {
                            //For single Line entries
                            Entry singleLineEntry = new Entry();

                            singleLineEntry.Text = myAnswerManager.ProvideAnswerText(item.id);

                            ViewCell ViewCellAnswer = new ViewCell()
                            {
                                View = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    VerticalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                        singleLineEntry
                                    }
                                }
                            };

                            singleLineEntry.Unfocused += (sender, e) =>
                            {
                                myAnswerManager.UpdateAnswerList(item.id, false, false, singleLineEntry.Text);

                            };
                           
                            section.Add(ViewCellHeader);
                            section.Add(ViewCellAnswer);

                        }


                        if (item.type == "textarea")
                        {
                            //For single Line entries
                            Editor multilineEditor = new Editor { HeightRequest = 50 };

                            multilineEditor.Text = myAnswerManager.ProvideAnswerText(item.id);

                            ViewCell ViewCellAnswer = new ViewCell()
                            {
                                View = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    Children =
                                    {
                                        multilineEditor
                                    }
                                }
                            };

                            multilineEditor.Unfocused += (sender, e) =>
                            {
                                myAnswerManager.UpdateAnswerList(item.id, false, false, multilineEditor.Text);
                            };

                            section.Add(ViewCellHeader);
                            section.Add(ViewCellAnswer);
                        }



                        if (item.type == "choice" || item.type == "options")
                        {
                            Picker pickerView = new Picker
                            {
                                Title = "Pick one",
                                BackgroundColor = Xamarin.Forms.Color.LightGray,
                                VerticalOptions = LayoutOptions.Center
                            };

                            foreach (var pickerItem in item.options)
                            {
                                pickerView.Items.Add(pickerItem);
                            }

                            String answerText = myAnswerManager.ProvideAnswerText(item.id);

                            if (answerText != "")
                            {
                                pickerView.SelectedIndex = Convert.ToInt32(answerText);
                            }

                            ViewCell ViewCellAnswer = new ViewCell()
                            {
                                View = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    Children =
                                    {
                                        pickerView
                                    }
                                }
                            };

                            pickerView.Unfocused += (sender, e) =>
                            {
                                myAnswerManager.UpdateAnswerList(item.id, false, false, pickerView.SelectedIndex.ToString());
                            };

                            section.Add(ViewCellHeader);
                            section.Add(ViewCellAnswer);
                        }

                        if (item.type == "slidingoption")
                        {

                            String answerText = myAnswerManager.ProvideAnswerText(item.id);
                            float answerPosition = 1.0f;

                            if (answerText != "")
                            {
                                answerPosition = float.Parse(answerText);
                            }

                            sliderView = new Slider
                            {
                                Minimum = 0.0f,
                                Maximum = 2.0f,
                                Value = answerPosition,
                                VerticalOptions = LayoutOptions.Center
                            };

                            OptionItems = new List<string>();
                            OptionItemsImage = new List<string>();

                            foreach (var optionItem in item.options)
                            {
                                OptionItems.Add(optionItem);
                            }

                            foreach (var optionItem in item.optionVisuals)
                            {
                                OptionItemsImage.Add(optionItem);
                            }

                            sliderPositionText = new Label
                            {
                                Text = OptionItems[Convert.ToInt32(sliderView.Value)]
                            };

                            sliderPositionImage = new Label
                            {
                                Text = OptionItemsImage[Convert.ToInt32(sliderView.Value)]
                            };

                            sliderView.ValueChanged += (sender, e) =>
                            {
                                //https://forums.xamarin.com/discussion/22473/can-you-limit-a-slider-to-only-allow-integer-values-hopefully-snapping-to-the-next-integer
                                var newStep = Math.Round(e.NewValue / 1.0);

                                sliderView.Value = newStep * 1.0;

                                sliderPositionText.Text = OptionItems[Convert.ToInt32(sliderView.Value)];
                                sliderPositionImage.Text = OptionItemsImage[Convert.ToInt32(sliderView.Value)];

                                myAnswerManager.UpdateAnswerList(item.id, false, false, sliderView.Value.ToString());

                            };

                            ViewCell ViewCellAnswer = new ViewCell()
                            {
                                View = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    Children =
                                    {
                                        sliderView,
                                        sliderPositionText,
                                        sliderPositionImage
                                    }
                                }
                            };

                            section.Add(ViewCellHeader);
                            section.Add(ViewCellAnswer);
                        }

                        if (item.type == "scale")
                        {

                            String answerText = myAnswerManager.ProvideAnswerText(item.id);
                            double answerPosition = Convert.ToDouble(item.end * 0.5f);

                            if (answerText != "")
                            {
                                answerPosition = double.Parse(answerText);
                            }

                            Stepper stepperView = new Stepper
                            {
                                Minimum = Convert.ToDouble(item.start),
                                Maximum = Convert.ToDouble(item.end),
                                Value = answerPosition,
                                VerticalOptions = LayoutOptions.Center,
                                Increment = Convert.ToDouble(item.increment)

                                //Here I was also wanting to utilise the colouring of the slider
                                //but I could not seem to bring up the MinimumTrackTintColor and
                                //MaximumTrackTintColor methods. If I could have done this then I
                                //would have applied the gradients.

                            };

                            Label stepperValue = new Label
                            {
                                Text = stepperView.Value.ToString()
                            };

                            StepperChange myStepperChange = new StepperChange();

                            myStepperChange.setLabel(stepperValue);
                            
                            //This is a hack here. I did it this way so that multiple steppers do not interfere with each other
                            stepperView.ValueChanged += (sender, e) =>
                            {
                                //See how I have pased on (sender, e)
                                myStepperChange.OnStepperValueChanged(sender, e);
                                myAnswerManager.UpdateAnswerList(item.id, false, false, stepperView.Value.ToString());
                            };

                            ViewCell ViewCellAnswer = new ViewCell()
                            {
                                View = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    Children =
                                    {
                                        stepperView,
                                        stepperValue
                                    }
                                }
                            };

                            section.Add(ViewCellHeader);
                            section.Add(ViewCellAnswer);

                        }


                        if (item.type == "multiplechoice")
                        {

                            //This approach was referenced from http://proquestcombo.safaribooksonline.com.ezproxy-b.deakin.edu.au/video/programming/mobile/9781771373371#

                            multipleChoiceAnswers = new List<MultipleChoiceAnswer>();

                            foreach (var optionItem in item.options)
                            {
                                multipleChoiceAnswers.Add(new MultipleChoiceAnswer(optionItem, myAnswerManager.ProvideAnswerMultipleChoice(item.id, optionItem)));
                                //false)); //here this false I need to get whether it is checked.
                            }

                            ListView listView = new ListView
                            {
                                ItemsSource = multipleChoiceAnswers,

                                ItemTemplate = new DataTemplate(() => {

                                    Label lblDescription = new Label();
                                    lblDescription.SetBinding(Label.TextProperty, "Description");

                                    Label lblIsChecked = new Label();
                                    lblIsChecked.SetBinding(Label.TextProperty, "IsCheckedString");

                                    Xamarin.Forms.Switch swIsChecked = new Xamarin.Forms.Switch();
                                    swIsChecked.SetBinding(Xamarin.Forms.Switch.IsToggledProperty, "isChecked");

                                    //This is to update the list of switches and the corresponding label.
                                    //It is a bit of a hack but it seems to be working OK.
                                    swIsChecked.Toggled += (sender, e) => // I got this line from https://stackoverflow.com/questions/32975894/xamarin-forms-switch-sends-toggled-event-when-value-is-updated
                                    {
                                        if (e != null)
                                        {
                                            var optionText = lblDescription.Text;

                                            foreach (var multianswer in multipleChoiceAnswers)
                                            {
                                                if (multianswer.Description == optionText)
                                                {
                                                    if (multianswer.isChecked == true)
                                                    {
                                                        multianswer.isChecked = false;
                                                        myAnswerManager.UpdateAnswerList(item.id, true, false, lblDescription.Text);
                                                            
                                                    }
                                                    else
                                                    {
                                                        multianswer.isChecked = true;
                                                        myAnswerManager.UpdateAnswerList(item.id, true, true, lblDescription.Text);
                                                    }
                                                    lblIsChecked.Text = multianswer.IsCheckedString;
                                                }
                                            }

                                        }

                                    };

                                    return new ViewCell
                                    {
                                        Height = 100,
                                        View = new StackLayout
                                        {
                                            Orientation = StackOrientation.Horizontal,
                                            HorizontalOptions = LayoutOptions.StartAndExpand,
                                            Children = {
                                new StackLayout {
                                    VerticalOptions = LayoutOptions.Center,
                                    Spacing = 0,
                                    Children = {
                                        swIsChecked
                                    }
                                },
                                new StackLayout {
                                    VerticalOptions = LayoutOptions.Center,
                                    Spacing = 0,
                                    Children = {
                                        lblDescription,
                                        lblIsChecked
                                    }
                                }
                            }
                                        }
                                    };
                                }


                                )
                            };


                            ViewCell ViewCellAnswer = new ViewCell()
                            {
                                View = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    Children =
                                    {
                                        listView
                                    }
                                }
                            };

                            section.Add(ViewCellHeader);
                            section.Add(ViewCellAnswer);

                        }


                    }

                }


            }
            tblQuizes.Root.Add(section);
            //tblQuizes.Root.Add(new TableSection[] { section });
        }

        private void sliderValueChanged(object sender, ValueChangedEventArgs e)
        {

            //https://forums.xamarin.com/discussion/22473/can-you-limit-a-slider-to-only-allow-integer-values-hopefully-snapping-to-the-next-integer
            var newStep = Math.Round(e.NewValue / 1.0);

            sliderView.Value = newStep * 1.0;
            
            sliderPositionText.Text = OptionItems[Convert.ToInt32(sliderView.Value)];
            sliderPositionImage.Text = OptionItemsImage[Convert.ToInt32(sliderView.Value)];

            

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