using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SockeyeCommon
{
  [DataContract]
  public class SockeyePoint
  {
    private const double _UnsetValue = -1.23432101234321e+308;

    [DataMember]
    public double X { get; set; }

    [DataMember]
    public double Y { get; set; }

    [DataMember]
    public double Z { get; set; }

    public SockeyePoint()
    {
      X = _UnsetValue;
      Y = _UnsetValue;
      Z = _UnsetValue;
    }

    public SockeyePoint(double x, double y, double z)
    {
      X = x;
      Y = y;
      Z = z;
    }

    public static SockeyePoint Zero
    { 
      get { return new SockeyePoint(0.0, 0.0, 0.0); }
    }
  }
}
