using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using SockeyeCommon;
using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SockeyeServer
{
  [Transaction(TransactionMode.Automatic)]
  [Regeneration(RegenerationOption.Manual)]
  public class SockeyeServerApp : IExternalApplication
  {
    #region IExternalApplication Members

    static SockeyeServerApp _instance;
    static UIControlledApplication _application;
    static string _assemblyTitle = "Sockeye";
    static string _assemblyLocation = Assembly.GetExecutingAssembly().Location;
    static string _assemblyDirectory = Path.GetDirectoryName(_assemblyLocation);
    static ServiceHost _serviceHost;

    public static SockeyeServerApp Instance
    {
      get { return _instance; }
    }

    public static UIControlledApplication Application
    {
      get { return _application; }
    }

    public Result OnStartup(UIControlledApplication application)
    {
      _instance = this;
      _application = application;

      try
      {
        _serviceHost = new ServiceHost(typeof(SockeyeService), new Uri("net.pipe://localhost/mcneel/sockeyeserver/1/server"));
      }
      catch
      {
        TaskDialog.Show("Sockeye", "Failed to create Sockeye service host.");
        throw;
      }

      try
      {
        _serviceHost.AddServiceEndpoint(typeof(ISockeyeService), new NetNamedPipeBinding(), "pipe");
      }
      catch
      {
        TaskDialog.Show("Sockeye", "Failed to create Sockeye service end point.");
        throw;
      }

      try
      {
        ServiceDebugBehavior debugBehavior = _serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
        if (null == debugBehavior)
        {
          debugBehavior = new ServiceDebugBehavior();
          debugBehavior.IncludeExceptionDetailInFaults = true;
          _serviceHost.Description.Behaviors.Add(debugBehavior);
        }
        else
        {
          debugBehavior.IncludeExceptionDetailInFaults = true;
        }
      }
      catch
      {
        TaskDialog.Show("Sockeye", "Failed to create Sockeye service debug behavior.");
        throw;
      }

      try
      {
        _serviceHost.Open();
      }
      catch
      {
        TaskDialog.Show("Sockeye", "Failed to open Sockeye service.");
        throw;
      }

      RibbonPanel ribbonPanel = application.CreateRibbonPanel(_assemblyTitle);

      PushButton pushButton = ribbonPanel.AddItem(new PushButtonData(
        _assemblyTitle, _assemblyTitle, _assemblyLocation, "SockeyeServer.Commands.CmdAbout")) as PushButton;

      Bitmap logo = SockeyeServer.Properties.Resources.Logo_32_32;

      BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
        logo.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

      pushButton.LargeImage = bitmapSource;
      pushButton.Image = bitmapSource;

      return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
      if (null != _serviceHost)
      {
        _serviceHost.Close();
        _serviceHost = null;
      }

      return Result.Succeeded;
    }

    #endregion
  }
}
