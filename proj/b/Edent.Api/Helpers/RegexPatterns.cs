namespace Edent.Api.Helpers
{
    public class RegexPatterns
    {
        public const string PhoneNumber = "^[+]*[0-9]{0,3}[-\\s\\./0-9]*$";
        public const string Email = "^\\w+[\\w-\\.]*\\@\\w+((-\\w+)|(\\w*))\\.[a-z]{2,3}$";
    }
}
