using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            var source=new DirectoryInfo(@"C:\constantmd\Bitbucket\erp\Site\CMDEHR\CMDEHR.Web\Shared\Uploads\ehr");
            char[] delimiterChar = { '\\' };
            string str = @"C:\constantmd\Bitbucket\erp\Site\CMDEHR\CMDEHR.Web\Shared\Uploads\ehr";
            string newPath = str.Replace(str.Split('\\').Last(),"sh");
            //Console.WriteLine(words);
            Console.ReadKey();
            //var de = source.CopyTo(@"C:\site", true);
        }
    }
}
