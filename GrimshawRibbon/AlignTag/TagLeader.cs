// Decompiled with JetBrains decompiler
// Type: AlignTag.TagLeader
// Assembly: SR_RevitTools, Version=2018.1.10.0, Culture=neutral, PublicKeyToken=null
// MVID: 2BFDAADD-616B-45CD-981C-80A6E0D00445
// Assembly location: K:\Sheppard\addins\later\2019\SR_RevitTools.dll

using Autodesk.Revit.DB;
using System;

namespace AlignTag
{
  internal class TagLeader
  {
    private Document _doc;
    private View _currentView;
    private Element _taggedElement;
    private IndependentTag _tag;
    private XYZ _tagHeadPosition;
    private XYZ _headOffset;
    private XYZ _tagCenter;
    private Line _endLine;
    private Line _baseLine;
    private ViewSides _side;
    private XYZ _elbowPosition;
    private XYZ _leaderEnd;
    private double _tagHeight;
    private double _tagWidth;

    public TagLeader(IndependentTag tag, Document doc)
    {
      this._doc = doc;
      this._currentView = this._doc.GetElement(((Element) tag).get_OwnerViewId()) as View;
      this._tag = tag;
      this._taggedElement = TagLeader.GetTaggedElement(this._doc, this._tag);
      this._tagHeadPosition = this._currentView.get_CropBox().get_Transform().get_Inverse().OfPoint(tag.get_TagHeadPosition());
      this._tagHeadPosition = new XYZ(this._tagHeadPosition.get_X(), this._tagHeadPosition.get_Y(), 0.0);
      this._leaderEnd = TagLeader.GetLeaderEnd(this._taggedElement, this._currentView);
      this._side = XYZ.op_Division(XYZ.op_Addition(this._currentView.get_CropBox().get_Max(), this._currentView.get_CropBox().get_Min()), 2.0).get_X() <= this._leaderEnd.get_X() ? ViewSides.Right : ViewSides.Left;
      this.GetTagDimension();
    }

    public XYZ TagCenter
    {
      get
      {
        return this._tagCenter;
      }
      set
      {
        this._tagCenter = value;
        this.UpdateLeaderPosition();
      }
    }

    public Line EndLine
    {
      get
      {
        return this._endLine;
      }
    }

    public Line BaseLine
    {
      get
      {
        return this._baseLine;
      }
    }

    public ViewSides Side
    {
      get
      {
        return this._side;
      }
    }

    public XYZ ElbowPosition
    {
      get
      {
        return this._elbowPosition;
      }
    }

    private void UpdateLeaderPosition()
    {
      XYZ xyz = XYZ.op_Subtraction(this._leaderEnd, this._tagCenter);
      double num1 = xyz.get_X() * xyz.get_Y();
      double num2 = num1 / Math.Abs(num1);
      this._elbowPosition = XYZ.op_Addition(this._tagCenter, new XYZ(xyz.get_X() - xyz.get_Y() * Math.Tan(num2 * Math.PI / 4.0), 0.0, 0.0));
      this._endLine = this._leaderEnd.DistanceTo(this._elbowPosition) <= this._doc.get_Application().get_ShortCurveTolerance() ? Line.CreateBound(new XYZ(0.0, 0.0, 0.0), new XYZ(0.0, 0.0, 1.0)) : Line.CreateBound(this._leaderEnd, this._elbowPosition);
      if (this._elbowPosition.DistanceTo(this._tagCenter) > this._doc.get_Application().get_ShortCurveTolerance())
        this._baseLine = Line.CreateBound(this._elbowPosition, this._tagCenter);
      else
        this._baseLine = Line.CreateBound(new XYZ(0.0, 0.0, 0.0), new XYZ(0.0, 0.0, 1.0));
    }

    public XYZ LeaderEnd
    {
      get
      {
        return this._leaderEnd;
      }
    }

    public double TagHeight
    {
      get
      {
        return this._tagHeight;
      }
    }

    public double TagWidth
    {
      get
      {
        return this._tagWidth;
      }
    }

    private void GetTagDimension()
    {
      BoundingBoxXYZ boundingBox = ((Element) this._tag).get_BoundingBox(this._currentView);
      BoundingBoxXYZ cropBox = this._currentView.get_CropBox();
      this._tagHeight = cropBox.get_Transform().get_Inverse().OfPoint(boundingBox.get_Max()).get_Y() - cropBox.get_Transform().get_Inverse().OfPoint(boundingBox.get_Min()).get_Y();
      this._tagWidth = cropBox.get_Transform().get_Inverse().OfPoint(boundingBox.get_Max()).get_X() - cropBox.get_Transform().get_Inverse().OfPoint(boundingBox.get_Min()).get_X();
      this._tagCenter = XYZ.op_Division(XYZ.op_Addition(cropBox.get_Transform().get_Inverse().OfPoint(boundingBox.get_Max()), cropBox.get_Transform().get_Inverse().OfPoint(boundingBox.get_Min())), 2.0);
      this._tagCenter = new XYZ(this._tagCenter.get_X(), this._tagCenter.get_Y(), 0.0);
      this._headOffset = XYZ.op_Subtraction(this._tagHeadPosition, this._tagCenter);
    }

    public static Element GetTaggedElement(Document doc, IndependentTag tag)
    {
      return !ElementId.op_Equality(tag.get_TaggedElementId().get_HostElementId(), ElementId.get_InvalidElementId()) ? doc.GetElement(tag.get_TaggedElementId().get_HostElementId()) : (doc.GetElement(tag.get_TaggedElementId().get_LinkInstanceId()) as RevitLinkInstance).GetLinkDocument().GetElement(tag.get_TaggedElementId().get_LinkedElementId());
    }

    public static XYZ GetLeaderEnd(Element taggedElement, View currentView)
    {
      BoundingBoxXYZ boundingBox = taggedElement.get_BoundingBox(currentView);
      BoundingBoxXYZ cropBox = currentView.get_CropBox();
      XYZ xyz1 = new XYZ();
      XYZ xyz2 = boundingBox == null ? XYZ.op_Addition(XYZ.op_Division(XYZ.op_Addition(cropBox.get_Max(), cropBox.get_Min()), 2.0), new XYZ(0.001, 0.0, 0.0)) : XYZ.op_Division(XYZ.op_Addition(boundingBox.get_Max(), boundingBox.get_Min()), 2.0);
      XYZ xyz3 = cropBox.get_Transform().get_Inverse().OfPoint(xyz2);
      return new XYZ(Math.Round(xyz3.get_X(), 4), Math.Round(xyz3.get_Y(), 4), 0.0);
    }

    public void UpdateTagPosition()
    {
      this._tag.set_LeaderEndCondition((LeaderEndCondition) 0);
      XYZ xyz = new XYZ();
      this._tag.set_TagHeadPosition(this._currentView.get_CropBox().get_Transform().OfPoint(XYZ.op_Addition(XYZ.op_Addition(this._headOffset, this._tagCenter), this._side != ViewSides.Left ? new XYZ(Math.Abs(this._tagWidth) * 0.5 + 0.1, 0.0, 0.0) : new XYZ(-Math.Abs(this._tagWidth) * 0.5 - 0.1, 0.0, 0.0))));
      this._tag.set_LeaderElbow(this._currentView.get_CropBox().get_Transform().OfPoint(this._elbowPosition));
    }
  }
}
