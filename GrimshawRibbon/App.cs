using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AlignTag;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;

namespace AEW_Ribbon
{
    class App : IExternalApplication
    {
        // define a method that will create our tab and button
        static void AddRibbonPanel(UIControlledApplication application)
        {
            // Create a custom ribbon tab
            string tabName = "AEW RTC";
            application.CreateRibbonTab(tabName);

            // Add a new ribbon panel
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "Initial Tools");

            // Get dll assembly path
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

            // create push button for CurveTotalLength
            PushButtonData b1Data = new PushButtonData(
                "cmdCurveTotalLength", 
                "Total" + System.Environment.NewLine + "  Length  ", 
                thisAssemblyPath,
                "TotalLength.CurveTotalLength");

            PushButton pb1 = ribbonPanel.AddItem(b1Data) as PushButton;
            pb1.ToolTip = "Select Multiple Lines to Obtain Total Length";
            BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/totalLength.png"));
            pb1.LargeImage = pb1Image;

            // Creating a second ribbon panel
            RibbonPanel ribbonPanel2 = application.CreateRibbonPanel(tabName, "Support");

            // Get dll assembly path
            string thisAssemblyPath2 = Assembly.GetExecutingAssembly().Location;

            // Create pushbutton data for Email BIM Team
            PushButtonData B2Data = new PushButtonData(
                "cmdEmailTeam",
                "Email" + System.Environment.NewLine + " BIM Team",
                thisAssemblyPath,
                "AEW_Ribbon.EmailTeam");

            PushButton pb2 = ribbonPanel2.AddItem(B2Data) as PushButton;
            pb2.ToolTip = "Email the BIM Team with your query or Question";
            BitmapImage pb2Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/send_help_L.png"));
            pb2.LargeImage = pb2Image;

            // Adding Align 

        }

        public Result OnShutdown(UIControlledApplication application)
        {
            // do nothing
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            // call our method that will load up our toolbar
            AddRibbonPanel(application);
            return Result.Succeeded;
        }
    }
}
