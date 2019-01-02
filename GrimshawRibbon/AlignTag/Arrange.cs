using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlignTag
{
  [Transaction(TransactionMode.Manual)]
  internal class Arrange : IExternalCommand
  
  {
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      UIDocument activeUiDocument = commandData.get_Application().get_ActiveUIDocument();
      Document document = activeUiDocument.get_Document();
      using (TransactionGroup transactionGroup = new TransactionGroup(document))
      {
        using (Transaction tx = new Transaction(document))
        {
          try
          {
            transactionGroup.Start("Arrange Tags");
            this.ArrangeTag(activeUiDocument, tx);
            transactionGroup.Assimilate();
            return (Result) 0;
          }
          catch (OperationCanceledException ex)
          {
            message = ((Exception) ex).Message;
            if (tx.HasStarted())
              tx.RollBack();
            return (Result) 1;
          }
          catch (ErrorMessageException ex)
          {
            message = ex.Message;
            if (tx.HasStarted())
              tx.RollBack();
            return (Result) -1;
          }
          catch (Exception ex)
          {
            message = ex.Message;
            if (tx.HasStarted())
              tx.RollBack();
            return (Result) -1;
          }
        }
      }
    }

    private void ArrangeTag(UIDocument uidoc, Transaction tx)
    {
      Document document = uidoc.get_Document();
      View activeView = document.get_ActiveView();
      if (!activeView.get_CropBoxActive())
        throw new ErrorMessageException("Please set a crop box to the view");
      IEnumerable<IndependentTag> independentTags = ((IEnumerable<Element>) new FilteredElementCollector(document, ((Element) activeView).get_Id()).OfClass(typeof (IndependentTag)).WhereElementIsNotElementType()).Select(elem => new
      {
        elem = elem,
        type = elem as IndependentTag
      }).Where(_param1 => _param1.type.get_HasLeader()).Select(_param1 => _param1.type);
      tx.Start("Prepare Tags");
      using (IEnumerator<IndependentTag> enumerator = independentTags.GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          IndependentTag current = enumerator.Current;
          current.set_LeaderEndCondition((LeaderEndCondition) 1);
          current.set_LeaderEnd(current.get_TagHeadPosition());
        }
      }
      tx.Commit();
      tx.Start("Arrange Tags");
      List<TagLeader> tagLeaderList1 = new List<TagLeader>();
      List<TagLeader> tagLeaderList2 = new List<TagLeader>();
      using (IEnumerator<IndependentTag> enumerator = independentTags.GetEnumerator())
      {
        while (((IEnumerator) enumerator).MoveNext())
        {
          TagLeader tagLeader = new TagLeader(enumerator.Current, document);
          if (tagLeader.Side == ViewSides.Left)
            tagLeaderList1.Add(tagLeader);
          else
            tagLeaderList2.Add(tagLeader);
        }
      }
      List<XYZ> tagPositionPoints1 = this.CreateTagPositionPoints(activeView, tagLeaderList1, ViewSides.Left);
      List<XYZ> tagPositionPoints2 = this.CreateTagPositionPoints(activeView, tagLeaderList2, ViewSides.Right);
      List<TagLeader> list1 = tagLeaderList1.OrderBy<TagLeader, double>((Func<TagLeader, double>) (x => x.LeaderEnd.get_X())).ToList<TagLeader>().OrderBy<TagLeader, double>((Func<TagLeader, double>) (x => x.LeaderEnd.get_Y())).ToList<TagLeader>();
      this.PlaceAndSort(tagPositionPoints1, list1);
      List<TagLeader> list2 = tagLeaderList2.OrderByDescending<TagLeader, double>((Func<TagLeader, double>) (x => x.LeaderEnd.get_X())).ToList<TagLeader>().OrderBy<TagLeader, double>((Func<TagLeader, double>) (x => x.LeaderEnd.get_Y())).ToList<TagLeader>();
      this.PlaceAndSort(tagPositionPoints2, list2);
      tx.Commit();
    }

    private void PlaceAndSort(List<XYZ> positionPoints, List<TagLeader> tags)
    {
      foreach (TagLeader tag in tags)
      {
        XYZ nearestPoint = this.FindNearestPoint(positionPoints, tag.TagCenter);
        tag.TagCenter = nearestPoint;
        positionPoints.Remove(nearestPoint);
      }
      this.UnCross(tags);
      this.UnCross(tags);
      foreach (TagLeader tag in tags)
        tag.UpdateTagPosition();
    }

    private void UnCross(List<TagLeader> tags)
    {
      foreach (TagLeader tag1 in tags)
      {
        foreach (TagLeader tag2 in tags)
        {
          if (tag1 != tag2 && (((Curve) tag1.BaseLine).Intersect((Curve) tag2.BaseLine) == 8 || ((Curve) tag1.BaseLine).Intersect((Curve) tag2.EndLine) == 8 || ((Curve) tag1.EndLine).Intersect((Curve) tag2.BaseLine) == 8 || ((Curve) tag1.EndLine).Intersect((Curve) tag2.EndLine) == 8))
          {
            XYZ tagCenter = tag1.TagCenter;
            tag1.TagCenter = tag2.TagCenter;
            tag2.TagCenter = tagCenter;
          }
        }
      }
    }

    private XYZ FindNearestPoint(List<XYZ> points, XYZ basePoint)
    {
      XYZ xyz = ((IEnumerable<XYZ>) points).FirstOrDefault<XYZ>();
      double num = basePoint.DistanceTo(xyz);
      basePoint.DistanceTo(xyz);
      using (List<XYZ>.Enumerator enumerator = points.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          XYZ current = enumerator.Current;
          if (basePoint.DistanceTo(current) < num)
          {
            xyz = current;
            num = basePoint.DistanceTo(current);
          }
        }
      }
      return xyz;
    }

    private List<XYZ> CreateTagPositionPoints(View activeView, List<TagLeader> tagLeaders, ViewSides side)
    {
      List<XYZ> xyzList = new List<XYZ>();
      BoundingBoxXYZ cropBox = activeView.get_CropBox();
      if ((uint) tagLeaders.Count > 0U)
      {
        double num1 = tagLeaders.Max<TagLeader>((Func<TagLeader, double>) (x => x.TagHeight));
        tagLeaders.Max<TagLeader>((Func<TagLeader, double>) (x => x.TagWidth));
        double num2 = num1 * 1.2;
        int num3 = (int) Math.Round((cropBox.get_Max().get_Y() - cropBox.get_Min().get_Y()) / num2);
        XYZ xyz1 = new XYZ(cropBox.get_Max().get_X(), cropBox.get_Min().get_Y(), 0.0);
        XYZ xyz2 = new XYZ(cropBox.get_Min().get_X(), cropBox.get_Min().get_Y(), 0.0);
        for (int index = num3 * 2; index > 0; --index)
        {
          if (side == ViewSides.Left)
            xyzList.Add(XYZ.op_Addition(xyz2, new XYZ(0.0, num2 * (double) index, 0.0)));
          else
            xyzList.Add(XYZ.op_Addition(xyz1, new XYZ(0.0, num2 * (double) index, 0.0)));
        }
      }
      return xyzList;
    }
  }
}
