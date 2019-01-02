using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using AlignTag;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;

namespace AlignTag
{
  internal class AlignToolIcons
  {
      public static void CreateAlignIcons(RibbonPanel rPanel)
    {
      string location = Assembly.GetExecutingAssembly().Location;

      //adding buttons

      //Button 1

      PushButtonData pushButtonData1 = new PushButtonData("alignLeftButton","Align Left",location,"AlignTag.AlignLeft");
      PushButton pB1 = rPanel.AddItem(pushButtonData1) as PushButton;
      pB1.ToolTip = "Align Tags or Elements Left";
      BitmapImage pb1LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignLeftLarge.png"));
      BitmapImage pb1Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignLeftSmall.png"));

      // Button 2
      PushButtonData pushButtonData2 = new PushButtonData("alignRightButton", "Align Right", location, "AlignTag.AlignRight");
      PushButton pB2 = rPanel.AddItem(pushButtonData2) as PushButton;
      pB2.ToolTip = "Align Tags or Elements Right";
      BitmapImage pb2LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignRightLarge.png"));
      BitmapImage pb2Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignRightSmall.png"));

      // Button 3

      PushButtonData pushButtonData3 = new PushButtonData("alignTopButton", "Align Top", location, "AlignTag.AlignTop");
      PushButton pB3 = rPanel.AddItem(pushButtonData3) as PushButton;
      pB3.ToolTip = "Align Tags or Elements Top";
      BitmapImage pb3LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignTopLarge.png"));
      BitmapImage pb3Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignTopSmall.png"));
      
      //Button 4
      PushButtonData pushButtonData4 = new PushButtonData("alignBottomButton", "Align Bottom", location, "AlignTag.AlignBottom");
      PushButton pB4 = rPanel.AddItem(pushButtonData4) as PushButton;
      pB4.ToolTip = "Align Tags or Elements Bottom";
      BitmapImage pb4LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignBottomLarge.png"));
      BitmapImage pb4Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignBottomSmall.png"));

      // Button 5
      PushButtonData pushButtonData5 = new PushButtonData("alignCenterButton", "Align Center", location, "AlignTag.AlignCenter");
      PushButton pB5 = rPanel.AddItem(pushButtonData5) as PushButton;
      pB5.ToolTip = "Align Tags or Elements Center";
      BitmapImage pb5LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignCenterLarge.png"));
      BitmapImage pb5Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignCenterSmall.png"));

      // Button 6
      PushButtonData pushButtonData6 = new PushButtonData("alignMiddleButton", "Align Middle", location, "AlignTag.AlignMiddle");
      PushButton pB6 = rPanel.AddItem(pushButtonData6) as PushButton;
      pB6.ToolTip = "Align Tags or Elements Middle";
      BitmapImage pb6LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignMiddleLarge.png"));
      BitmapImage pb6Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/AlignMiddleSmall.png"));

      // Button 7
      PushButtonData pushButtonData7 = new PushButtonData("distributeHorizontallyButton", "Distribute\nHorizontally", location, "AlignTag.DistributeHorizontally");
      PushButton pB7 = rPanel.AddItem(pushButtonData7) as PushButton;
      pB7.ToolTip = "Distribute Tags or Elements Horizontally";
      BitmapImage pb7LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/DistributeHorizontallyLarge.png"));
      BitmapImage pb7Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/DistributeHorizontallySmall.png"));

      // Button 8
      PushButtonData pushButtonData8 = new PushButtonData("distributeVerticallyButton", "Distribute\nVertically", location, "AlignTag.DistributeVertically");
      PushButton pB8 = rPanel.AddItem(pushButtonData8) as PushButton;
      pB8.ToolTip = "Distribute Tags or Elements Vertically";
      BitmapImage pb8LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/DistributeVerticallyLarge.png"));
      BitmapImage pb8Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/DistributeVerticallySmall.png"));

      // Button 9
      PushButtonData pushButtonData9 = new PushButtonData("ArrangeButton", "Arrange\nTags", location, "AlignTag.Arrange");
      PushButton pB9 = rPanel.AddItem(pushButtonData9) as PushButton;
      pB9.ToolTip = "Arrange Tags around the view";
      BitmapImage pb9LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/ArrangeLarge.png"));
      BitmapImage pb9Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/ArrangeSmall.png"));

      // Button 10
      PushButtonData pushButtonData10 = new PushButtonData("UntangleVerticallyButton", "Untangle\nVertically", location, "AlignTag.UntangleVertically");
      PushButton pB10 = rPanel.AddItem(pushButtonData10) as PushButton;
      pB10.ToolTip = "Untangle Vertically Tags or Elements ";
      BitmapImage pb10LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/UntangleVerticallyLarge.png"));
      BitmapImage pb10Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/UntangleVerticallySmall.png"));

      // Button 11
      PushButtonData pushButtonData11 = new PushButtonData("UntangleHorizontallyButton", "Untangle\nHorizontally", location, "AlignTag.UntangleHorizontally");
      PushButton pB11 = rPanel.AddItem(pushButtonData11) as PushButton;
      pB11.ToolTip = "Untangle Horizontally Tags or Elements ";
      BitmapImage pb11LargeImage = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/UntangleHorizontallyLarge.png"));
      BitmapImage pb11Image = new BitmapImage(new Uri("pack://application:,,,/AEW_Ribbon;component/Resources/UntangleHorizontallySmall.png"));

      rPanel.AddStackedItems((RibbonItemData) pushButtonData1, (RibbonItemData) pushButtonData5, (RibbonItemData) pushButtonData2);
      rPanel.AddStackedItems((RibbonItemData) pushButtonData3, (RibbonItemData) pushButtonData6, (RibbonItemData) pushButtonData4);
      rPanel.AddStackedItems((RibbonItemData) pushButtonData7, (RibbonItemData) pushButtonData8, (RibbonItemData) pushButtonData9);
      rPanel.AddStackedItems((RibbonItemData) pushButtonData10, (RibbonItemData) pushButtonData11);
    }

    private static ContextualHelp CreateContextualHelp(string helpFile)
    {
      FileInfo fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
      string fileName = Path.Combine(fileInfo.Directory.Parent.FullName, "help.htm");
      if (new FileInfo(fileName).Exists)
        return new ContextualHelp((ContextualHelpType) 2, fileName);
      string path = Path.Combine(fileInfo.Directory.FullName, helpFile);
      Tools.ExtractResource("AlignHelp.chm", path);
      return new ContextualHelp((ContextualHelpType) 3, path);
    }
  }
}
