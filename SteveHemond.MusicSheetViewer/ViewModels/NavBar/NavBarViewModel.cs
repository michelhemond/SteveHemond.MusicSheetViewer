using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;

namespace SteveHemond.MusicSheetViewer.ViewModels.NavBar
{
    public class NavBarViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private List<NavBarItemViewModel> menuItems;
        public List<NavBarItemViewModel> MenuItems
        {
            get => menuItems;
            set => SetProperty(ref menuItems, value);
        }

        private NavBarItemViewModel selectedNavBarMenuItem;
        public NavBarItemViewModel SelectedNavBarMenuItem
        {
            get => selectedNavBarMenuItem;
            set => SetProperty(ref selectedNavBarMenuItem, value);
        }

        public NavBarViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            PopulateMenuItems();
        }

        public DelegateCommand NavBarItemSelectedCommand { get; private set; }

        private void PopulateMenuItems()
        {
            MenuItems = new List<NavBarItemViewModel>
            {
                new NavBarItemViewModel { Icon = MahApps.Metro.IconPacks.PackIconMaterialKind.FilePdf, Text = "Partitions", ViewName = "ScrapingView" },
                new NavBarItemViewModel { Icon = MahApps.Metro.IconPacks.PackIconMaterialKind.PlaylistPlay, Text = "Listes de lecture", ViewName = "PlaylistsView", CommandBarViewName = "Playlists.CommandBarView" },
                new NavBarItemViewModel { Icon = MahApps.Metro.IconPacks.PackIconMaterialKind.Settings, Text = "Paramètres", ViewName = "SettingsView" },
            };

            NavBarItemSelectedCommand = new DelegateCommand(OnNavBarItemSelected);
        }

        private void OnNavBarItemSelected()
        {
            regionManager.RequestNavigate("ContentRegion", SelectedNavBarMenuItem.ViewName);

            var navigationParameters = new NavigationParameters
            {
                { "DisplayName", SelectedNavBarMenuItem.Text },
                { "Icon", SelectedNavBarMenuItem.Icon }
            };

            regionManager.RequestNavigate("ContentHeaderRegion", "ContentHeaderView", navigationParameters);

            if (!string.IsNullOrEmpty(SelectedNavBarMenuItem.CommandBarViewName))
            {
                regionManager.RequestNavigate("CommandBarRegion", SelectedNavBarMenuItem.CommandBarViewName);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

        public void OnNavigatedTo(NavigationContext navigationContext) { }
    }
}