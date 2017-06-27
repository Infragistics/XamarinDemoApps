using System; 
using System.ComponentModel; 
using Newtonsoft.Json;

namespace BlankAppPort
{ 
    [JsonObject(MemberSerialization.OptIn)]
    public class PopulationData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Constructors
        public PopulationData()
        {
        }

        public PopulationData(int age, int year, double males, double females)
        {
            this.Age = age;
            this.Year = year;
            this.Males = males;
            this.Females = females;
            Update();
        }

        public PopulationData(int start, int end, string name)
        {
            this.GenerationStart = start;
            this.GenerationEnd = end;
            this.Name = name;
        }
        #endregion

        #region Properties
        [JsonProperty(PropertyName = "females")]
        public double Females
        {
            get { return _Females; }
            set
            {
                if (_Females == value) return; _Females = value;
                OnPropertyChanged("Females"); Update();
            }
        }
        private double _Females;

        [JsonProperty(PropertyName = "males")]
        public double Males
        {
            get { return _Males; }
            set
            {
                if (_Males == value) return; _Males = value;
                OnPropertyChanged("Males"); Update();
            }
        }
        private double _Males;

        [JsonProperty(PropertyName = "age")]
        public int Age { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        public int BirthYear { get { return Year - Age; } }

        public string Name { get; set; }

        public int GenerationStart { get; set; }
        public int GenerationEnd { get; set; }

        public string GenerationRange { get { return GenerationStart + "-" + GenerationEnd; } }

        public double Total { get; private set; }
         
        public double FemalesInMillions
        {
            get { return _FemalesInMillions; }
            set
            {
                if (_FemalesInMillions == value) return; _FemalesInMillions = value;
                OnPropertyChanged("FemalesInMillions");
            }
        }
        private double _FemalesInMillions;
         
        public double MalesInMillions
        {
            get { return _MalesInMillions; }
            set
            {
                if (_MalesInMillions == value) return; _MalesInMillions = value;
                OnPropertyChanged("MalesInMillions");
            }
        }
        private double _MalesInMillions;
         
        #endregion

        public void Update()
        {
            if (double.IsNaN(Males) || double.IsNaN(Females)) return;

            Total = Males + Females;

            // converting meles to negative values to plot them as opposite values to females
            MalesInMillions = -Math.Round(Males / 1000000, 1);
            FemalesInMillions = Math.Round(Females / 1000000, 1);

            OnPropertyChanged("Total");
        }

        public PopulationData Clone()
        {
            return new PopulationData(this.Age, this.Year, this.Males, this.Females);
        }
        public override string ToString()
        {
            return Year + " A=" + Age + " M=" + MalesInMillions + " F=" + FemalesInMillions;
        }
    }

}
