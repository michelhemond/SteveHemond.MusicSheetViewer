namespace SteveHemond.MusicSheetViewer.Services
{
    public class SettingService
    {
        public string GetPdfFilePath()
        {
            return Properties.Settings.Default.PdfFilePath;
        }

        public void SetPdfFilePath(string path)
        {
            Properties.Settings.Default.PdfFilePath = path;
            Properties.Settings.Default.Save();
        }
    }
}