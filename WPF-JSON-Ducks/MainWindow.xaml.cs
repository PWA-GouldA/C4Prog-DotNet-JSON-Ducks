using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;

/// <summary>
/// JSON Data save and Load demo
/// 
/// Demo contains following:
///     JSON Serialisation
///     List of Objects
///     JSON Save
///     JSON Load
///     Combo box with HTML colour names, samples and Hex codes
///     Add to list
///     
/// Resources used:
///     https://www.newtonsoft.com/json
///     http://csharphelper.com/blog/2015/10/list-colors-in-wpf-and-c/
///     http://flounder.com/csharp_color_table.htm
///     https://stackoverflow.com/questions/32489262/how-to-fill-wpf-combobox-with-text-image-at-runtime
///     http://www.blackwasp.co.uk/StatusBar.aspx
///     https://www.wpf-tutorial.com/common-interface-controls/menu-control/
///     https://www.wpf-tutorial.com/listview-control/listview-data-binding-item-template/
///     https://www.wpf-tutorial.com/common-interface-controls/statusbar-control/
///     
/// </summary>

namespace WPF_JSON_Ducks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string duckFileName = "ducks.json";

        List<Duck> ducks = new List<Duck>();

        public MainWindow()
        {
            InitializeComponent();
            AddColours();
            AddSizes();
            AddSampleDucks();
        }

        void LoadDucks()
        {
            ducks.Clear();
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(duckFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                ducks = (List<Duck>)serializer.Deserialize(file, typeof(List<Duck>));
            }
            DuckListView.ItemsSource = ducks;
        }

        void SaveDucks()
        {
            // serialize JSON to a string and then write string to a file
            //File.WriteAllText(@duckFileName, JsonConvert.SerializeObject(ducks));

            // serialize JSON directly to a file
            using (StreamWriter file = File.CreateText(@duckFileName))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, ducks);
            }
        }

        private void NameTextbox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int row = NameTextbox.GetLineIndexFromCharacterIndex(NameTextbox.CaretIndex);
            int col = NameTextbox.CaretIndex - NameTextbox.GetCharacterIndexFromLineIndex(row);
            CursorPositionLabel.Text = "Line " + (row + 1) + ", Char " + (col + 1);
        }

        void AddSampleDucks()
        {
            if (!File.Exists(duckFileName))
            {

                ducks.Add(new Duck()
                {
                    Name = "Yellow Duck",
                    Colour = "Yellow",
                    Size = DuckSizes.Egg
                }
            );
                //ducks.Add(new Duck() { Name = "Green Duck", Colour = "Green", Size = DuckSizes.Duckling });
                //ducks.Add(new Duck() { Name = "Cyan Duck", Colour = "Cyan", Size = DuckSizes.Small });
                //ducks.Add(new Duck() { Name = "Magenta Duck", Colour = "Magenta", Size = DuckSizes.Medium });
                //ducks.Add(new Duck() { Name = "Blue Duck", Colour = "Blue", Size = DuckSizes.Large });
                //ducks.Add(new Duck() { Name = "Red Duck", Colour = "Red", Size = DuckSizes.Supersized });
                //ducks.Add(new Duck() { Name = "Purple Duck", Colour = "Purple", Size = DuckSizes.Ermagerd });
                //ducks.Add(new Duck() { Name = "Black Duck", Colour = "Black", Size = DuckSizes.Error });
            }
            else
            {
                LoadDucks();
            }
            DuckListView.ItemsSource = ducks;
        }


        private void LoadDucksButton_Click(object sender, RoutedEventArgs e)
        {
            ducks.Clear();
            LoadDucks();
            DuckListView.Items.Refresh();
        }

        private void SaveDucksButton_Click(object sender, RoutedEventArgs e)
        {
            SaveDucks();
        }

        private void ClearDucksButton_Click(object sender, RoutedEventArgs e)
        {
            ducks.Clear();
            DuckListView.Items.Refresh();
        }

        private void AddDuckButton_Click(object sender, RoutedEventArgs e)
        {
            string duckName = NameTextbox.Text;
            string duckColour = ((ColourInfo)ColourComboBox.SelectedValue).ColourName;
            DuckSizes duckSize = (DuckSizes) SizeComboBox.SelectedIndex;

            ducks.Add(new Duck(duckName, duckColour, duckSize,0));

            DuckListView.Items.Refresh();
        }

        void AddSizes()
        {
            SizeComboBox.ItemsSource = Enum.GetValues(typeof(DuckSizes));
        }

        void AddColours()
        {
            var color_query =
                from PropertyInfo property in typeof(Colors).GetProperties()
                orderby property.Name
                //orderby ((Color)property.GetValue(null, null)).ToString()
                select new ColourInfo(
                    property.Name,
                    (Color)property.GetValue(null, null));
            ColourComboBox.ItemsSource = color_query;
        }

    }

    
}