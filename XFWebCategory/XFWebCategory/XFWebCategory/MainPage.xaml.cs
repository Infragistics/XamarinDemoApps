using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Infragistics.XamarinForms.Controls.Charts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace XFWebCategory
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			//var categoryChart = new XamCategoryChart();
			//Content = categoryChart;


			GetData();
		}

		

		private async void GetData()
		{
			HttpClient c = new HttpClient();
			var str = await c.GetStringAsync("http://api.population.io/1.0/population/2010/United%20States/?format=json");
			var data = await Task.Run(
				() => JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(str));
			Device.BeginInvokeOnMainThread(() =>
			{
				this.BindingContext = data;
			});
		}

		
	}
}
