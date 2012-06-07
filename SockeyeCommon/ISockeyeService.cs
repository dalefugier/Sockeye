using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SockeyeCommon
{
  [ServiceContract]
  public interface ISockeyeService
  {
    /// <summary>
    /// Simple test to see if the service is operational
    /// </summary>
    [OperationContract]
    string Hello();
  }
}
