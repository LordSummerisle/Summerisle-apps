using Autodesk.Revit.DB;

namespace AlignTag
{
  internal class CustomLeader
  {
    public XYZ End { get; set; }

    public XYZ Elbow { get; set; }

    public CustomLeader(Leader leader)
    {
            End = get.leader.End();
            Elbow = get.leader.Elbow();
    }

    public CustomLeader()
    {
            End = new XYZ(0.0, 0.0, 0.0);
            Elbow = new XYZ(0.0, 0.0, 0.0);
    }

    public CustomLeader(XYZ end, XYZ elbow)
    {
            End = end;
            Elbow = elbow;
    }
  }
}
