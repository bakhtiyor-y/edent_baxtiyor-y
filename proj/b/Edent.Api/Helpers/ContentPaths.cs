using Edent.Api.Infrastructure;
using System.IO;

namespace Edent.Api.Helpers
{
    public class ContentPaths
    {
        public static string GetSeedDataPath(string fileName)
        {
            return Path.Combine(GlobalConfiguration.ContentRoot, GlobalConfiguration.SeedDataContent, fileName);
        }
    }
}
