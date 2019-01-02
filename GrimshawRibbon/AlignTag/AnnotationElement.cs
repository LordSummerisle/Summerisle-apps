using Autodesk.Revit.DB;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlignTag
{
  internal class AnnotationElement
  {
    private Document _doc;
    private View _ownerView;

    public XYZ Center { get; set; }

    public XYZ UpLeft { get; set; }

    public XYZ UpRight { get; set; }

    public XYZ DownLeft { get; set; }

    public XYZ DownRight { get; set; }

    public Element Parent { get; set; }

    public AnnotationElement(Element e)
    {
      this.Parent = e;
      this._doc = e.get_Document();
      this._ownerView = this._doc.GetElement(e.get_OwnerViewId()) == null ? this._doc.get_ActiveView() : this._doc.GetElement(e.get_OwnerViewId()) as View;
      Plane byNormalAndOrigin = Plane.CreateByNormalAndOrigin(this._ownerView.get_ViewDirection(), this._ownerView.get_Origin());
      BoundingBoxXYZ boundingBox = e.get_BoundingBox(this._ownerView);
      XYZ pointA1 = boundingBox.get_Max();
      XYZ pointB1 = boundingBox.get_Min();
      double num = this.ProjectedDistance(byNormalAndOrigin, pointA1, pointB1);
      XYZ pointA2 = new XYZ(pointA1.get_X(), pointB1.get_Y(), pointA1.get_Z());
      XYZ pointB2 = new XYZ(pointB1.get_X(), pointA1.get_Y(), pointB1.get_Z());
      if (this.ProjectedDistance(byNormalAndOrigin, pointA2, pointB2) > num)
      {
        pointA1 = pointA2;
        pointB1 = pointB2;
      }
      Transform transform = this._ownerView.get_CropBox().get_Transform();
      XYZ xyz1 = transform.get_Inverse().OfPoint(pointA1);
      XYZ xyz2 = transform.get_Inverse().OfPoint(pointB1);
      this.UpLeft = new XYZ(this.GetMin(xyz2.get_X(), xyz1.get_X()), this.GetMax(xyz1.get_Y(), xyz2.get_Y()), 0.0);
      this.UpRight = new XYZ(this.GetMax(xyz2.get_X(), xyz1.get_X()), this.GetMax(xyz1.get_Y(), xyz2.get_Y()), 0.0);
      this.DownLeft = new XYZ(this.GetMin(xyz2.get_X(), xyz1.get_X()), this.GetMin(xyz1.get_Y(), xyz2.get_Y()), 0.0);
      this.DownRight = new XYZ(this.GetMax(xyz2.get_X(), xyz1.get_X()), this.GetMin(xyz1.get_Y(), xyz2.get_Y()), 0.0);
      this.Center = XYZ.op_Division(XYZ.op_Addition(this.UpRight, this.DownLeft), 2.0);
    }

    private double ProjectedDistance(Plane plane, XYZ pointA, XYZ pointB)
    {
      return this.ProjectionOnPlane(pointA, plane).DistanceTo(this.ProjectionOnPlane(pointB, plane));
    }

    private XYZ ProjectionOnPlane(XYZ q, Plane plane)
    {
      XYZ origin = plane.get_Origin();
      XYZ xyz = plane.get_Normal().Normalize();
      return XYZ.op_Subtraction(q, xyz.Multiply(xyz.DotProduct(XYZ.op_Subtraction(q, origin))));
    }

    private double GetMax(double value1, double value2)
    {
      if (value1 >= value2)
        return value1;
      return value2;
    }

    private double GetMin(double value1, double value2)
    {
      if (value1 >= value2)
        return value2;
      return value1;
    }

    public XYZ GetLeaderEnd()
    {
      XYZ xyz = this.Center;
      Element parent = this.Parent;
      if (((object) parent).GetType() == typeof (IndependentTag))
      {
        IndependentTag tag = parent as IndependentTag;
        if (tag.get_HasLeader())
          xyz = tag.get_LeaderEndCondition() != 1 ? TagLeader.GetLeaderEnd(TagLeader.GetTaggedElement(this._doc, tag), this._ownerView) : tag.get_LeaderEnd();
      }
      else if (((object) parent).GetType() == typeof (TextNote))
      {
        TextNote textNote = parent as TextNote;
        if ((uint) textNote.get_LeaderCount() > 0U)
          xyz = ((IEnumerable<Leader>) textNote.GetLeaders()).FirstOrDefault<Leader>().get_End();
      }
      else if (((object) parent).GetType().IsSubclassOf(typeof (SpatialElementTag)))
      {
        SpatialElementTag spatialElementTag = parent as SpatialElementTag;
        if (spatialElementTag.get_HasLeader())
          xyz = spatialElementTag.get_LeaderEnd();
      }
      else
        xyz = this.Center;
      return xyz;
    }

    public void MoveTo(XYZ point, AlignType alignType)
    {
      XYZ xyz = new XYZ();
      switch (alignType)
      {
        case AlignType.Left:
          xyz = XYZ.op_Subtraction(point, this.UpLeft);
          break;
        case AlignType.Right:
          xyz = XYZ.op_Subtraction(point, this.UpRight);
          break;
        case AlignType.Up:
          xyz = XYZ.op_Subtraction(point, this.UpRight);
          break;
        case AlignType.Down:
          xyz = XYZ.op_Subtraction(point, this.DownRight);
          break;
        case AlignType.Center:
          xyz = XYZ.op_Subtraction(point, this.Center);
          break;
        case AlignType.Middle:
          xyz = XYZ.op_Subtraction(point, this.Center);
          break;
        case AlignType.Vertically:
          xyz = XYZ.op_Subtraction(point, this.Center);
          break;
        case AlignType.Horizontally:
          xyz = XYZ.op_Subtraction(point, this.Center);
          break;
        case AlignType.UntangleVertically:
          xyz = XYZ.op_Subtraction(point, this.UpLeft);
          break;
        case AlignType.UntangleHorizontally:
          xyz = XYZ.op_Subtraction(point, this.UpLeft);
          break;
      }
      Transform translation = Transform.CreateTranslation(this._ownerView.get_CropBox().get_Transform().OfVector(xyz));
      if (((object) this.Parent).GetType() == typeof (IndependentTag))
      {
        IndependentTag parent = this.Parent as IndependentTag;
        CustomLeader customLeader = new CustomLeader();
        if (parent.get_HasLeader() && parent.get_LeaderEndCondition() == 1)
          customLeader = new CustomLeader(parent.get_LeaderEnd(), new XYZ(0.0, 0.0, 0.0));
        parent.set_TagHeadPosition(translation.OfPoint(parent.get_TagHeadPosition()));
        if (!parent.get_HasLeader() || parent.get_LeaderEndCondition() != 1)
          return;
        parent.set_LeaderEnd(customLeader.End);
      }
      else if (((object) this.Parent).GetType() == typeof (TextNote))
      {
        List<CustomLeader> customLeaderList = new List<CustomLeader>();
        TextNote parent = this.Parent as TextNote;
        if ((uint) parent.get_LeaderCount() > 0U)
        {
          using (IEnumerator<Leader> enumerator = ((IEnumerable<Leader>) parent.GetLeaders()).GetEnumerator())
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              Leader current = enumerator.Current;
              customLeaderList.Add(new CustomLeader(current));
            }
          }
        }
        ((TextElement) parent).set_Coord(translation.OfPoint(((TextElement) parent).get_Coord()));
        if ((uint) customLeaderList.Count <= 0U)
          return;
        int index = 0;
        using (IEnumerator<Leader> enumerator = ((IEnumerable<Leader>) parent.GetLeaders()).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Leader current = enumerator.Current;
            current.set_End(customLeaderList[index].End);
            current.set_Elbow(customLeaderList[index].Elbow);
            ++index;
          }
        }
      }
      else if (((object) this.Parent).GetType().IsSubclassOf(typeof (SpatialElementTag)))
      {
        SpatialElementTag parent = this.Parent as SpatialElementTag;
        CustomLeader customLeader = new CustomLeader();
        if (parent.get_HasLeader())
          customLeader = new CustomLeader(parent.get_LeaderEnd(), new XYZ(0.0, 0.0, 0.0));
        parent.set_TagHeadPosition(translation.OfPoint(parent.get_TagHeadPosition()));
        if (!parent.get_HasLeader())
          return;
        parent.set_LeaderEnd(customLeader.End);
      }
      else
        this.Parent.get_Location().Move(this._ownerView.get_CropBox().get_Transform().OfVector(xyz));
    }
  }
}
