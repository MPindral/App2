﻿using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{


    //Reference from http://proquestcombo.safaribooksonline.com.ezproxy-b.deakin.edu.au/video/programming/mobile/9781771373371#
    public class MultipleChoiceAnswer
    {

        public MultipleChoiceAnswer(string Description, bool isChecked)
        {
            this.Description = Description;
            this.isChecked = isChecked;
        }

        public string Description { set; get; }

        public bool isChecked { set; get; }

        public string IsCheckedString { get { return isChecked ? "Selected" : "Not Selected"; } }


    }
}
