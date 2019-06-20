using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace WPF_JSON_Ducks
{
    /// <summary>
    /// Interaction logic for DucksAdd.xaml
    /// </summary>
    public partial class DucksAdd : MetroWindow
    {

        internal Duck aDuck
        {
            get
            {
                return theDuck;
            }
        }

        Duck theDuck;

        public DucksAdd()
        {
            InitializeComponent();
            AddColours();
            AddSizes();
        }

        private void NameTextbox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int row = NameTextbox.GetLineIndexFromCharacterIndex(NameTextbox.CaretIndex);
            int col = NameTextbox.CaretIndex - NameTextbox.GetCharacterIndexFromLineIndex(row);
            CursorPositionLabel.Text = "Line " + (row + 1) + ", Char " + (col + 1);
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

        private void AddDuckButton_Click(object sender, RoutedEventArgs e)
        {
            string duckName = NameTextbox.Text;
            string duckColour = ((ColourInfo)ColourComboBox.SelectedValue).ColourName;
            DuckSizes duckSize = (DuckSizes)SizeComboBox.SelectedIndex;

            this.theDuck = new Duck(duckName, duckColour, duckSize, 0);

            this.DialogResult = true;
            this.Close();

        }
    }
}
