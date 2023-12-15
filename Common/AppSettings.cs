namespace GeminiAI.Common
{
    public class AppSettings
    {
        public AppSettings(bool isProd)
        {
            Url = Environment.GetEnvironmentVariable("Url");
            API_KEY = Environment.GetEnvironmentVariable("API_KEY");
        }

        public AppSettings()
        {
            
        }
        public string Url { get; set; }
        public string API_KEY { get; set; }
    }
}
