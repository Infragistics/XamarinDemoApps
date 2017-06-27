using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace XFGridUpdates
{
	public class GridTestData
		: List<IGSalesmanItem>
	{
		public GridTestData()
		{
			foreach (var item in IGSalesmanItem.GenerateData(5000))
			{
				this.Add(item);
			}
		}
	}

	public class BigGridTestData
		: List<IGSalesmanItem>
	{
		public BigGridTestData()
		{
			foreach (var item in IGSalesmanItem.GenerateData(500000))
			{
				this.Add(item);
			}
		}
	}

	public class IGSalesmanItem
		: INotifyPropertyChanged
	{
		public IGSalesmanItem()
		{
		}

		private int _index;
		private string _imageName;
		private string _territory;
		private string _lastName;
		private string _firstName;
		private int _yearToDateSales;
		private double _yearToDateSalesHeat;
		private DateTime _dateValue;

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public string ImageName
		{
			get
			{
				return _imageName;
			}

			set
			{
				_imageName = value;
				OnPropertyChanged(nameof(ImageName));
			}
		}

		public int Index
		{
			get
			{
				return _index;
			}

			set
			{
				_index = value;
				OnPropertyChanged(nameof(Index));
			}
		}

		public string Territory
		{
			get
			{
				return _territory;
			}

			set
			{
				_territory = value;
				OnPropertyChanged(nameof(Territory));
			}
		}

		public string LastName
		{
			get
			{
				return _lastName;
			}

			set
			{
				_lastName = value;
				OnPropertyChanged(nameof(LastName));
			}
		}

		public string FirstName
		{
			get
			{
				return _firstName;
			}

			set
			{
				_firstName = value;
				OnPropertyChanged(nameof(FirstName));
			}
		}

		public int YearToDateSales
		{
			get
			{
				return _yearToDateSales;
			}

			set
			{
				_yearToDateSales = value;
				OnPropertyChanged(nameof(YearToDateSales));
			}
		}

		public double YearToDateSalesHeat
		{
			get
			{
				return _yearToDateSalesHeat;
			}

			set
			{
				_yearToDateSalesHeat = value;
				OnPropertyChanged(nameof(YearToDateSalesHeat));
			}
		}

		public DateTime DateValue
		{
			get
			{
				return _dateValue;
			}

			set
			{
				_dateValue = value;
				OnPropertyChanged(nameof(DateValue));
			}
		}

		public string LongText
		{
			get
			{
				return "Long Long Long Long";
			}
		}

		private static string CreateLocalUri(string image)
		{
			return image;
			//return "XFGridDemo.People." + image;
		}

		private static string CreateRemoteUri(string image)
		{
			return @"http://10.20.12.56/People/" + image;
		}

		public static List<IGSalesmanItem> GenerateData(int count)
		{

			String[] firstNames = {
				"Kyle",
				"Gina",
				"Irene",
				"Katie",
				"Michael",
				"Oscar",
				"Ralph",
				"Torrey",
				"William",
				"Bill",
				"Daniel",
				"Frank",
				"Brenda",
				"Danielle",
				"Fiona",
				"Howard",
				"Jack",
				"Larry",
				"Holly",
				"Jennifer",
				"Liz",
				"Pete",
				"Steve",
				"Vince",
				"Zeke"
		};

			String[] lastNames = {
				"Adams",
				"Crowley",
				"Ellis",
				"Gable",
				"Irvine",
				"Keefe",
				"Mendoza",
				"Owens",
				"Rooney",
				"Waddell",
				"Thomas",
				"Betts",
				"Doran",
				"Fitzgerald",
				"Holmes",
				"Jefferson",
				"Landry",
				"Newberry",
				"Perez",
				"Spencer",
				"Vargas",
				"Grimes",
				"Edwards",
				"Stark",
				"Cruise",
				"Fitz",
				"Chief",
				"Blanc",
				"Perry",
				"Stone",
				"Williams",
				"Lane",
				"Jobs"
		};

			String[] genders = {
				"GUY",
				"GIRL",
				"GIRL",
				"GIRL",
				"GUY",
				"GUY",
				"GUY",
				"GUY",
				"GUY",
				"GUY",
				"GUY",
				"GUY",
				"GIRL",
				"GIRL",
				"GIRL",
				"GUY",
				"GUY",
				"GUY",
				"GIRL",
				"GIRL",
				"GIRL",
				"GUY",
				"GUY",
				"GUY",
				"GUY"
		};

			String[] territories = {
				"Australia",
				"Canada",
				"Egypt",
				"Greece",
				"Italy",
				"Kenya",
				"Mexico",
				"Oman",
				"Qatar",
				"Sweden",
				"Uruguay",
				"Yemen",
				"Bulgaria",
				"Denmark",
				"France",
				"Hungary",
				"Japan",
				"Latvia",
				"Netherlands",
				"Portugal",
				"Russia",
				"Turkey",
				"Venezuela",
				"Zimbabwe"
		};

			int min = 10;
			int max = 35;

			Random r = new Random();
			//int fontSize = r.nextInt(max - min + 1) + min;

			List<IGSalesmanItem> items = new List<IGSalesmanItem>();
			for (int i = 0; i < count; i++)
			{


				IGSalesmanItem item = new IGSalesmanItem();
				int firstIndex = r.Next(firstNames.Length - 1);
				item.Index = i;
				item.FirstName = firstNames[firstIndex];
				item.LastName = lastNames[r.Next(lastNames.Length - 1)];


				int randomIndex = r.Next(firstNames.Length - 1);
				if (randomIndex == 0)
					randomIndex = 1;

				string number = randomIndex.ToString();
				if (randomIndex < 10)
					number = "0" + number;
				item.ImageName = CreateLocalUri(genders[firstIndex] + number + ".png");
				item.Territory = territories[r.Next(territories.Length - 1)];
				item.YearToDateSales = r.Next(50000);

				item.DateValue = DateTime.Now.AddDays(r.Next(500));

				items.Add(item);
			}

			return items;
		}
	}

	
}
