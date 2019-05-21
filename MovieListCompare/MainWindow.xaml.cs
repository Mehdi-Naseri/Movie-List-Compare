using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.VisualBasic.FileIO;
using MovieListCompare.Models;
using MovieListCompare.business;
using System.Collections;

namespace MovieListCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MovieCompareClass MovieCompareClass1 = new MovieCompareClass();
        List<MovieCompare> ListMovieCompare;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region UI
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            buttonCompare.IsEnabled = false;
            buttonCopy.IsEnabled = false;
            checkBoxCopyUI.IsEnabled = false;
            ButtonExportCSV.IsEnabled = false;
            textBox1.TextWrapping = TextWrapping.NoWrap;
            textBox2.TextWrapping = TextWrapping.NoWrap;
            textBox1.Text = System.IO.Directory.GetCurrentDirectory() + @"\MovieList.txt";
            checkBoxSize.IsChecked = true;
        }
        private void textBoxes_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check TextBox 1 & 2 . If they are not empty then enable compare button.
            if (textBox1.Text.Length > 0 & textBox2.Text.Length > 0)
            {
                buttonCompare.IsEnabled = true;

            }
            else
            {
                buttonCompare.IsEnabled = false;

            }
        }
        private void ButtonBrowse1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                if (FolderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text = FolderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonBrowse2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                if (FolderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = FolderBrowserDialog1.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void buttonCompare_Click(object sender, RoutedEventArgs e)
        {
            ListMovieCompare = MovieCompareClass1.Compare(textBox1.Text, textBox2.Text, checkBoxSize.IsChecked);
            dataGridResult.ItemsSource = ListMovieCompare;
            dataGridResult.Items.Refresh();
            if (ListMovieCompare.Count() > 0)
            {
                buttonCopy.IsEnabled = true;
                checkBoxCopyUI.IsEnabled = true;
                ButtonExportCSV.IsEnabled = true;
            }
            else
            {
                buttonCopy.IsEnabled = false;
                checkBoxCopyUI.IsEnabled = false;
                ButtonExportCSV.IsEnabled = false;
            }
        }
        private void buttonCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string stringSaveTo;
                System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                if (FolderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    stringSaveTo = FolderBrowserDialog1.SelectedPath;
                    System.IO.DirectoryInfo DirectoryInfoTo = new System.IO.DirectoryInfo(stringSaveTo);
                    foreach (MovieCompare MovieCompare1 in ListMovieCompare)
                    {
                        if (MovieCompare1.Copy)
                        {
                            try
                            {
                                if (MovieCompare1.Folder)
                                {
                                    string stringDirectorySource = System.IO.Path.Combine(textBox2.Text, MovieCompare1.Movie2);
                                    string stringDirectoryDestination = System.IO.Path.Combine(stringSaveTo, MovieCompare1.Movie2);
                                    //System.IO.DirectoryInfo DirectoryInfoSource = new System.IO.DirectoryInfo();
                                    //CopyDirectory(DirectoryInfoSource, DirectoryInfoTo);
                                    if (checkBoxCopyUI.IsChecked == true)
                                        FileSystem.CopyDirectory(stringDirectorySource, stringDirectoryDestination, UIOption.AllDialogs);
                                    else
                                        FileSystem.CopyDirectory(stringDirectorySource, stringDirectoryDestination);
                                }
                                else
                                {
                                    System.IO.FileInfo fileInfoSource = new System.IO.FileInfo(System.IO.Path.Combine(textBox2.Text, MovieCompare1.Movie2));
                                    string stringDestination = System.IO.Path.Combine(DirectoryInfoTo.FullName, fileInfoSource.Name);
                                    System.IO.File.Copy(fileInfoSource.FullName, stringDestination, false);
                                }
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ButtonExportCSV_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable IEnumerableGridData = dataGridResult.ItemsSource;
            Microsoft.Win32.SaveFileDialog SaveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            SaveFileDialog1.Filter = "Excel Workbook (*.csv)|*.csv";
            SaveFileDialog1.DefaultExt = "csv";
            if ((bool)SaveFileDialog1.ShowDialog())
            {
                if (MovieCompareClass1.ExportGridToCSV(IEnumerableGridData, SaveFileDialog1.FileName))
                { MessageBox.Show("Saved successfully", "Save Movie Comparison", MessageBoxButton.OK, MessageBoxImage.Information); }
                else
                { MessageBox.Show("Unable to save.", "Save Movie Comparison", MessageBoxButton.OK, MessageBoxImage.Error); }
                ;
            }
        }
        #endregion
    }
}
