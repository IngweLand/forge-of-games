// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: hero_finish_wave_response.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from hero_finish_wave_response.proto</summary>
  public static partial class HeroFinishWaveResponseReflection {

    #region Descriptor
    /// <summary>File descriptor for hero_finish_wave_response.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static HeroFinishWaveResponseReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch9oZXJvX2ZpbmlzaF93YXZlX3Jlc3BvbnNlLnByb3RvGh5nb29nbGUvcHJv",
            "dG9idWYvd3JhcHBlcnMucHJvdG8aGGJhdHRsZV9zdW1tYXJ5X2R0by5wcm90",
            "byJrChZIZXJvRmluaXNoV2F2ZVJlc3BvbnNlEi4KCWJhdHRsZV9pZBgBIAEo",
            "CzIbLmdvb2dsZS5wcm90b2J1Zi5CeXRlc1ZhbHVlEiEKBnJlc3VsdBgCIAEo",
            "CzIRLkJhdHRsZVN1bW1hcnlEdG9CH6oCHEluZ3dlbGFuZC5Gb2cuSW5uLk1v",
            "ZGVscy5Ib2hiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor, global::Ingweland.Fog.Inn.Models.Hoh.BattleSummaryDtoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.HeroFinishWaveResponse), global::Ingweland.Fog.Inn.Models.Hoh.HeroFinishWaveResponse.Parser, new[]{ "BattleId", "Result" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class HeroFinishWaveResponse : pb::IMessage<HeroFinishWaveResponse>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<HeroFinishWaveResponse> _parser = new pb::MessageParser<HeroFinishWaveResponse>(() => new HeroFinishWaveResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<HeroFinishWaveResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.HeroFinishWaveResponseReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroFinishWaveResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroFinishWaveResponse(HeroFinishWaveResponse other) : this() {
      BattleId = other.BattleId;
      result_ = other.result_ != null ? other.result_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public HeroFinishWaveResponse Clone() {
      return new HeroFinishWaveResponse(this);
    }

    /// <summary>Field number for the "battle_id" field.</summary>
    public const int BattleIdFieldNumber = 1;
    private static readonly pb::FieldCodec<pb::ByteString> _single_battleId_codec = pb::FieldCodec.ForClassWrapper<pb::ByteString>(10);
    private pb::ByteString battleId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pb::ByteString BattleId {
      get { return battleId_; }
      set {
        battleId_ = value;
      }
    }


    /// <summary>Field number for the "result" field.</summary>
    public const int ResultFieldNumber = 2;
    private global::Ingweland.Fog.Inn.Models.Hoh.BattleSummaryDto result_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.BattleSummaryDto Result {
      get { return result_; }
      set {
        result_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as HeroFinishWaveResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(HeroFinishWaveResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (BattleId != other.BattleId) return false;
      if (!object.Equals(Result, other.Result)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (battleId_ != null) hash ^= BattleId.GetHashCode();
      if (result_ != null) hash ^= Result.GetHashCode();
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
      if (battleId_ != null) {
        _single_battleId_codec.WriteTagAndValue(output, BattleId);
      }
      if (result_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Result);
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
      if (battleId_ != null) {
        _single_battleId_codec.WriteTagAndValue(ref output, BattleId);
      }
      if (result_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Result);
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
      if (battleId_ != null) {
        size += _single_battleId_codec.CalculateSizeWithTag(BattleId);
      }
      if (result_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Result);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(HeroFinishWaveResponse other) {
      if (other == null) {
        return;
      }
      if (other.battleId_ != null) {
        if (battleId_ == null || other.BattleId != pb::ByteString.Empty) {
          BattleId = other.BattleId;
        }
      }
      if (other.result_ != null) {
        if (result_ == null) {
          Result = new global::Ingweland.Fog.Inn.Models.Hoh.BattleSummaryDto();
        }
        Result.MergeFrom(other.Result);
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
          case 10: {
            pb::ByteString value = _single_battleId_codec.Read(input);
            if (battleId_ == null || value != pb::ByteString.Empty) {
              BattleId = value;
            }
            break;
          }
          case 18: {
            if (result_ == null) {
              Result = new global::Ingweland.Fog.Inn.Models.Hoh.BattleSummaryDto();
            }
            input.ReadMessage(Result);
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
          case 10: {
            pb::ByteString value = _single_battleId_codec.Read(ref input);
            if (battleId_ == null || value != pb::ByteString.Empty) {
              BattleId = value;
            }
            break;
          }
          case 18: {
            if (result_ == null) {
              Result = new global::Ingweland.Fog.Inn.Models.Hoh.BattleSummaryDto();
            }
            input.ReadMessage(Result);
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
