using System; 
using System.Collections.Generic; 
using System.ComponentModel;  
using System.Net.Http; 
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace BlankAppPort
{
    public class PopulationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

            if (name == "IsUpdatingData" && this.IsUpdatingData)
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(this.UpdateInterval), TimerCallback);
            }
        }
                   
        public PopulationViewModel()
        {
            PopulationLookup = new Dictionary<int, List<PopulationData>>();

            // initalize population with default values for each age of population
            PopulationByAge = new List<PopulationData>();
            for (var age = 0; age <= 100; age++)
            {
                PopulationByAge.Add(new PopulationData(age, HistoryStart, 0, 0));
            }

            // initalize population with default values for each generation of population
            PopulationByGen = new List<PopulationData>();
            PopulationByGen.Add(new PopulationData(1820, 1900, "1900" ));
            PopulationByGen.Add(new PopulationData(1900, 1930, "1930" ));
            PopulationByGen.Add(new PopulationData(1930, 1945, "Silent" ));
            PopulationByGen.Add(new PopulationData(1945, 1965, "Boomers" ));
            PopulationByGen.Add(new PopulationData(1965, 1980, "X"));
            PopulationByGen.Add(new PopulationData(1980, 2000, "Millennials"));
            PopulationByGen.Add(new PopulationData(2000, 2025, "2025" ));
            PopulationByGen.Add(new PopulationData(2025, 2050, "2050" ));
            PopulationByGen.Add(new PopulationData(2050, 2075, "2075" ));
            PopulationByGen.Add(new PopulationData(2075, 2100, "2100" ));

            GetHistory(); 
        }

        protected Dictionary<int, List<PopulationData>> PopulationLookup;

        private List<PopulationData> _PopulationByAge;
        public List<PopulationData> PopulationByAge
        {
            get { return _PopulationByAge; }
            set
            {
                if (_PopulationByAge == value) return; _PopulationByAge = value;
                OnPropertyChanged("PopulationByAge");
            }
        }

        private List<PopulationData> _PopulationByGen;
        public List<PopulationData> PopulationByGen
        {
            get { return _PopulationByGen; }
            set
            {
                if (_PopulationByGen == value) return; _PopulationByGen = value;
                OnPropertyChanged("PopulationByGen");
            }
        }

        private int _UpdateInterval = 750;
        /// <summary> Gets or sets UpdateInterval </summary>
        public int UpdateInterval
        {
            get { return _UpdateInterval; }
            set { if (_UpdateInterval == value) return; _UpdateInterval = value; OnPropertyChanged("UpdateInterval"); }
        } 

        private bool _IsUpdatingData = true;
        /// <summary> Gets or sets IsUpdatingData </summary>
        public bool IsUpdatingData
        {
            get { return _IsUpdatingData; }
            set
            {
                if (_IsUpdatingData == value) return; _IsUpdatingData = value;
                OnPropertyChanged("IsUpdatingData"); OnPropertyChanged("IsEditableYear");
            }
        }
         
        /// <summary> Gets IsEditableYear </summary>
        public bool IsEditableYear 
        {
            get { return !this.IsUpdatingData; } 
        } 

        private int _CurrentYear;
        public int CurrentYear
        {
            get { return _CurrentYear; }
            set
            {
                if (value == _CurrentYear) return;
                if (value > HistoryStop) value = HistoryStart;
                if (value < HistoryStart) value = HistoryStart;

                if (PopulationLookup.ContainsKey(value))
                {
                    _CurrentYear = value;
                    OnPropertyChanged("CurrentYear"); 
                    UpdateData(); 
                }
            }
        }
         
        // values controling range of data retrived from population service  
        protected int HistoryStop = 2100;
        protected int HistoryStart = 1950;
        protected int HistoryInterval = 10;

        protected HttpClient Client = new HttpClient();
        protected string Source = "http://api.population.io/1.0/population/{0}/United%20States/?format=json";

        public async Task<List<PopulationData>> GetData(int year)
        {
            var url = string.Format(Source, year);
            var str = await Client.GetStringAsync(url);
            var population = await Task.Run(
                () => JsonConvert.DeserializeObject<List<PopulationData>>(str));

            return population;
        }

        private async void GetHistory()
        {  
            for (int year = HistoryStart; year <= HistoryStop; year += HistoryInterval)
            {                
                var data = await GetData(year);
                
                PopulationLookup.Add(year, data);
            }
              
            CurrentYear = 1950; 
             
            Device.StartTimer(new TimeSpan(0, 0, 0, 0, 400), TimerCallback);
        }

        private bool TimerCallback()
        {
            CurrentYear += HistoryInterval;

            return IsUpdatingData;
        }

        private void UpdateData()
        { 
            for (int i = 0; i < PopulationLookup[CurrentYear].Count; i++)
            {
                PopulationByAge[i].Age = PopulationLookup[CurrentYear][i].Age;
                PopulationByAge[i].Year = PopulationLookup[CurrentYear][i].Year;
                PopulationByAge[i].Males = PopulationLookup[CurrentYear][i].Males;
                PopulationByAge[i].Females = PopulationLookup[CurrentYear][i].Females;
            }

            foreach (var generation in PopulationByGen)
            { 
                var males = 0.0;
                var females = 0.0;
                foreach (var population in PopulationLookup[CurrentYear])
                {
                    if (population.BirthYear > generation.GenerationStart &&
                        population.BirthYear <= generation.GenerationEnd)
                    {
                        females += population.Females;
                        males += population.Males; 
                    }
                } 
                generation.MalesInMillions = males / 1000000;
                generation.FemalesInMillions = females / 1000000;
            } 
        }
    }

}
