using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App2
{
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
