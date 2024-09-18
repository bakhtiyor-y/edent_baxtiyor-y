using System.IO;

namespace Edent.Api.Infrastructure
{
    public class GlobalConfiguration
    {
        public static string WebRoot { get; set; }
        public static string ContentRoot { get; set; }
        public static string UserContent => "UserContent";
        public static string SharedContent => "SharedContent";
        public static string SeedDataContent => Path.Combine("App_Data", "InitData");
        public static bool IsDevelopment { get; set; }
    }
}
