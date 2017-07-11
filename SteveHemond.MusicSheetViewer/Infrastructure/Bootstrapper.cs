using Prism.Unity;
using Microsoft.Practices.Unity;
using System.Windows;
using SteveHemond.MusicSheetViewer.Views.Shell;
using SteveHemond.MusicSheetViewer.Views.Viewer;
using SteveHemond.MusicSheetViewer.Views.NavBar;
using SteveHemond.MusicSheetViewer.ViewModels.NavBar;
using SteveHemond.MusicSheetViewer.Views.Partitions;
using SteveHemond.MusicSheetViewer.ViewModels.Partitions;
using SteveHemond.MusicSheetViewer.ViewModels.Playlists;
using SteveHemond.MusicSheetViewer.ViewModels.Settings;
using SteveHemond.MusicSheetViewer.Views.Settings;
using SteveHemond.MusicSheetViewer.Views.Playlists;
using SteveHemond.MusicSheetViewer.ViewModels.Shell;

namespace SteveHemond.MusicSheetViewer.Infrastructure
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell() => Container.Resolve<ShellView>();

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            Container.RegisterType<ContentHeaderViewModel>();
            Container.RegisterType<object, ContentHeaderView>("ContentHeaderView");

            // NavBar
            Container.RegisterType<NavBarViewModel>();
            Container.RegisterType<object, NavBarView>("NavBarView");

            // Partitions
            Container.RegisterType<ScrapingViewModel>();
            Container.RegisterType<object, ScrapingView>("ScrapingView");
            var partitionsCommandBarViewModel = new ViewModels.Partitions.CommandBarViewModel();
            Container.RegisterInstance(partitionsCommandBarViewModel);
            Container.RegisterType<PartitionsViewModel>();
            Container.RegisterType<object, Views.Partitions.CommandBarView>("Partitions.CommandBarView");
            Container.RegisterType<object, PartitionsView>("PartitionsView");
            Container.RegisterType<AddPartitionsToPlaylistViewModel>();
            Container.RegisterType<object, AddPartitionsToPlaylistView>("AddPartitionsToPlaylistView");

            // Playlists
            var playlistsCommandBarViewModel = new ViewModels.Playlists.CommandBarViewModel();
            Container.RegisterInstance(playlistsCommandBarViewModel);
            Container.RegisterType<PlaylistsViewModel>();
            Container.RegisterType<object, Views.Playlists.CommandBarView>("Playlists.CommandBarView");
            Container.RegisterType<object, PlaylistsView>("PlaylistsView");
            Container.RegisterType<AddPartitionsIntoPlaylistViewModel>();
            Container.RegisterType<object, AddPartitionsIntoPlaylistView>("AddPartitionsIntoPlaylistView");

            // Settings
            Container.RegisterType<SettingsViewModel>();
            Container.RegisterType<object, SettingsView>("SettingsView");

            Container.RegisterType<object, ViewerView>("ViewerView");
            
            base.ConfigureContainer();
        }
    }
}