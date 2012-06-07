using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using SockeyeCommon;

namespace Sockeye
{
  [System.Runtime.InteropServices.Guid("da3e5534-c01b-42fa-9094-49443b882b6d")]
  public class SockeyeCommand : Command
  {
    static SockeyeCommand _instance;

    public SockeyeCommand()
    {
      _instance = this;
    }

    public static SockeyeCommand Instance
    {
      get { return _instance; }
    }

    public override string EnglishName
    {
      get { return "SockeyeCommand"; }
    }

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
      string message = string.Empty;
      try
      {
        SockeyeChannel channel = new SockeyeChannel();
        channel.Create();
        message = channel.Hello();
        channel.Dispose();
        channel = null;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, EnglishName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return Result.Failure;
      }

      MessageBox.Show(message, EnglishName, MessageBoxButtons.OK, MessageBoxIcon.Information);

      return Result.Success;
    }
  }
}
