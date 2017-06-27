using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infragistics.XamarinForms;
using Xamarin.Forms;

namespace XFGridUpdates
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			_data = new GridTestData();
			BindingContext = _data;

			InitializeComponent();

			Task.Delay(33).ContinueWith((t) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Tick();
				});
			});
		}

		private Random _rand = new Random();
		private GridTestData _data;
		private DateTime _lastDataUpdate = DateTime.Now;

		private void Tick()
		{
			const int toChange = 1000; // number of data items to change

			HashSet<int> toChangeIndexes = new HashSet<int>();
			bool stillAnimating = _data.Any((item) => item.YearToDateSalesHeat > 0); // if any of the items are "hot" then we will still be animating their background color.

			if (!stillAnimating) // don't update anything if animation is in progress
			{
				_lastDataUpdate = DateTime.Now;
				for (var i = 0; i < toChange; i++)
				{
					int index = _rand.Next(_data.Count - 1);
					while (toChangeIndexes.Contains(index))
					{
						index = _rand.Next(_data.Count - 1);
					}
					toChangeIndexes.Add(index);
				}
			}

            // loop through the full dataset
            for (var i = 0; i < _data.Count; i++)
			{
				var item = _data[i];
				if (toChangeIndexes.Contains(i)) // if this item was flagged to change...
				{
					item.YearToDateSales += _rand.Next(4); // change its YearToDateSales to a random number.
					item.YearToDateSalesHeat = 1; // set its YearToDateSalesHeat to 1 to indicate the value was just changed.
				}
				else // if the item was not flagged to change...
				{
					if (item.YearToDateSalesHeat > 0) // if it has recently changed...
					{
                        // decrease its YearToDateSalesHeat until it reaches zero.
						item.YearToDateSalesHeat -= .03; 
						if (item.YearToDateSalesHeat < 0)
						{
							item.YearToDateSalesHeat = 0;
						}
					}
				}
			}

            // we have updated the YearToDateSales value of many datapoints.
            // if the grid is currently sorted this column, invoke the QueueAutoRefresh button to re-sort the grid.
            bool sortedBySales = grid.SortDescriptions.Any((sortDescription) => sortDescription.PropertyName == "YearToDateSales");
            if (sortedBySales && toChangeIndexes.Count > 0)
			{
				grid.ActualDataSource.QueueAutoRefresh();
			}
            
            // re-invoke this method to do another update.
			Task.Delay(33).ContinueWith((t) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					Tick();
				});
			});
		}

		private void yearToDateSalesColumn_DataBound(object sender, Infragistics.XamarinForms.Controls.Grids.DataBindingEventArgs args)
		{
            // use the DataBound event to perform contextual coloring of the YearToDateSales cells, based on their YearToDateSalesHeat values.
			var item = args.CellInfo.RowItem as IGSalesmanItem;
			if (item != null)
			{
				if (item.YearToDateSalesHeat > 0)
				{
					var p = item.YearToDateSalesHeat;
					var toA = 1.0;
					var fromA = 1.0;
					var toR = 1.0;
					var fromR = 1.0;
					var toG = 0.0;
					var fromG = 1.0;
					var toB = 0.0;
					var fromB = 1.0;

					var a = fromA + (toA - fromA) * p;
					var r = fromR + (toR - fromR) * p;
					var g = fromG + (toG - fromG) * p;
					var b = fromB + (toB - fromB) * p;

					var brush = new SolidColorBrush(Color.FromRgba(r, g, b, a));
					args.CellInfo.Background = brush;
				}
				else
				{
					args.CellInfo.Background = new SolidColorBrush(Color.White);
				}
			}
		}
	}
}
