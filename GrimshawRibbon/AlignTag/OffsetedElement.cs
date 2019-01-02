// Decompiled with JetBrains decompiler
// Type: AlignTag.OffsetedElement
// Assembly: SR_RevitTools, Version=2018.1.10.0, Culture=neutral, PublicKeyToken=null
// MVID: 2BFDAADD-616B-45CD-981C-80A6E0D00445
// Assembly location: K:\Sheppard\addins\later\2019\SR_RevitTools.dll

using Autodesk.Revit.DB;

namespace AlignTag
{
  internal class OffsetedElement
  {
    public OffsetedElement(Element e, XYZ offset)
    {
      this.Element = e;
      this.Offset = offset;
    }

    public Element Element { get; set; }

    public XYZ Offset { get; set; }
  }
}
