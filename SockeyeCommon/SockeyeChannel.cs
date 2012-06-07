using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace SockeyeCommon
{
  /// <summary>
  /// Sockeye channel exceptions
  /// </summary>
  public class SockeyeChannelException : System.ApplicationException
  {
    public SockeyeChannelException()
      : base()
    {
    }

    public SockeyeChannelException(string message)
      : base(message)
    {
    }

    public SockeyeChannelException(string message, System.Exception inner)
      : base(message, inner)
    {
    }

    protected SockeyeChannelException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
    {
    }
  }

  /// <summary>
  /// Sockeye client channel
  /// </summary>
  public class SockeyeChannel : IDisposable
  {
    ChannelFactory<ISockeyeService> _factory;
    ISockeyeService _channel;
    NetNamedPipeBinding _binding;
    EndpointAddress _endpoint;
    object _locker;
    bool _disposed;

    /// <summary>
    /// Public constructor
    /// </summary>
    public SockeyeChannel()
    {
      _locker = new object();
      _disposed = false;
    }

    /// <summary>
    /// Public creator
    /// </summary>
    public bool Create()
    {
      bool rc = false;
      try
      {
        _binding = new NetNamedPipeBinding();
        _endpoint = new EndpointAddress("net.pipe://localhost/mcneel/sockeyeserver/1/server/pipe");
        _factory = new ChannelFactory<ISockeyeService>(_binding, _endpoint);
        _channel = _factory.CreateChannel();
        rc = true;
      }
      catch (Exception ex)
      {
        ThrowCreationException(ex);
        Dispose();
      }
      return rc;
    }

    /// <summary>
    /// Simple test to see if the service is operational
    /// </summary>
    public string Hello()
    {
      if (IsValid())
      {
        try
        {
          string result = _channel.Hello();
          return result;
        }
        catch (Exception ex)
        {
          HandleException(ex);
          Dispose();
        }
      }
      return null;
    }

    /// <summary>
    /// Validator
    /// </summary>
    public bool IsValid()
    {
      if (null != _factory && null != _channel && false == _disposed)
        return true;
      return false;
    }

    /// <summary>
    /// IsDisposed
    /// </summary>
    public bool IsDisposed()
    {
      return _disposed;
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        if (disposing)
        {
          lock (_locker)
          {
            if (null != _channel)
            {
              ((IClientChannel)_channel).Abort();
              _channel = null;
            }

            if (null != _factory)
            {
              _factory.Abort();
              _factory = null;
            }

            _endpoint = null;
            _binding = null;
          }

          _disposed = true;
        }
      }
    }

    /// <summary>
    /// Exception handler
    /// </summary>
    void HandleException(Exception ex)
    {
      if (ex is FaultException)
      {
        ThrowFaultException((FaultException)ex);
      }
      else if (ex is CommunicationException)
      {
        ThrowCommunicationException((CommunicationException)ex);
      }
      else if (ex is TimeoutException)
      {
        ThrowTimeoutException((TimeoutException)ex);
      }
      else
      {
        ThrowGeneralException(ex);
      }
    }

    /// <summary>
    /// Handles creation exceptions
    /// </summary>
    void ThrowCreationException(Exception ex)
    {
      System.Diagnostics.Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
      string message = "There was a problem creating the communication channel.";
      throw new SockeyeChannelException(message);
    }

    /// <summary>
    /// Handles fault exceptions
    /// </summary>
    void ThrowFaultException(FaultException ex)
    {
      System.Diagnostics.Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
      throw new SockeyeChannelException(ex.Message);
    }

    /// <summary>
    /// Handles communication exceptions
    /// </summary>
    void ThrowCommunicationException(CommunicationException ex)
    {
      System.Diagnostics.Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
      string message = "There was a problem communicating with the service.";
      throw new SockeyeChannelException(message);
    }

    /// <summary>
    /// Handles timeout exceptions
    /// </summary>
    void ThrowTimeoutException(TimeoutException ex)
    {
      System.Diagnostics.Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
      string message = "The service operation has timed out.";
      throw new SockeyeChannelException(message);
    }

    /// <summary>
    /// Handles generic exceptions
    /// </summary>
    void ThrowGeneralException(Exception ex)
    {
      System.Diagnostics.Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
      string message = "An unknown exception has occurred.";
      throw new SockeyeChannelException(message);
    }
  }
}
