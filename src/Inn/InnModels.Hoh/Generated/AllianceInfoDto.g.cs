// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: alliance_info_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from alliance_info_dto.proto</summary>
  public static partial class AllianceInfoDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for alliance_info_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AllianceInfoDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdhbGxpYW5jZV9pbmZvX2R0by5wcm90bxoQcGxheWVyX2R0by5wcm90bxof",
            "Z29vZ2xlL3Byb3RvYnVmL3RpbWVzdGFtcC5wcm90byJgCg9BbGxpYW5jZUlu",
            "Zm9EdG8SGgoGbGVhZGVyGAcgASgLMgouUGxheWVyRHRvEjEKDXJlZ2lzdGVy",
            "ZWRfYXQYCiABKAsyGi5nb29nbGUucHJvdG9idWYuVGltZXN0YW1wQh+qAhxJ",
            "bmd3ZWxhbmQuRm9nLklubi5Nb2RlbHMuSG9oYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Ingweland.Fog.Inn.Models.Hoh.PlayerDtoReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.AllianceInfoDto), global::Ingweland.Fog.Inn.Models.Hoh.AllianceInfoDto.Parser, new[]{ "Leader", "RegisteredAt" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class AllianceInfoDto : pb::IMessage<AllianceInfoDto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<AllianceInfoDto> _parser = new pb::MessageParser<AllianceInfoDto>(() => new AllianceInfoDto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<AllianceInfoDto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.AllianceInfoDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public AllianceInfoDto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public AllianceInfoDto(AllianceInfoDto other) : this() {
      leader_ = other.leader_ != null ? other.leader_.Clone() : null;
      registeredAt_ = other.registeredAt_ != null ? other.registeredAt_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public AllianceInfoDto Clone() {
      return new AllianceInfoDto(this);
    }

    /// <summary>Field number for the "leader" field.</summary>
    public const int LeaderFieldNumber = 7;
    private global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto leader_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto Leader {
      get { return leader_; }
      set {
        leader_ = value;
      }
    }

    /// <summary>Field number for the "registered_at" field.</summary>
    public const int RegisteredAtFieldNumber = 10;
    private global::Google.Protobuf.WellKnownTypes.Timestamp registeredAt_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Google.Protobuf.WellKnownTypes.Timestamp RegisteredAt {
      get { return registeredAt_; }
      set {
        registeredAt_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as AllianceInfoDto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(AllianceInfoDto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Leader, other.Leader)) return false;
      if (!object.Equals(RegisteredAt, other.RegisteredAt)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (leader_ != null) hash ^= Leader.GetHashCode();
      if (registeredAt_ != null) hash ^= RegisteredAt.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (leader_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(Leader);
      }
      if (registeredAt_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(RegisteredAt);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (leader_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(Leader);
      }
      if (registeredAt_ != null) {
        output.WriteRawTag(82);
        output.WriteMessage(RegisteredAt);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (leader_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Leader);
      }
      if (registeredAt_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(RegisteredAt);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(AllianceInfoDto other) {
      if (other == null) {
        return;
      }
      if (other.leader_ != null) {
        if (leader_ == null) {
          Leader = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
        }
        Leader.MergeFrom(other.Leader);
      }
      if (other.registeredAt_ != null) {
        if (registeredAt_ == null) {
          RegisteredAt = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        RegisteredAt.MergeFrom(other.RegisteredAt);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 58: {
            if (leader_ == null) {
              Leader = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
            }
            input.ReadMessage(Leader);
            break;
          }
          case 82: {
            if (registeredAt_ == null) {
              RegisteredAt = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(RegisteredAt);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 58: {
            if (leader_ == null) {
              Leader = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
            }
            input.ReadMessage(Leader);
            break;
          }
          case 82: {
            if (registeredAt_ == null) {
              RegisteredAt = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(RegisteredAt);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
