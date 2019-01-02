using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Diagnostics;

namespace AEW_Ribbon
{
    [Transaction(TransactionMode.Manual)]
    public class EmailTeam : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Process.Start("mailto:MHaworth@AEWArchitects.com?Subject=I%20need%20help!&Body=Can%20you%20please%20assist%20me%20with%20the%20following...");
            return (Result)0;
        }
   }
}
