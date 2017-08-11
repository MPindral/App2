using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    public class Question
    {

        //I needed this assistance in creating the JSON object from here http://json2csharp.com/

        public Question()
        {
        }

        public int id { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string help { get; set; }
        public List<string> options { get; set; }
        public List<string> optionVisuals { get; set; }
        public int? start { get; set; }
        public double? end { get; set; }
        public double? increment { get; set; }
        public string gradientStart { get; set; }
        public string gradientEnd { get; set; }
        public string validate { get; set; }
        public object answer { get; set; }
        public int? weighting { get; set; }
    }



    public class RootObject
    {

        public RootObject()
        {
        }

        public string id { get; set; }
        public string title { get; set; }
        public List<Question> questions { get; set; }
        public List<int?> questionsPerPage { get; set; }
        public int? score { get; set; }
    }


}
