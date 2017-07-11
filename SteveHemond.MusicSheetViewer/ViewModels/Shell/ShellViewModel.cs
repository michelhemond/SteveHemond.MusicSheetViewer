using System;
using Prism.Mvvm;
using Prism.Regions;
using SteveHemond.MusicSheetViewer.Views.NavBar;

namespace SteveHemond.MusicSheetViewer.ViewModels.Shell
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        public ShellViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            regionManager.RegisterViewWithRegion("NavBarRegion", typeof(NavBarView));
        }
    }
}