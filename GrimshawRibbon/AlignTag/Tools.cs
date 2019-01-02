using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace AlignTag
{
    internal class Tools
    {
        public static ResourceManager LangResMan;
        public static CultureInfo Cult;

        public static void GetLocalisationValues()
        {
            Tools.Cult = CultureInfo.CreateSpecificCulture("en");
            Tools.LangResMan = new ResourceManager("AEW_Ribbon.Resources.en", Assembly.GetExecutingAssembly());
        }

        public static double? GetValueFromString(string text)
        {
            double result;
            if (double.TryParse(!text.Contains(" mm") ? (!text.Contains("mm") ? text : text.Replace("mm", "")) : text.Replace(" mm", ""), out result))
                return new double?(result);
            return new double?();
        }

        public static void ExtractRessource(string resourceName, string path)
        {
            using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (Stream stream = (Stream)File.Create(path))
                {
                    byte[] buffer = new byte[8192];
                    int count;
                    while ((count = manifestResourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        stream.Write(buffer, 0, count);
                }
            }
        }

        internal static ICollection<ElementId> RevitReferencesToElementIds(Autodesk.Revit.DB.Document doc, IList<Reference> selectedReferences)
        {
            return (ICollection<ElementId>)((IEnumerable<Reference>)selectedReferences).Select<Reference, ElementId>((Func<Reference, ElementId>)(x => doc.GetElement(x).get_Id())).ToList<ElementId>();
        }

        public static void CreateDebuggingSphere(Autodesk.Revit.DB.Document doc, XYZ point, string value, Color color)
        {
            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Create Direct Shapes Spheres");
                Solid sphereAt = Tools.CreateSphereAt(point, 0.5);
                Tools.CreateDirectShape(doc, sphereAt, color, value + "{" + (object)point.get_X() + "," + (object)point.get_Y() + "," + (object)point.get_Z() + "}");
                transaction.Commit();
            }
        }

        public static void DrawInView(Document doc, View view, XYZ point)
        {
            XYZ xyz = view.get_CropBox().get_Transform().get_Inverse().OfPoint(point);
            Plane byNormalAndOrigin = Plane.CreateByNormalAndOrigin(view.get_ViewDirection(), xyz);
            Arc arc = Arc.Create(xyz, 10.0, 0.0, 2.0 * Math.PI, byNormalAndOrigin.get_XVec(), byNormalAndOrigin.get_YVec());
            SketchPlane.Create(doc, byNormalAndOrigin);
            DetailLine detailLine = ((ItemFactoryBase)doc.get_Create()).NewDetailCurve(view, (Curve)arc) as DetailLine;
        }

        public void DrawInViewCoordinates(Autodesk.Revit.DB.Document doc)
        {
            View activeView = doc.get_ActiveView();
            XYZ xyz1 = new XYZ(0.0, 0.0, 0.0);
            Solid sphereAt1 = Tools.CreateSphereAt(activeView.get_CropBox().get_Transform().OfPoint(xyz1), 0.1);
            Tools.CreateDirectShape(doc, sphereAt1, new Color((byte)0, byte.MaxValue, (byte)0), "View Origin");
            XYZ xyz2 = new XYZ(1.0, 0.0, 0.0);
            Solid sphereAt2 = Tools.CreateSphereAt(activeView.get_CropBox().get_Transform().OfPoint(xyz2), 0.1);
            Tools.CreateDirectShape(doc, sphereAt2, new Color((byte)0, byte.MaxValue, (byte)0), "View X");
            XYZ xyz3 = new XYZ(0.0, 1.0, 0.0);
            Solid sphereAt3 = Tools.CreateSphereAt(activeView.get_CropBox().get_Transform().OfPoint(xyz3), 0.1);
            Tools.CreateDirectShape(doc, sphereAt3, new Color((byte)0, byte.MaxValue, (byte)0), "View Y");
            XYZ xyz4 = new XYZ(0.0, 0.0, 1.0);
            Solid sphereAt4 = Tools.CreateSphereAt(activeView.get_CropBox().get_Transform().OfPoint(xyz4), 0.1);
            Tools.CreateDirectShape(doc, sphereAt4, new Color((byte)0, byte.MaxValue, (byte)0), "View Z");
        }

        private static void CreateDirectShape(Autodesk.Revit.DB.Document doc, Solid solid, Color color, string paramValue)
        {
            OverrideGraphicSettings overrideGraphicSettings = new OverrideGraphicSettings();
            DirectShape element = DirectShape.CreateElement(doc, new ElementId((BuiltInCategory) - 2000151));
            element.set_ApplicationId("ApplicationID");
            element.set_ApplicationDataId("ApplicationDataId");
            element.SetShape((IList<GeometryObject>)new GeometryObject[1]
            {
        (GeometryObject) solid
            });
            doc.get_ActiveView().SetElementOverrides(((Element)element).get_Id(), overrideGraphicSettings);
            ((Element)element).get_Parameter((BuiltInParameter) - 1010106).Set(paramValue);
        }

        private static Solid CreateSphereAt(XYZ centre, double radius)
        {
            Frame frame = new Frame(centre, XYZ.get_BasisX(), XYZ.get_BasisY(), XYZ.get_BasisZ());
            Arc arc = Arc.Create(XYZ.op_Subtraction(centre, XYZ.op_Multiply(radius, XYZ.get_BasisZ())), XYZ.op_Addition(centre, XYZ.op_Multiply(radius, XYZ.get_BasisZ())), XYZ.op_Addition(centre, XYZ.op_Multiply(radius, XYZ.get_BasisX())));
            Line bound = Line.CreateBound(((Curve)arc).GetEndPoint(1), ((Curve)arc).GetEndPoint(0));
            CurveLoop curveLoop = new CurveLoop();
            curveLoop.Append((Curve)arc);
            curveLoop.Append((Curve)bound);
            List<CurveLoop> curveLoopList = new List<CurveLoop>(1);
            curveLoopList.Add(curveLoop);
            return GeometryCreationUtilities.CreateRevolvedGeometry(frame, (IList<CurveLoop>)curveLoopList, 0.0, 2.0 * Math.PI);
        }

        internal static void ExtractResource(string v, string path)
        {
            throw new NotImplementedException();
        }
    }
}
