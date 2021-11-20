using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Utils
{
    public static class GuidHelper
    {
        public static Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }

        public static string ConvertToString(Guid guid)
        {
            return string.Concat(guid.ToString("N").Select(c => (char)(c + 17)));
        }

        public static Guid ConvertFromString(string stringGuid)
        {
            return Guid.ParseExact(string.Concat(stringGuid.Select(c => (char)(c - 17))), "N");
        }
    }
}