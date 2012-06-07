using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using SockeyeCommon;
using Autodesk.Revit;

namespace SockeyeServer
{
  /// <summary>
  /// One and only implementation of ISockeyeService
  /// </summary>
  public class SockeyeService : ISockeyeService
  {
    /// <summary>
    /// Public constructor
    /// </summary>
    public SockeyeService()
    {
    }

    /// <summary>
    /// Simple test to see if the service is operational
    /// </summary>
    public string Hello()
    {
      Autodesk.Revit.UI.UIControlledApplication uiapp = SockeyeServerApp.Application;
      Autodesk.Revit.ApplicationServices.ControlledApplication app = uiapp.ControlledApplication;
      string message = string.Format("{0} Build {1}", app.VersionName, app.VersionBuild);
      return message;
    }
  }
}
