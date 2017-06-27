using System; 
using Infragistics.XamarinForms.Controls.Charts; 
using Xamarin.Forms;

namespace BlankAppPort
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
              
            this.BindingContext = new PopulationViewModel();

            BarXAxis.FormatLabel += BarXAxis_FormatLabel;
            BarYAxis.FormatLabel += BarYAxis_FormatLabel;
            RadiusAxis.FormatLabel += RadiusAxis_FormatLabel;
        } 

        private void RadiusAxis_FormatLabel(object sender, AxisFormatLabelEventArgs e)
        {
            var value = (double)e.Item;
            e.Label = value.ToString("0M");
        }


        private void BarXAxis_FormatLabel(object sender, AxisFormatLabelEventArgs e)
        {
            var value = Math.Abs((double)e.Item);
            e.Label = value.ToString("0M");
        }

        private void BarYAxis_FormatLabel(object sender, AxisFormatLabelEventArgs e)
        { 
            var item = e.Item as PopulationData;

            if (item.Age < 100)
                e.Label = item.Age.ToString("0");
            else 
                e.Label = item.Age.ToString("0") + "+";
        } 


    }
}
