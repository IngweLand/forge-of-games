// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: instant_upgrade_reward_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from instant_upgrade_reward_dto.proto</summary>
  public static partial class InstantUpgradeRewardDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for instant_upgrade_reward_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static InstantUpgradeRewardDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiBpbnN0YW50X3VwZ3JhZGVfcmV3YXJkX2R0by5wcm90byJgChdJbnN0YW50",
            "VXBncmFkZVJld2FyZERUTxIaChJ0YXJnZXRfYnVpbGRpbmdfaWQYAiABKAkS",
            "GQoRYnVpbGRpbmdfZ3JvdXBfaWQYCiABKAkSDgoGY2l0aWVzGAsgAygJQh+q",
            "AhxJbmd3ZWxhbmQuRm9nLklubi5Nb2RlbHMuSG9oYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.InstantUpgradeRewardDTO), global::Ingweland.Fog.Inn.Models.Hoh.InstantUpgradeRewardDTO.Parser, new[]{ "TargetBuildingId", "BuildingGroupId", "Cities" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class InstantUpgradeRewardDTO : pb::IMessage<InstantUpgradeRewardDTO>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<InstantUpgradeRewardDTO> _parser = new pb::MessageParser<InstantUpgradeRewardDTO>(() => new InstantUpgradeRewardDTO());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<InstantUpgradeRewardDTO> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.InstantUpgradeRewardDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public InstantUpgradeRewardDTO() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public InstantUpgradeRewardDTO(InstantUpgradeRewardDTO other) : this() {
      targetBuildingId_ = other.targetBuildingId_;
      buildingGroupId_ = other.buildingGroupId_;
      cities_ = other.cities_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public InstantUpgradeRewardDTO Clone() {
      return new InstantUpgradeRewardDTO(this);
    }

    /// <summary>Field number for the "target_building_id" field.</summary>
    public const int TargetBuildingIdFieldNumber = 2;
    private string targetBuildingId_ = "";
    /// <summary>
    ///    1 {
    ///  1: "Technology_AgeOfTheFranks_RegnumFrancorum_TransitionReward_1"
    ///      }
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string TargetBuildingId {
      get { return targetBuildingId_; }
      set {
        targetBuildingId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "building_group_id" field.</summary>
    public const int BuildingGroupIdFieldNumber = 10;
    private string buildingGroupId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string BuildingGroupId {
      get { return buildingGroupId_; }
      set {
        buildingGroupId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "cities" field.</summary>
    public const int CitiesFieldNumber = 11;
    private static readonly pb::FieldCodec<string> _repeated_cities_codec
        = pb::FieldCodec.ForString(90);
    private readonly pbc::RepeatedField<string> cities_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<string> Cities {
      get { return cities_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as InstantUpgradeRewardDTO);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(InstantUpgradeRewardDTO other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (TargetBuildingId != other.TargetBuildingId) return false;
      if (BuildingGroupId != other.BuildingGroupId) return false;
      if(!cities_.Equals(other.cities_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (TargetBuildingId.Length != 0) hash ^= TargetBuildingId.GetHashCode();
      if (BuildingGroupId.Length != 0) hash ^= BuildingGroupId.GetHashCode();
      hash ^= cities_.GetHashCode();
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
      if (TargetBuildingId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(TargetBuildingId);
      }
      if (BuildingGroupId.Length != 0) {
        output.WriteRawTag(82);
        output.WriteString(BuildingGroupId);
      }
      cities_.WriteTo(output, _repeated_cities_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (TargetBuildingId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(TargetBuildingId);
      }
      if (BuildingGroupId.Length != 0) {
        output.WriteRawTag(82);
        output.WriteString(BuildingGroupId);
      }
      cities_.WriteTo(ref output, _repeated_cities_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (TargetBuildingId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(TargetBuildingId);
      }
      if (BuildingGroupId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(BuildingGroupId);
      }
      size += cities_.CalculateSize(_repeated_cities_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(InstantUpgradeRewardDTO other) {
      if (other == null) {
        return;
      }
      if (other.TargetBuildingId.Length != 0) {
        TargetBuildingId = other.TargetBuildingId;
      }
      if (other.BuildingGroupId.Length != 0) {
        BuildingGroupId = other.BuildingGroupId;
      }
      cities_.Add(other.cities_);
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
          case 18: {
            TargetBuildingId = input.ReadString();
            break;
          }
          case 82: {
            BuildingGroupId = input.ReadString();
            break;
          }
          case 90: {
            cities_.AddEntriesFrom(input, _repeated_cities_codec);
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
          case 18: {
            TargetBuildingId = input.ReadString();
            break;
          }
          case 82: {
            BuildingGroupId = input.ReadString();
            break;
          }
          case 90: {
            cities_.AddEntriesFrom(ref input, _repeated_cities_codec);
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
