using System;

namespace Edent.Api.Helpers
{
    public class RandomCodeGenerator
    {
        /// <summary>
        /// Generates random code from 1000 to 9999 
        /// </summary>
        public static int GetRandomCode()
        {
            return new Random().Next(1000, 9999);
        }

        public static int GetRandomCode(int from, int to)
        {
            return new Random().Next(from, to);
        }
    }
}
