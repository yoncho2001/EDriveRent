using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDriveRent
{
    public class Validate
    {
        public static void isString(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("Null input is not allowed ");
            }
        }
    }
}
