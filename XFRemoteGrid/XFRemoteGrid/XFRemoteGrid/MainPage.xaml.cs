using Infragistics.Controls.DataSource;
using Xamarin.Forms;

namespace XFRemoteGrid
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();


			var dataSource = new ODataVirtualDataSource()
			{
				BaseUri = "http://services.odata.org/V4/Northwind/Northwind.svc",
				EntitySet = "Orders",
				PageSizeRequested = 25,
				MaxCachedPages = 5
			};

			this.BindingContext = dataSource;
		}
	}
}
