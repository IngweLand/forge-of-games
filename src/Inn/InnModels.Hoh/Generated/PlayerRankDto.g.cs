// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: player_rank_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from player_rank_dto.proto</summary>
  public static partial class PlayerRankDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for player_rank_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PlayerRankDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVwbGF5ZXJfcmFua19kdG8ucHJvdG8ilQEKDVBsYXllclJhbmtEdG8SCgoC",
            "aWQYASABKAUSDAoEbmFtZRgCIAEoCRIRCglhdmF0YXJfaWQYAyABKAUSCwoD",
            "YWdlGAQgASgJEhoKDWFsbGlhbmNlX25hbWUYBSABKAlIAIgBARIMCgRyYW5r",
            "GAYgASgFEg4KBnBvaW50cxgHIAEoBUIQCg5fYWxsaWFuY2VfbmFtZUIfqgIc",
            "SW5nd2VsYW5kLkZvZy5Jbm4uTW9kZWxzLkhvaGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.PlayerRankDto), global::Ingweland.Fog.Inn.Models.Hoh.PlayerRankDto.Parser, new[]{ "Id", "Name", "AvatarId", "Age", "AllianceName", "Rank", "Points" }, new[]{ "AllianceName" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class PlayerRankDto : pb::IMessage<PlayerRankDto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<PlayerRankDto> _parser = new pb::MessageParser<PlayerRankDto>(() => new PlayerRankDto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<PlayerRankDto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.PlayerRankDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PlayerRankDto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PlayerRankDto(PlayerRankDto other) : this() {
      id_ = other.id_;
      name_ = other.name_;
      avatarId_ = other.avatarId_;
      age_ = other.age_;
      allianceName_ = other.allianceName_;
      rank_ = other.rank_;
      points_ = other.points_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public PlayerRankDto Clone() {
      return new PlayerRankDto(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private int id_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 2;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "avatar_id" field.</summary>
    public const int AvatarIdFieldNumber = 3;
    private int avatarId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int AvatarId {
      get { return avatarId_; }
      set {
        avatarId_ = value;
      }
    }

    /// <summary>Field number for the "age" field.</summary>
    public const int AgeFieldNumber = 4;
    private string age_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Age {
      get { return age_; }
      set {
        age_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "alliance_name" field.</summary>
    public const int AllianceNameFieldNumber = 5;
    private readonly static string AllianceNameDefaultValue = "";

    private string allianceName_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string AllianceName {
      get { return allianceName_ ?? AllianceNameDefaultValue; }
      set {
        allianceName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "alliance_name" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasAllianceName {
      get { return allianceName_ != null; }
    }
    /// <summary>Clears the value of the "alliance_name" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearAllianceName() {
      allianceName_ = null;
    }

    /// <summary>Field number for the "rank" field.</summary>
    public const int RankFieldNumber = 6;
    private int rank_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Rank {
      get { return rank_; }
      set {
        rank_ = value;
      }
    }

    /// <summary>Field number for the "points" field.</summary>
    public const int PointsFieldNumber = 7;
    private int points_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Points {
      get { return points_; }
      set {
        points_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as PlayerRankDto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(PlayerRankDto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (Name != other.Name) return false;
      if (AvatarId != other.AvatarId) return false;
      if (Age != other.Age) return false;
      if (AllianceName != other.AllianceName) return false;
      if (Rank != other.Rank) return false;
      if (Points != other.Points) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (Id != 0) hash ^= Id.GetHashCode();
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (AvatarId != 0) hash ^= AvatarId.GetHashCode();
      if (Age.Length != 0) hash ^= Age.GetHashCode();
      if (HasAllianceName) hash ^= AllianceName.GetHashCode();
      if (Rank != 0) hash ^= Rank.GetHashCode();
      if (Points != 0) hash ^= Points.GetHashCode();
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
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Id);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (AvatarId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(AvatarId);
      }
      if (Age.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Age);
      }
      if (HasAllianceName) {
        output.WriteRawTag(42);
        output.WriteString(AllianceName);
      }
      if (Rank != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(Rank);
      }
      if (Points != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(Points);
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
      if (Id != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(Id);
      }
      if (Name.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Name);
      }
      if (AvatarId != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(AvatarId);
      }
      if (Age.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(Age);
      }
      if (HasAllianceName) {
        output.WriteRawTag(42);
        output.WriteString(AllianceName);
      }
      if (Rank != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(Rank);
      }
      if (Points != 0) {
        output.WriteRawTag(56);
        output.WriteInt32(Points);
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
      if (Id != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Id);
      }
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (AvatarId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AvatarId);
      }
      if (Age.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Age);
      }
      if (HasAllianceName) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AllianceName);
      }
      if (Rank != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Rank);
      }
      if (Points != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Points);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(PlayerRankDto other) {
      if (other == null) {
        return;
      }
      if (other.Id != 0) {
        Id = other.Id;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.AvatarId != 0) {
        AvatarId = other.AvatarId;
      }
      if (other.Age.Length != 0) {
        Age = other.Age;
      }
      if (other.HasAllianceName) {
        AllianceName = other.AllianceName;
      }
      if (other.Rank != 0) {
        Rank = other.Rank;
      }
      if (other.Points != 0) {
        Points = other.Points;
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
          case 8: {
            Id = input.ReadInt32();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 24: {
            AvatarId = input.ReadInt32();
            break;
          }
          case 34: {
            Age = input.ReadString();
            break;
          }
          case 42: {
            AllianceName = input.ReadString();
            break;
          }
          case 48: {
            Rank = input.ReadInt32();
            break;
          }
          case 56: {
            Points = input.ReadInt32();
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
          case 8: {
            Id = input.ReadInt32();
            break;
          }
          case 18: {
            Name = input.ReadString();
            break;
          }
          case 24: {
            AvatarId = input.ReadInt32();
            break;
          }
          case 34: {
            Age = input.ReadString();
            break;
          }
          case 42: {
            AllianceName = input.ReadString();
            break;
          }
          case 48: {
            Rank = input.ReadInt32();
            break;
          }
          case 56: {
            Points = input.ReadInt32();
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
