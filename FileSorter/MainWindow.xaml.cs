using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace FileSorter
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Data> SortDatas = new ObservableCollection<Data>( );

        public MainWindow( )
        {
            InitializeComponent( );
            DataList.ItemsSource = SortDatas;
        }

        private void RefreshUI()
        {
            DataList.ItemsSource = null;
            DataList.ItemsSource = SortDatas;
        }

        private void SelectFolder(object o, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                Description = "选择要整理的文件夹",
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog( ) == System.Windows.Forms.DialogResult.OK)
                PathBox.Text = dialog.SelectedPath;
        }

        private void SortFile(object o, RoutedEventArgs e)
        {
            if (!Directory.Exists(PathBox.Text))
                return;
            FileInfo[] files;
            DirectoryInfo folder = new DirectoryInfo(PathBox.Text);
            string destPath;
            try
            {
                foreach (Data item in SortDatas)
                {
                    files = folder.GetFiles("*" + item.Key + "*");
                    if (files.Length == 0)
                        continue;
                    destPath = Path.Combine(PathBox.Text, item.Value);
                    if (!Directory.Exists(destPath))
                        Directory.CreateDirectory(destPath);
                    foreach (FileInfo info in files)
                        info.MoveTo(Path.Combine(destPath, info.Name));
                }
                FinishPopup.IsOpen = true;
            }
            catch (ArgumentException) { }
        }

        private void AddKey(object o, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(KeyBox.Text))
                return;
            if (string.IsNullOrWhiteSpace(ValueBox.Text))
                ValueBox.Text = KeyBox.Text;
            SortDatas.Add(new Data(KeyBox.Text, ValueBox.Text));
            RefreshUI( );
        }

        private void RemoveKey(object o, RoutedEventArgs e)
        {
            ObservableCollection<Data> tmp = SortDatas;
            for (int i = 0; i < SortDatas.Count; i++)
                if (SortDatas[i].IsChecked == true)
                    tmp.Remove(SortDatas[i]);
            SortDatas = tmp;
            RefreshUI( );
        }
    }

    public class Data
    {
        public Data()
        {
            IsChecked = false;
        }
        public Data(string k, string v)
        {
            IsChecked = false;
            Key = k;
            Value = v;
        }
        public bool IsChecked { get; set;}
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
