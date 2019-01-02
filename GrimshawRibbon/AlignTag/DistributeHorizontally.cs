// Decompiled with JetBrains decompiler
// Type: AlignTag.DistributeHorizontally
// Assembly: SR_RevitTools, Version=2018.1.10.0, Culture=neutral, PublicKeyToken=null
// MVID: 2BFDAADD-616B-45CD-981C-80A6E0D00445
// Assembly location: K:\Sheppard\addins\later\2019\SR_RevitTools.dll

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AlignTag
{
  [Transaction(TransactionMode.Manual)]
  internal class DistributeHorizontally : IExternalCommand
  {
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      return new Align().AlignElements(commandData, ref message, AlignType.Horizontally);
    }
  }
}
