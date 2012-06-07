using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SockeyeServer.Commands
{
  [Transaction(TransactionMode.Manual)]
  [Regeneration(RegenerationOption.Manual)]
  class CmdTestGableWall : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      Autodesk.Revit.UI.UIApplication uiapp = commandData.Application;
      Autodesk.Revit.UI.UIDocument uidoc = uiapp.ActiveUIDocument;
      Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
      Autodesk.Revit.DB.Document doc = uidoc.Document;

      // Build a wall profile for the wall creation 
      XYZ[] pts = new XYZ[] {
        XYZ.Zero,
        new XYZ(20, 0,  0),
        new XYZ(20, 0, 15),
        new XYZ(10, 0, 30),
        new XYZ( 0, 0, 15)
        };

      // Get application creation object 
      Autodesk.Revit.Creation.Application appCreation = app.Create;

      // Create wall profile
      CurveArray profile = new CurveArray();
      XYZ q = pts[pts.Length - 1];

      foreach (XYZ p in pts)
      {
        profile.Append(appCreation.NewLineBound(q, p));
        q = p;
      }

      XYZ normal = XYZ.BasisY;

      WallType wallType
        = new FilteredElementCollector(doc)
          .OfClass(typeof(WallType))
          .First<Element>()
            as WallType;

      Level level
        = new FilteredElementCollector(doc)
          .OfClass(typeof(Level))
          .First<Element>(e
            => e.Name.Equals("Level 1"))
          as Level;

      Transaction trans = new Transaction(doc);
      trans.Start("Test Gable Wall");
      Wall wall = doc.Create.NewWall(profile, wallType, level, true, normal);
      trans.Commit();

      return Result.Succeeded;
    }

    #endregion
  }
}
