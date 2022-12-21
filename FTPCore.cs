using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Hosting;

namespace Providers
{
    public class FTPCore
    {
        private static string username = "dleonlin";
        private static string password = "rX0v12fr9U";


        // FTPCore.Send(_hostingEnvironment, "/img/", "chart.png");
        public static bool Send(IHostingEnvironment hostingEnvironment, string path, string name)
        {
            try
            {
                WebClient wc = new WebClient();
                Uri uriadd = new Uri(@"ftp://hn92.mylittledatacenter.com/public_html/files/" + PersianYear() + "/" + name);
                wc.Credentials = new NetworkCredential(username, password);
                var fullName = hostingEnvironment.WebRootPath + (path + name);
                wc.UploadFile(uriadd, fullName);
                
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        // برگرداندن سال شمسی
        private static string PersianYear()
        {
            PersianCalendar t = new PersianCalendar();
            DateTime Time = DateTime.Now;
            int Y = t.GetYear(Time);
            return Y.ToString();
        }
    }
}