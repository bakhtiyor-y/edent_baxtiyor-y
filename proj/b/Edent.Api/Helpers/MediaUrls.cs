using Edent.Api.Infrastructure;
using System.IO;

namespace Edent.Api.Helpers
{
    public class MediaUrls
    {
        public static string GetUserMediaUrl(string fileName, int userId)
        {
            return Path.Combine(GlobalConfiguration.WebRoot, GlobalConfiguration.UserContent, userId.ToString(), fileName);
        }

        public static string GetSharedImageDirectory()
        {
            return Path.Combine(GlobalConfiguration.WebRoot, GlobalConfiguration.SharedContent, "Images");
        }

        public static string GetSharedImageUrl(string fileName)
        {
            return Path.Combine(GlobalConfiguration.WebRoot, GlobalConfiguration.SharedContent, "Images", fileName);
        }

        public static string GetUserMediaDirectory(int userId)
        {
            return Path.Combine(GlobalConfiguration.WebRoot, GlobalConfiguration.UserContent, userId.ToString());
        }

        public static string GetUserMedia(string fileName, int userId)
        {
            return $"//{GlobalConfiguration.UserContent}//{userId}//{fileName}";
        }

        public static string GetSharedImage(string fileName)
        {
            return $"//{GlobalConfiguration.SharedContent}//Images//{fileName}";
        }
    }
}
