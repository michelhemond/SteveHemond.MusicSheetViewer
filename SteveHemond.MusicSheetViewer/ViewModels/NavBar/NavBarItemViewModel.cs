using MahApps.Metro.IconPacks;

namespace SteveHemond.MusicSheetViewer.ViewModels.NavBar
{
    public class NavBarItemViewModel
    {
        public PackIconMaterialKind Icon { get; set; }

        public string Text { get; set; }

        public string ViewName { get; set; }

        public string CommandBarViewName { get; set; }
    }
}