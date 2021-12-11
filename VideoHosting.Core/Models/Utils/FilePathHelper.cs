using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Utils
{
    public static class FilePathHelper
    {
        public static string GetExtension(string path)
        {
            return path.Substring(path.LastIndexOf("."), path.Length - path.LastIndexOf("."));
        }
    }
}