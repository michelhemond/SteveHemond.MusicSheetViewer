using System.Windows.Interactivity;
using Button = System.Windows.Controls.Button;
using System.Windows;
using System.Windows.Forms;

namespace SteveHemond.MusicSheetViewer.Behaviors
{
    public class FolderDialogBehavior : Behavior<Button>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Click += OnClick;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Click -= OnClick;
            base.OnDetaching();
        }
        
        public static readonly DependencyProperty FolderName = 
            DependencyProperty.RegisterAttached("FolderName", typeof(string), typeof(FolderDialogBehavior));

        public static string GetFolderName(DependencyObject obj)
        {
            return (string)obj.GetValue(FolderName);
        }

        public static void SetFolderName(DependencyObject obj, string value)
        {
            obj.SetValue(FolderName, value);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var currentPath = GetValue(FolderName) as string;
            dialog.SelectedPath = currentPath;
            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                SetValue(FolderName, dialog.SelectedPath);
            }
        }
    }
}