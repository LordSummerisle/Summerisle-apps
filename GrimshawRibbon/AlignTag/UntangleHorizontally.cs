using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace AlignTag
{
  [Transaction(TransactionMode.Manual)]
  internal class UntangleHorizontally : IExternalCommand
  {
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      return new Align().AlignElements(commandData, ref message, AlignType.UntangleHorizontally);
    }
  }
}
