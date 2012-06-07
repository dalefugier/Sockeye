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
  class CmdAbout : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
      TaskDialog.Show("Sockeye", "Sockeye Server v0.1");

      return Result.Succeeded;
    }

    #endregion
  }
}
