// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: alliance_member_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from alliance_member_dto.proto</summary>
  public static partial class AllianceMemberDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for alliance_member_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AllianceMemberDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChlhbGxpYW5jZV9tZW1iZXJfZHRvLnByb3RvGhBwbGF5ZXJfZHRvLnByb3Rv",
            "IkcKEUFsbGlhbmNlTWVtYmVyRHRvEhoKBnBsYXllchgHIAEoCzIKLlBsYXll",
            "ckR0bxIWCg5yYW5raW5nX3BvaW50cxgIIAEoBUIfqgIcSW5nd2VsYW5kLkZv",
            "Zy5Jbm4uTW9kZWxzLkhvaGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Ingweland.Fog.Inn.Models.Hoh.PlayerDtoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.AllianceMemberDto), global::Ingweland.Fog.Inn.Models.Hoh.AllianceMemberDto.Parser, new[]{ "Player", "RankingPoints" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class AllianceMemberDto : pb::IMessage<AllianceMemberDto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<AllianceMemberDto> _parser = new pb::MessageParser<AllianceMemberDto>(() => new AllianceMemberDto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<AllianceMemberDto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.AllianceMemberDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public AllianceMemberDto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public AllianceMemberDto(AllianceMemberDto other) : this() {
      player_ = other.player_ != null ? other.player_.Clone() : null;
      rankingPoints_ = other.rankingPoints_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public AllianceMemberDto Clone() {
      return new AllianceMemberDto(this);
    }

    /// <summary>Field number for the "player" field.</summary>
    public const int PlayerFieldNumber = 7;
    private global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto player_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    /// <summary>Field number for the "ranking_points" field.</summary>
    public const int RankingPointsFieldNumber = 8;
    private int rankingPoints_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int RankingPoints {
      get { return rankingPoints_; }
      set {
        rankingPoints_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as AllianceMemberDto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(AllianceMemberDto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Player, other.Player)) return false;
      if (RankingPoints != other.RankingPoints) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (player_ != null) hash ^= Player.GetHashCode();
      if (RankingPoints != 0) hash ^= RankingPoints.GetHashCode();
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
      if (player_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(Player);
      }
      if (RankingPoints != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(RankingPoints);
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
      if (player_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(Player);
      }
      if (RankingPoints != 0) {
        output.WriteRawTag(64);
        output.WriteInt32(RankingPoints);
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
      if (player_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Player);
      }
      if (RankingPoints != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RankingPoints);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(AllianceMemberDto other) {
      if (other == null) {
        return;
      }
      if (other.player_ != null) {
        if (player_ == null) {
          Player = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
        }
        Player.MergeFrom(other.Player);
      }
      if (other.RankingPoints != 0) {
        RankingPoints = other.RankingPoints;
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
            if (player_ == null) {
              Player = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
            }
            input.ReadMessage(Player);
            break;
          }
          case 64: {
            RankingPoints = input.ReadInt32();
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
            if (player_ == null) {
              Player = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
            }
            input.ReadMessage(Player);
            break;
          }
          case 64: {
            RankingPoints = input.ReadInt32();
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
