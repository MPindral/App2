using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2
{
    //I created this class because having multiple steppers ont he same page caused them to get confused with each other.
    //I therefore created a separate reference to each steppr and the problem was fixed.

    class StepperChange
    {
        public Label mylabel;

        public void setLabel(Label externalLabel)
        {
            mylabel = externalLabel;
        }

        public void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            mylabel.Text = e.NewValue.ToString();



        }

    }
}
