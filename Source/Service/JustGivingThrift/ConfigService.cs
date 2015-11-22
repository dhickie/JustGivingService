/**
 * Autogenerated by Thrift Compiler (0.9.3)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace JustGivingThrift
{
  public partial class ConfigService {
    public interface Iface {
      /// <summary>
      /// Sets the current service configuration
      /// </summary>
      /// <param name="newConfig"></param>
      void SetConfiguration(ServiceConfig newConfig);
      #if SILVERLIGHT
      IAsyncResult Begin_SetConfiguration(AsyncCallback callback, object state, ServiceConfig newConfig);
      void End_SetConfiguration(IAsyncResult asyncResult);
      #endif
      /// <summary>
      /// Gets the current service configuration
      /// </summary>
      ServiceConfig GetConfiguration();
      #if SILVERLIGHT
      IAsyncResult Begin_GetConfiguration(AsyncCallback callback, object state);
      ServiceConfig End_GetConfiguration(IAsyncResult asyncResult);
      #endif
    }

    public class Client : IDisposable, Iface {
      public Client(TProtocol prot) : this(prot, prot)
      {
      }

      public Client(TProtocol iprot, TProtocol oprot)
      {
        iprot_ = iprot;
        oprot_ = oprot;
      }

      protected TProtocol iprot_;
      protected TProtocol oprot_;
      protected int seqid_;

      public TProtocol InputProtocol
      {
        get { return iprot_; }
      }
      public TProtocol OutputProtocol
      {
        get { return oprot_; }
      }


      #region " IDisposable Support "
      private bool _IsDisposed;

      // IDisposable
      public void Dispose()
      {
        Dispose(true);
      }
      

      protected virtual void Dispose(bool disposing)
      {
        if (!_IsDisposed)
        {
          if (disposing)
          {
            if (iprot_ != null)
            {
              ((IDisposable)iprot_).Dispose();
            }
            if (oprot_ != null)
            {
              ((IDisposable)oprot_).Dispose();
            }
          }
        }
        _IsDisposed = true;
      }
      #endregion


      
      #if SILVERLIGHT
      public IAsyncResult Begin_SetConfiguration(AsyncCallback callback, object state, ServiceConfig newConfig)
      {
        return send_SetConfiguration(callback, state, newConfig);
      }

      public void End_SetConfiguration(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        recv_SetConfiguration();
      }

      #endif

      /// <summary>
      /// Sets the current service configuration
      /// </summary>
      /// <param name="newConfig"></param>
      public void SetConfiguration(ServiceConfig newConfig)
      {
        #if !SILVERLIGHT
        send_SetConfiguration(newConfig);
        recv_SetConfiguration();

        #else
        var asyncResult = Begin_SetConfiguration(null, null, newConfig);
        End_SetConfiguration(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_SetConfiguration(AsyncCallback callback, object state, ServiceConfig newConfig)
      #else
      public void send_SetConfiguration(ServiceConfig newConfig)
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("SetConfiguration", TMessageType.Call, seqid_));
        SetConfiguration_args args = new SetConfiguration_args();
        args.NewConfig = newConfig;
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public void recv_SetConfiguration()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        SetConfiguration_result result = new SetConfiguration_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        return;
      }

      
      #if SILVERLIGHT
      public IAsyncResult Begin_GetConfiguration(AsyncCallback callback, object state)
      {
        return send_GetConfiguration(callback, state);
      }

      public ServiceConfig End_GetConfiguration(IAsyncResult asyncResult)
      {
        oprot_.Transport.EndFlush(asyncResult);
        return recv_GetConfiguration();
      }

      #endif

      /// <summary>
      /// Gets the current service configuration
      /// </summary>
      public ServiceConfig GetConfiguration()
      {
        #if !SILVERLIGHT
        send_GetConfiguration();
        return recv_GetConfiguration();

        #else
        var asyncResult = Begin_GetConfiguration(null, null);
        return End_GetConfiguration(asyncResult);

        #endif
      }
      #if SILVERLIGHT
      public IAsyncResult send_GetConfiguration(AsyncCallback callback, object state)
      #else
      public void send_GetConfiguration()
      #endif
      {
        oprot_.WriteMessageBegin(new TMessage("GetConfiguration", TMessageType.Call, seqid_));
        GetConfiguration_args args = new GetConfiguration_args();
        args.Write(oprot_);
        oprot_.WriteMessageEnd();
        #if SILVERLIGHT
        return oprot_.Transport.BeginFlush(callback, state);
        #else
        oprot_.Transport.Flush();
        #endif
      }

      public ServiceConfig recv_GetConfiguration()
      {
        TMessage msg = iprot_.ReadMessageBegin();
        if (msg.Type == TMessageType.Exception) {
          TApplicationException x = TApplicationException.Read(iprot_);
          iprot_.ReadMessageEnd();
          throw x;
        }
        GetConfiguration_result result = new GetConfiguration_result();
        result.Read(iprot_);
        iprot_.ReadMessageEnd();
        if (result.__isset.success) {
          return result.Success;
        }
        throw new TApplicationException(TApplicationException.ExceptionType.MissingResult, "GetConfiguration failed: unknown result");
      }

    }
    public class Processor : TProcessor {
      public Processor(Iface iface)
      {
        iface_ = iface;
        processMap_["SetConfiguration"] = SetConfiguration_Process;
        processMap_["GetConfiguration"] = GetConfiguration_Process;
      }

      protected delegate void ProcessFunction(int seqid, TProtocol iprot, TProtocol oprot);
      private Iface iface_;
      protected Dictionary<string, ProcessFunction> processMap_ = new Dictionary<string, ProcessFunction>();

      public bool Process(TProtocol iprot, TProtocol oprot)
      {
        try
        {
          TMessage msg = iprot.ReadMessageBegin();
          ProcessFunction fn;
          processMap_.TryGetValue(msg.Name, out fn);
          if (fn == null) {
            TProtocolUtil.Skip(iprot, TType.Struct);
            iprot.ReadMessageEnd();
            TApplicationException x = new TApplicationException (TApplicationException.ExceptionType.UnknownMethod, "Invalid method name: '" + msg.Name + "'");
            oprot.WriteMessageBegin(new TMessage(msg.Name, TMessageType.Exception, msg.SeqID));
            x.Write(oprot);
            oprot.WriteMessageEnd();
            oprot.Transport.Flush();
            return true;
          }
          fn(msg.SeqID, iprot, oprot);
        }
        catch (IOException)
        {
          return false;
        }
        return true;
      }

      public void SetConfiguration_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        SetConfiguration_args args = new SetConfiguration_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        SetConfiguration_result result = new SetConfiguration_result();
        iface_.SetConfiguration(args.NewConfig);
        oprot.WriteMessageBegin(new TMessage("SetConfiguration", TMessageType.Reply, seqid)); 
        result.Write(oprot);
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

      public void GetConfiguration_Process(int seqid, TProtocol iprot, TProtocol oprot)
      {
        GetConfiguration_args args = new GetConfiguration_args();
        args.Read(iprot);
        iprot.ReadMessageEnd();
        GetConfiguration_result result = new GetConfiguration_result();
        result.Success = iface_.GetConfiguration();
        oprot.WriteMessageBegin(new TMessage("GetConfiguration", TMessageType.Reply, seqid)); 
        result.Write(oprot);
        oprot.WriteMessageEnd();
        oprot.Transport.Flush();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SetConfiguration_args : TBase
    {
      private ServiceConfig _newConfig;

      public ServiceConfig NewConfig
      {
        get
        {
          return _newConfig;
        }
        set
        {
          __isset.newConfig = true;
          this._newConfig = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool newConfig;
      }

      public SetConfiguration_args() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 1:
                if (field.Type == TType.Struct) {
                  NewConfig = new ServiceConfig();
                  NewConfig.Read(iprot);
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("SetConfiguration_args");
          oprot.WriteStructBegin(struc);
          TField field = new TField();
          if (NewConfig != null && __isset.newConfig) {
            field.Name = "newConfig";
            field.Type = TType.Struct;
            field.ID = 1;
            oprot.WriteFieldBegin(field);
            NewConfig.Write(oprot);
            oprot.WriteFieldEnd();
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("SetConfiguration_args(");
        bool __first = true;
        if (NewConfig != null && __isset.newConfig) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("NewConfig: ");
          __sb.Append(NewConfig== null ? "<null>" : NewConfig.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class SetConfiguration_result : TBase
    {

      public SetConfiguration_result() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("SetConfiguration_result");
          oprot.WriteStructBegin(struc);

          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("SetConfiguration_result(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class GetConfiguration_args : TBase
    {

      public GetConfiguration_args() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("GetConfiguration_args");
          oprot.WriteStructBegin(struc);
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("GetConfiguration_args(");
        __sb.Append(")");
        return __sb.ToString();
      }

    }


    #if !SILVERLIGHT
    [Serializable]
    #endif
    public partial class GetConfiguration_result : TBase
    {
      private ServiceConfig _success;

      public ServiceConfig Success
      {
        get
        {
          return _success;
        }
        set
        {
          __isset.success = true;
          this._success = value;
        }
      }


      public Isset __isset;
      #if !SILVERLIGHT
      [Serializable]
      #endif
      public struct Isset {
        public bool success;
      }

      public GetConfiguration_result() {
      }

      public void Read (TProtocol iprot)
      {
        iprot.IncrementRecursionDepth();
        try
        {
          TField field;
          iprot.ReadStructBegin();
          while (true)
          {
            field = iprot.ReadFieldBegin();
            if (field.Type == TType.Stop) { 
              break;
            }
            switch (field.ID)
            {
              case 0:
                if (field.Type == TType.Struct) {
                  Success = new ServiceConfig();
                  Success.Read(iprot);
                } else { 
                  TProtocolUtil.Skip(iprot, field.Type);
                }
                break;
              default: 
                TProtocolUtil.Skip(iprot, field.Type);
                break;
            }
            iprot.ReadFieldEnd();
          }
          iprot.ReadStructEnd();
        }
        finally
        {
          iprot.DecrementRecursionDepth();
        }
      }

      public void Write(TProtocol oprot) {
        oprot.IncrementRecursionDepth();
        try
        {
          TStruct struc = new TStruct("GetConfiguration_result");
          oprot.WriteStructBegin(struc);
          TField field = new TField();

          if (this.__isset.success) {
            if (Success != null) {
              field.Name = "Success";
              field.Type = TType.Struct;
              field.ID = 0;
              oprot.WriteFieldBegin(field);
              Success.Write(oprot);
              oprot.WriteFieldEnd();
            }
          }
          oprot.WriteFieldStop();
          oprot.WriteStructEnd();
        }
        finally
        {
          oprot.DecrementRecursionDepth();
        }
      }

      public override string ToString() {
        StringBuilder __sb = new StringBuilder("GetConfiguration_result(");
        bool __first = true;
        if (Success != null && __isset.success) {
          if(!__first) { __sb.Append(", "); }
          __first = false;
          __sb.Append("Success: ");
          __sb.Append(Success== null ? "<null>" : Success.ToString());
        }
        __sb.Append(")");
        return __sb.ToString();
      }

    }

  }
}
