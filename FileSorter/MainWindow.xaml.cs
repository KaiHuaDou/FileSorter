using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace FileSorter
{
    public partial class MainWindow : Window
    {
        public MainWindow( )
        {
            InitializeComponent( );
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
                foreach (string item in KeyList.Items)
                {
                    files = folder.GetFiles("*" + item + "*");
                    if (files.Length == 0)
                        continue;
                    destPath = Path.Combine(PathBox.Text, item);
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
            KeyList.Items.Add(KeyBox.Text);
        }

        private void RemoveKey(object o, RoutedEventArgs e)
        {
            int cnt = KeyList.SelectedItems.Count;
            for (int i = 0; i < cnt; i++)
                KeyList.Items.Remove(KeyList.SelectedItems[0]);
        }
    }
}
