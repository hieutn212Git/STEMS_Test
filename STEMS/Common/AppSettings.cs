namespace Common
{
    public class AppSettings
    {
        public static AppSettings Instance { get; set; }

        public string Issuer { get; set; }

        public string TokenSecretKey { get; set; }

        public string Audience { get; set; }

        public string CoreDb { get; set; }
    }
}
