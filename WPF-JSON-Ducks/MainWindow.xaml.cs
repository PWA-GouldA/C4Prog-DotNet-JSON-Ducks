using MahApps.Metro.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
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
///     https://www.wpf-tutorial.com/listview-control/listview-sorting/
///     https://www.wpf-tutorial.com/listview-control/listview-filtering/
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

        public bool duckListChanged = false; // this is updated if you add/edit/remove a duck

        List<Duck> ducks = new List<Duck>();

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;



        public MainWindow()
        {
            InitializeComponent();
            ConfigureFoldersAndFiles();
            AddSampleDucks();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DuckListView.ItemsSource);
            view.Filter = DuckFilter;
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
            DuckListView.Items.Refresh();
            RecordNumberLabel.Text = DuckListView.Items.Count + " Records";
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
            duckListChanged = false;
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

        private void ClearDucks()
        {
            ducks.Clear();
            DuckListView.Items.Refresh();
            RecordNumberLabel.Text = DuckListView.Items.Count + " Records";
            duckListChanged = true;
        }

        private void ClearDucksButton_Click(object sender, RoutedEventArgs e)
        {
            ClearDucks();
        }

        private void LoadDucksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDucks();
        }

        private void SaveDucksButton_Click(object sender, RoutedEventArgs e)
        {
            SaveDucks();
        }

        private void DeleteDuckButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Duck allForADuck = button.DataContext as Duck;
            ducks.Remove(allForADuck);
            CollectionViewSource.GetDefaultView(DuckListView.ItemsSource).Refresh();
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


        void ConfigureFoldersAndFiles()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            duckDirectoryPath = documents + @"\\" + @clusterFolder + @"\\" + @duckFolder;

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
            VerifyExitAndSave();
        }

        private void DuckListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DuckListView.SelectedIndex != -1)
            {
                RecordNumberLabel.Text = "Record " + (DuckListView.SelectedIndex + 1).ToString()
                    + " of " + DuckListView.Items.Count;
            }
            else
            {
                RecordNumberLabel.Text = DuckListView.Items.Count + " Records";
            }
        }

        private void FileOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            LoadDucks();
            DuckListView.ItemsSource = ducks;
            DuckListView.Items.Refresh();
        }

        private void FileSaveMenu_Click(object sender, RoutedEventArgs e)
        {
            SaveDucks();
            DuckListView.ItemsSource = ducks;
            DuckListView.Items.Refresh();
        }

        private void FileNewMenu_Click(object sender, RoutedEventArgs e)
        {
            ClearDucks();
            DuckListView.ItemsSource = ducks;
            DuckListView.Items.Refresh();
        }

        private void FileExitMenu_Click(object sender, RoutedEventArgs e)
        {
            VerifyExitAndSave();
        }

        private void VerifyExitAndSave()
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


        private bool DuckFilter(object item)
        {
            if (String.IsNullOrEmpty(FilterText.Text))
                return true;
            else
                return (item as Duck).Name.IndexOf(FilterText.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void FilterText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(DuckListView.ItemsSource).Refresh();
        }

        private void DuckListViewHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                DuckListView.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            DuckListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterText.Text = "";
        }

        private void DuckNewMenu_Click(object sender, RoutedEventArgs e)
        {
            DucksAdd addWindow = new DucksAdd()
            {
                Title = "Create New Duck",
                ShowTitleBar = true,
                GlowBrush = new SolidColorBrush(Colors.DodgerBlue),
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            addWindow.Owner = this;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var res = addWindow.ShowDialog();

            if (res == true)
            {
                ducks.Add(addWindow.aDuck);
                duckListChanged = true;
                CollectionViewSource.GetDefaultView(DuckListView.ItemsSource).Refresh();
            }

        }
    }



    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element) => this.Direction = dir;

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }


}