using System.Windows.Input;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace SteveHemond.MusicSheetViewer.Views.Shell
{
    public partial class ShellView : MetroWindow
    {
        public ShellView()
        {
            InitializeComponent();
            MouseDown += Window_MouseDown;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MenuButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Storyboard storyboard = FindResource("ExpandNavBar") as Storyboard;
            storyboard.Begin();
        }

        private void MenuButton_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Storyboard storyboard = FindResource("CollapseNavBar") as Storyboard;
            storyboard.Begin();
        }

        private void MinimizeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeRestoreButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                MaximizeRestoreButtonImage.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.WindowRestore;
            }
            else
            {
                WindowState = WindowState.Normal;
                MaximizeRestoreButtonImage.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.WindowMaximize;
            }
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
    }
}