using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffManager.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            TiffManager.Split(@"D:\Play Ground\tiff\MD-Rate-01212022855 - Copy (2).tif", @"D:\Play Ground\tiff\split");

            TiffManager.Merge(@"D:\Play Ground\tiff\merge.tif", @"D:\Play Ground\tiff\split\MD-Rate-01212022855 - Copy (2)_00012.tif,D:\Play Ground\tiff\split\MD-Rate-01212022855 - Copy (2)_00013.tif
,D:\Play Ground\tiff\split\MD-Rate-01212022855 - Copy (2)_00016.tif");
        }
    }
}
