// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: increase_building_limit_reward_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from increase_building_limit_reward_dto.proto</summary>
  public static partial class IncreaseBuildingLimitRewardDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for increase_building_limit_reward_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static IncreaseBuildingLimitRewardDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CihpbmNyZWFzZV9idWlsZGluZ19saW1pdF9yZXdhcmRfZHRvLnByb3RvGhhi",
            "dWlsZGluZ19ncm91cF9kdG8ucHJvdG8iZwoeSW5jcmVhc2VCdWlsZGluZ0xp",
            "bWl0UmV3YXJkRFRPEgoKAnUxGAEgASgJEikKDmJ1aWxkaW5nX2dyb3VwGAMg",
            "ASgLMhEuQnVpbGRpbmdHcm91cER0bxIOCgZjaXRpZXMYBCADKAlCH6oCHElu",
            "Z3dlbGFuZC5Gb2cuSW5uLk1vZGVscy5Ib2hiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Ingweland.Fog.Inn.Models.Hoh.BuildingGroupDtoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.IncreaseBuildingLimitRewardDTO), global::Ingweland.Fog.Inn.Models.Hoh.IncreaseBuildingLimitRewardDTO.Parser, new[]{ "U1", "BuildingGroup", "Cities" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class IncreaseBuildingLimitRewardDTO : pb::IMessage<IncreaseBuildingLimitRewardDTO>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<IncreaseBuildingLimitRewardDTO> _parser = new pb::MessageParser<IncreaseBuildingLimitRewardDTO>(() => new IncreaseBuildingLimitRewardDTO());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<IncreaseBuildingLimitRewardDTO> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.IncreaseBuildingLimitRewardDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IncreaseBuildingLimitRewardDTO() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IncreaseBuildingLimitRewardDTO(IncreaseBuildingLimitRewardDTO other) : this() {
      u1_ = other.u1_;
      buildingGroup_ = other.buildingGroup_ != null ? other.buildingGroup_.Clone() : null;
      cities_ = other.cities_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public IncreaseBuildingLimitRewardDTO Clone() {
      return new IncreaseBuildingLimitRewardDTO(this);
    }

    /// <summary>Field number for the "u1" field.</summary>
    public const int U1FieldNumber = 1;
    private string u1_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string U1 {
      get { return u1_; }
      set {
        u1_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "building_group" field.</summary>
    public const int BuildingGroupFieldNumber = 3;
    private global::Ingweland.Fog.Inn.Models.Hoh.BuildingGroupDto buildingGroup_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.BuildingGroupDto BuildingGroup {
      get { return buildingGroup_; }
      set {
        buildingGroup_ = value;
      }
    }

    /// <summary>Field number for the "cities" field.</summary>
    public const int CitiesFieldNumber = 4;
    private static readonly pb::FieldCodec<string> _repeated_cities_codec
        = pb::FieldCodec.ForString(34);
    private readonly pbc::RepeatedField<string> cities_ = new pbc::RepeatedField<string>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<string> Cities {
      get { return cities_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as IncreaseBuildingLimitRewardDTO);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(IncreaseBuildingLimitRewardDTO other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (U1 != other.U1) return false;
      if (!object.Equals(BuildingGroup, other.BuildingGroup)) return false;
      if(!cities_.Equals(other.cities_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (U1.Length != 0) hash ^= U1.GetHashCode();
      if (buildingGroup_ != null) hash ^= BuildingGroup.GetHashCode();
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
      if (U1.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(U1);
      }
      if (buildingGroup_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(BuildingGroup);
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
      if (U1.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(U1);
      }
      if (buildingGroup_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(BuildingGroup);
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
      if (U1.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(U1);
      }
      if (buildingGroup_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BuildingGroup);
      }
      size += cities_.CalculateSize(_repeated_cities_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(IncreaseBuildingLimitRewardDTO other) {
      if (other == null) {
        return;
      }
      if (other.U1.Length != 0) {
        U1 = other.U1;
      }
      if (other.buildingGroup_ != null) {
        if (buildingGroup_ == null) {
          BuildingGroup = new global::Ingweland.Fog.Inn.Models.Hoh.BuildingGroupDto();
        }
        BuildingGroup.MergeFrom(other.BuildingGroup);
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
          case 10: {
            U1 = input.ReadString();
            break;
          }
          case 26: {
            if (buildingGroup_ == null) {
              BuildingGroup = new global::Ingweland.Fog.Inn.Models.Hoh.BuildingGroupDto();
            }
            input.ReadMessage(BuildingGroup);
            break;
          }
          case 34: {
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
          case 10: {
            U1 = input.ReadString();
            break;
          }
          case 26: {
            if (buildingGroup_ == null) {
              BuildingGroup = new global::Ingweland.Fog.Inn.Models.Hoh.BuildingGroupDto();
            }
            input.ReadMessage(BuildingGroup);
            break;
          }
          case 34: {
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
