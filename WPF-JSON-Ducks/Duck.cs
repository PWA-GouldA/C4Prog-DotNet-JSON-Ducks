using System;
using System.ComponentModel;
using System.Windows.Media;
using WPF_JSON_Ducks;

namespace WPF_JSON_Ducks
{
    enum DuckSizes
    {
        Error = -1,
        Egg = 0,
        Duckling = 1,
        Small = 2,
        Medium = 3,
        Large = 4,
        ExtraLarge = 5,
        Supersized = 6,
        Ermagerd = 7
    }

    public class ColourInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string ColourName { get; set; }
        public Color Colour { get; set; }

        public SolidColorBrush SampleBrush
        {
            get { return new SolidColorBrush(Colour); }
        }
        public string HexValue
        {
            get { return Colour.ToString(); }
        }

        public ColourInfo(string colour_name, Color colour)
        {
            ColourName = colour_name;
            Colour = colour;
        }
    }

    class Duck
    {
        private Guid _id;
        private string _name;
        private string _colour;
        private int _birthYear;
        private DuckSizes _size;


        public Guid ID
        {
            get { return _id; }
            set { _id = value; }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }

        public SolidColorBrush ColourSample
        {
            get {
                SolidColorBrush xx = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(Colour)
                    );
                return xx;
            }
        }

        public DuckSizes Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int BirthYear
        {
            get { return _birthYear; }
            set { _birthYear = value; }
        }
        public Duck()
        {
            ID = System.Guid.NewGuid();
            Name = "";
            Colour = "";
            Size = DuckSizes.Error;
            BirthYear = 0;
        }

        public Duck(string name, string colour, DuckSizes size, int birthYear)
        {
            ID = Guid.NewGuid();
            this.Name = name;
            this.Colour = colour;
            Size = size;
            this.BirthYear = birthYear;
        }

        public Duck(string name, Color colour, DuckSizes size, int birthYear)
        {
            ID = Guid.NewGuid();
            this.Name = name;
            this.Colour = colour.ToString();
            Size = size;
            this.BirthYear = birthYear;
        }

        private DuckSizes ConvertToDucksize(string size)
        {
            DuckSizes duckSized;
            Enum.TryParse<DuckSizes>(size, out duckSized);
            return duckSized;
        }


        // override public string ToString() {
        //  return Name + " (" + Colour + ") " + SizeText();
        // }

        override public string ToString() => Name + " (" + Colour + ") " + Enum.GetName(typeof(DuckSizes), Size);


    }
}
