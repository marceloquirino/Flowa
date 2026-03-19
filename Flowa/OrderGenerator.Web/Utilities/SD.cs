namespace OrderGenerator.Web.Utilities
{
    // Static Details class to hold application-wide constants and settings
    public static class SD
    {
        public static string? OrderGeneratorAPIBase { get; set; }

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
