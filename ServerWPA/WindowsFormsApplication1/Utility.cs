using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Utility
    {
          public static String FilePathWithVers(String path, Int64 Version)
        {
            if (Path.GetDirectoryName(path) == "\\")
                return '\\' + Path.GetFileNameWithoutExtension(path) + '_' + Version + Path.GetExtension(path);
            else return Path.GetDirectoryName(path) + '\\' + Path.GetFileNameWithoutExtension(path) + '_' + Version + Path.GetExtension(path);
        }
    }
}
