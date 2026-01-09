namespace TradeSphere.Application.Settings
{
    public class FileLoggingSettings
    {
        public const string SectionName = "CustomSettings";
        public string LogFilePath { get; set; } = "Logs";
    }
}
