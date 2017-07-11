using SteveHemond.MusicSheetViewer.Infrastructure;
using System.Windows;

namespace SteveHemond.MusicSheetViewer
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}