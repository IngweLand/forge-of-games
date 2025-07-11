// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: other_city_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from other_city_dto.proto</summary>
  public static partial class OtherCityDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for other_city_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static OtherCityDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChRvdGhlcl9jaXR5X2R0by5wcm90bxoQcGxheWVyX2R0by5wcm90bxoZY2l0",
            "eV9tYXBfZW50aXR5X2R0by5wcm90bxoeZXhwYW5zaW9uX21hcF9lbnRpdHlf",
            "ZHRvLnByb3RvGhpyZXdvcmtlZF93b25kZXJzX2R0by5wcm90byLrAQoMT3Ro",
            "ZXJDaXR5RFRPEhYKDnBsYXllcl9jaXR5X2lkGAEgASgFEhoKBnBsYXllchgC",
            "IAEoCzIKLlBsYXllckR0bxIPCgdjaXR5X2lkGAMgASgJEicKDG1hcF9lbnRp",
            "dGllcxgFIAMoCzIRLkNpdHlNYXBFbnRpdHlEdG8SNgoWZXhwYW5zaW9uX21h",
            "cF9lbnRpdGllcxgGIAMoCzIWLkV4cGFuc2lvbk1hcEVudGl0eUR0bxIpCgd3",
            "b25kZXJzGAcgASgLMhMuUmV3b3JrZWRXb25kZXJzRFRPSACIAQFCCgoIX3dv",
            "bmRlcnNCH6oCHEluZ3dlbGFuZC5Gb2cuSW5uLk1vZGVscy5Ib2hiBnByb3Rv",
            "Mw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Ingweland.Fog.Inn.Models.Hoh.PlayerDtoReflection.Descriptor, global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDtoReflection.Descriptor, global::Ingweland.Fog.Inn.Models.Hoh.ExpansionMapEntityDtoReflection.Descriptor, global::Ingweland.Fog.Inn.Models.Hoh.ReworkedWondersDtoReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.OtherCityDTO), global::Ingweland.Fog.Inn.Models.Hoh.OtherCityDTO.Parser, new[]{ "PlayerCityId", "Player", "CityId", "MapEntities", "ExpansionMapEntities", "Wonders" }, new[]{ "Wonders" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class OtherCityDTO : pb::IMessage<OtherCityDTO>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<OtherCityDTO> _parser = new pb::MessageParser<OtherCityDTO>(() => new OtherCityDTO());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<OtherCityDTO> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.OtherCityDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OtherCityDTO() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OtherCityDTO(OtherCityDTO other) : this() {
      playerCityId_ = other.playerCityId_;
      player_ = other.player_ != null ? other.player_.Clone() : null;
      cityId_ = other.cityId_;
      mapEntities_ = other.mapEntities_.Clone();
      expansionMapEntities_ = other.expansionMapEntities_.Clone();
      wonders_ = other.wonders_ != null ? other.wonders_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public OtherCityDTO Clone() {
      return new OtherCityDTO(this);
    }

    /// <summary>Field number for the "player_city_id" field.</summary>
    public const int PlayerCityIdFieldNumber = 1;
    private int playerCityId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int PlayerCityId {
      get { return playerCityId_; }
      set {
        playerCityId_ = value;
      }
    }

    /// <summary>Field number for the "player" field.</summary>
    public const int PlayerFieldNumber = 2;
    private global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto player_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto Player {
      get { return player_; }
      set {
        player_ = value;
      }
    }

    /// <summary>Field number for the "city_id" field.</summary>
    public const int CityIdFieldNumber = 3;
    private string cityId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string CityId {
      get { return cityId_; }
      set {
        cityId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "map_entities" field.</summary>
    public const int MapEntitiesFieldNumber = 5;
    private static readonly pb::FieldCodec<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDto> _repeated_mapEntities_codec
        = pb::FieldCodec.ForMessage(42, global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDto.Parser);
    private readonly pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDto> mapEntities_ = new pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDto>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDto> MapEntities {
      get { return mapEntities_; }
    }

    /// <summary>Field number for the "expansion_map_entities" field.</summary>
    public const int ExpansionMapEntitiesFieldNumber = 6;
    private static readonly pb::FieldCodec<global::Ingweland.Fog.Inn.Models.Hoh.ExpansionMapEntityDto> _repeated_expansionMapEntities_codec
        = pb::FieldCodec.ForMessage(50, global::Ingweland.Fog.Inn.Models.Hoh.ExpansionMapEntityDto.Parser);
    private readonly pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.ExpansionMapEntityDto> expansionMapEntities_ = new pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.ExpansionMapEntityDto>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.ExpansionMapEntityDto> ExpansionMapEntities {
      get { return expansionMapEntities_; }
    }

    /// <summary>Field number for the "wonders" field.</summary>
    public const int WondersFieldNumber = 7;
    private global::Ingweland.Fog.Inn.Models.Hoh.ReworkedWondersDTO wonders_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.ReworkedWondersDTO Wonders {
      get { return wonders_; }
      set {
        wonders_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as OtherCityDTO);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(OtherCityDTO other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (PlayerCityId != other.PlayerCityId) return false;
      if (!object.Equals(Player, other.Player)) return false;
      if (CityId != other.CityId) return false;
      if(!mapEntities_.Equals(other.mapEntities_)) return false;
      if(!expansionMapEntities_.Equals(other.expansionMapEntities_)) return false;
      if (!object.Equals(Wonders, other.Wonders)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (PlayerCityId != 0) hash ^= PlayerCityId.GetHashCode();
      if (player_ != null) hash ^= Player.GetHashCode();
      if (CityId.Length != 0) hash ^= CityId.GetHashCode();
      hash ^= mapEntities_.GetHashCode();
      hash ^= expansionMapEntities_.GetHashCode();
      if (wonders_ != null) hash ^= Wonders.GetHashCode();
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
      if (PlayerCityId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(PlayerCityId);
      }
      if (player_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Player);
      }
      if (CityId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(CityId);
      }
      mapEntities_.WriteTo(output, _repeated_mapEntities_codec);
      expansionMapEntities_.WriteTo(output, _repeated_expansionMapEntities_codec);
      if (wonders_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(Wonders);
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
      if (PlayerCityId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(PlayerCityId);
      }
      if (player_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Player);
      }
      if (CityId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(CityId);
      }
      mapEntities_.WriteTo(ref output, _repeated_mapEntities_codec);
      expansionMapEntities_.WriteTo(ref output, _repeated_expansionMapEntities_codec);
      if (wonders_ != null) {
        output.WriteRawTag(58);
        output.WriteMessage(Wonders);
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
      if (PlayerCityId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(PlayerCityId);
      }
      if (player_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Player);
      }
      if (CityId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CityId);
      }
      size += mapEntities_.CalculateSize(_repeated_mapEntities_codec);
      size += expansionMapEntities_.CalculateSize(_repeated_expansionMapEntities_codec);
      if (wonders_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Wonders);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(OtherCityDTO other) {
      if (other == null) {
        return;
      }
      if (other.PlayerCityId != 0) {
        PlayerCityId = other.PlayerCityId;
      }
      if (other.player_ != null) {
        if (player_ == null) {
          Player = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
        }
        Player.MergeFrom(other.Player);
      }
      if (other.CityId.Length != 0) {
        CityId = other.CityId;
      }
      mapEntities_.Add(other.mapEntities_);
      expansionMapEntities_.Add(other.expansionMapEntities_);
      if (other.wonders_ != null) {
        if (wonders_ == null) {
          Wonders = new global::Ingweland.Fog.Inn.Models.Hoh.ReworkedWondersDTO();
        }
        Wonders.MergeFrom(other.Wonders);
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
            PlayerCityId = input.ReadInt32();
            break;
          }
          case 18: {
            if (player_ == null) {
              Player = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
            }
            input.ReadMessage(Player);
            break;
          }
          case 26: {
            CityId = input.ReadString();
            break;
          }
          case 42: {
            mapEntities_.AddEntriesFrom(input, _repeated_mapEntities_codec);
            break;
          }
          case 50: {
            expansionMapEntities_.AddEntriesFrom(input, _repeated_expansionMapEntities_codec);
            break;
          }
          case 58: {
            if (wonders_ == null) {
              Wonders = new global::Ingweland.Fog.Inn.Models.Hoh.ReworkedWondersDTO();
            }
            input.ReadMessage(Wonders);
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
            PlayerCityId = input.ReadInt32();
            break;
          }
          case 18: {
            if (player_ == null) {
              Player = new global::Ingweland.Fog.Inn.Models.Hoh.PlayerDto();
            }
            input.ReadMessage(Player);
            break;
          }
          case 26: {
            CityId = input.ReadString();
            break;
          }
          case 42: {
            mapEntities_.AddEntriesFrom(ref input, _repeated_mapEntities_codec);
            break;
          }
          case 50: {
            expansionMapEntities_.AddEntriesFrom(ref input, _repeated_expansionMapEntities_codec);
            break;
          }
          case 58: {
            if (wonders_ == null) {
              Wonders = new global::Ingweland.Fog.Inn.Models.Hoh.ReworkedWondersDTO();
            }
            input.ReadMessage(Wonders);
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
