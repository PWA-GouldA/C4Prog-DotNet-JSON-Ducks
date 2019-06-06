using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
///     Duck Icon: https://www.flaticon.com/authors/smashicons from https://www.flaticon.com/ 
///                licensed by http://creativecommons.org/licenses/by/3.0/ Creative Commons BY 3.0
///     https://www.technical-recipes.com/2016/setting-the-application-icon-in-wpf-xaml/
///     https://www.zamzar.com/convert/png-to-ico/
///     
/// </summary>

namespace WPF_JSON_Ducks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        string clusterFolder = "C4ProgS1";
        string duckFolder = "DuckDemo";
        string duckFileName = "ducks.json";
        string duckDirectoryPath = ""; // this is set by the ConfigureFoldersAndFiles method.
        string fullDuckFilePath = ""; // this is set by the ConfigureFoldersAndFiles method.

        bool duckListChanged = false; // this is updated if you add/edit/remove a duck

        List<Duck> ducks = new List<Duck>();

        public MainWindow()
        {
            InitializeComponent();
            ConfigureFoldersAndFiles();
            AddColours();
            AddSizes();
            AddSampleDucks();
        }

        void LoadDucks()
        {
            ducks.Clear();
            // deserialize JSON directly from a file
            using (StreamReader file = File.OpenText(fullDuckFilePath))
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
            using (StreamWriter file = File.CreateText(@fullDuckFilePath))
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
            if (!File.Exists(fullDuckFilePath))
            {

                ducks.Add(new Duck()
                {
                    Name = "Yellow Duck",
                    Colour = "Yellow",
                    Size = DuckSizes.Egg
                });
                ducks.Add(new Duck() { Name = "Green Duck", Colour = "Green", Size = DuckSizes.Duckling });
                ducks.Add(new Duck() { Name = "Cyan Duck", Colour = "Cyan", Size = DuckSizes.Small });
                ducks.Add(new Duck() { Name = "Magenta Duck", Colour = "Magenta", Size = DuckSizes.Medium });
                ducks.Add(new Duck() { Name = "Blue Duck", Colour = "Blue", Size = DuckSizes.Large });
                ducks.Add(new Duck() { Name = "Red Duck", Colour = "Red", Size = DuckSizes.Supersized });
                ducks.Add(new Duck() { Name = "Purple Duck", Colour = "Purple", Size = DuckSizes.Ermagerd });
                ducks.Add(new Duck() { Name = "Black Duck", Colour = "Black", Size = DuckSizes.Error });

                duckListChanged = true;
            }
            else
            {
                LoadDucks();
            }
            DuckListView.ItemsSource = ducks;
            RecordNumberLabel.Text = DuckListView.Items.Count + " Records";
        }

        private void ClearDucksButton_Click(object sender, RoutedEventArgs e)
        {
            ducks.Clear();
            DuckListView.Items.Refresh();
            RecordNumberLabel.Text = DuckListView.Items.Count + " Records";
            duckListChanged = true;
        }



        private void LoadDucksButton_Click(object sender, RoutedEventArgs e)
        {
            ducks.Clear();
            LoadDucks();
            DuckListView.Items.Refresh();
            RecordNumberLabel.Text = DuckListView.Items.Count + " Records";
        }

        private void SaveDucksButton_Click(object sender, RoutedEventArgs e)
        {
            SaveDucks();
            duckListChanged = false;

        }


        private void AddDuckButton_Click(object sender, RoutedEventArgs e)
        {
            string duckName = NameTextbox.Text;
            string duckColour = ((ColourInfo)ColourComboBox.SelectedValue).ColourName;
            DuckSizes duckSize = (DuckSizes)SizeComboBox.SelectedIndex;

            ducks.Add(new Duck(duckName, duckColour, duckSize, 0));

            DuckListView.Items.Refresh();
            duckListChanged = true;

        }

        private void DeleteDuckButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Duck allForADuck = button.DataContext as Duck;
            ducks.Remove(allForADuck);
            DuckListView.Items.Refresh();
            duckListChanged = true;
        }


        private void ViewDuckButton_Click(object sender, RoutedEventArgs e)
        {
            // code to show a duck
        }

        private void EditDuckButton_Click(object sender, RoutedEventArgs e)
        {
            // Code to edit the duck
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

        void ConfigureFoldersAndFiles()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            duckDirectoryPath = documents + @"\"+ @clusterFolder + @"\" + @duckFolder;

            try
            {
                // If the directory doesn't exist, create it.
                if (!Directory.Exists(duckDirectoryPath))
                {
                    Directory.CreateDirectory(duckDirectoryPath);
                }

                fullDuckFilePath = duckDirectoryPath + "\\" + duckFileName;
                FileNameLabel.Text = fullDuckFilePath;

            }
            catch (IOException e)
            {
                if (e.Source != null) Console.WriteLine("OOPS, We have a problem {0}", e.Source);
            }
        }


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Determine if the list of ducks has been altered.
            if (duckListChanged)
            {
                if (MessageBox.Show("Do you wish to save the changes to your ducks?", 
                    "JaSON Ducks", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    SaveDucks();
                    duckListChanged = false;
                }
            }
        }

        private void DuckListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DuckListView.SelectedIndex != -1)
            {
                RecordNumberLabel.Text = "Record " + (DuckListView.SelectedIndex + 1).ToString() 
                    + " of " + DuckListView.Items.Count;
            } else
            {
                RecordNumberLabel.Text =  DuckListView.Items.Count + " Records";
            }
        }
    }

}