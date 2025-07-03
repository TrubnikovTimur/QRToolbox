using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRToolbox
{
    public static class DataBank
    {
        public static string ImagePath { get; set; }
        public static BoxPlacement Placement { get; set; }
        public static ExternalEvent ImportImageEvent { get; set; }
    }
}
