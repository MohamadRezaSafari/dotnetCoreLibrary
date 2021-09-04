using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Providers
{
    public class FileSizeCore
    {
        public static long Mb(long bytes)
        {
            return bytes / 1024 / 1024;
        }
        
        public static long KB(long bytes)
        {
            return bytes / 1024;
        }
    }
}