// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: city_map_entity_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from city_map_entity_dto.proto</summary>
  public static partial class CityMapEntityDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for city_map_entity_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CityMapEntityDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChljaXR5X21hcF9lbnRpdHlfZHRvLnByb3RvGiBwcm9kdWN0aW9uX3NvdXJj",
            "ZV9jb25zdGFudC5wcm90byLYAQoQQ2l0eU1hcEVudGl0eUR0bxIWCg5jaXR5",
            "X2VudGl0eV9pZBgCIAEoCRIJCgF4GAQgASgFEgkKAXkYBSABKAUSMAoLcHJv",
            "ZHVjdGlvbnMYBiADKAsyGy5DaXR5TWFwRW50aXR5UHJvZHVjdGlvbkR0bxIS",
            "Cgppc19yb3RhdGVkGAkgASgIEh8KF2N1c3RvbWl6YXRpb25fZW50aXR5X2lk",
            "GA8gASgJEiAKGGN1c3RvbWl6YXRpb25fYXBwbGllZF9hdBgQIAEoBRINCgVs",
            "ZXZlbBgSIAEoBSJyChpDaXR5TWFwRW50aXR5UHJvZHVjdGlvbkR0bxIVCg1k",
            "ZWZpbml0aW9uX2lkGAIgASgJEhIKCmlzX3N0YXJ0ZWQYCCABKAgSKQoGc291",
            "cmNlGA0gASgOMhkuUHJvZHVjdGlvblNvdXJjZUNvbnN0YW50Qh+qAhxJbmd3",
            "ZWxhbmQuRm9nLklubi5Nb2RlbHMuSG9oYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstantReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDto), global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDto.Parser, new[]{ "CityEntityId", "X", "Y", "Productions", "IsRotated", "CustomizationEntityId", "CustomizationAppliedAt", "Level" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityProductionDto), global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityProductionDto.Parser, new[]{ "DefinitionId", "IsStarted", "Source" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class CityMapEntityDto : pb::IMessage<CityMapEntityDto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CityMapEntityDto> _parser = new pb::MessageParser<CityMapEntityDto>(() => new CityMapEntityDto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CityMapEntityDto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CityMapEntityDto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CityMapEntityDto(CityMapEntityDto other) : this() {
      cityEntityId_ = other.cityEntityId_;
      x_ = other.x_;
      y_ = other.y_;
      productions_ = other.productions_.Clone();
      isRotated_ = other.isRotated_;
      customizationEntityId_ = other.customizationEntityId_;
      customizationAppliedAt_ = other.customizationAppliedAt_;
      level_ = other.level_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CityMapEntityDto Clone() {
      return new CityMapEntityDto(this);
    }

    /// <summary>Field number for the "city_entity_id" field.</summary>
    public const int CityEntityIdFieldNumber = 2;
    private string cityEntityId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string CityEntityId {
      get { return cityEntityId_; }
      set {
        cityEntityId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 4;
    private int x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 5;
    private int y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "productions" field.</summary>
    public const int ProductionsFieldNumber = 6;
    private static readonly pb::FieldCodec<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityProductionDto> _repeated_productions_codec
        = pb::FieldCodec.ForMessage(50, global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityProductionDto.Parser);
    private readonly pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityProductionDto> productions_ = new pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityProductionDto>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityProductionDto> Productions {
      get { return productions_; }
    }

    /// <summary>Field number for the "is_rotated" field.</summary>
    public const int IsRotatedFieldNumber = 9;
    private bool isRotated_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool IsRotated {
      get { return isRotated_; }
      set {
        isRotated_ = value;
      }
    }

    /// <summary>Field number for the "customization_entity_id" field.</summary>
    public const int CustomizationEntityIdFieldNumber = 15;
    private string customizationEntityId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string CustomizationEntityId {
      get { return customizationEntityId_; }
      set {
        customizationEntityId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "customization_applied_at" field.</summary>
    public const int CustomizationAppliedAtFieldNumber = 16;
    private int customizationAppliedAt_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CustomizationAppliedAt {
      get { return customizationAppliedAt_; }
      set {
        customizationAppliedAt_ = value;
      }
    }

    /// <summary>Field number for the "level" field.</summary>
    public const int LevelFieldNumber = 18;
    private int level_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int Level {
      get { return level_; }
      set {
        level_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as CityMapEntityDto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(CityMapEntityDto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (CityEntityId != other.CityEntityId) return false;
      if (X != other.X) return false;
      if (Y != other.Y) return false;
      if(!productions_.Equals(other.productions_)) return false;
      if (IsRotated != other.IsRotated) return false;
      if (CustomizationEntityId != other.CustomizationEntityId) return false;
      if (CustomizationAppliedAt != other.CustomizationAppliedAt) return false;
      if (Level != other.Level) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (CityEntityId.Length != 0) hash ^= CityEntityId.GetHashCode();
      if (X != 0) hash ^= X.GetHashCode();
      if (Y != 0) hash ^= Y.GetHashCode();
      hash ^= productions_.GetHashCode();
      if (IsRotated != false) hash ^= IsRotated.GetHashCode();
      if (CustomizationEntityId.Length != 0) hash ^= CustomizationEntityId.GetHashCode();
      if (CustomizationAppliedAt != 0) hash ^= CustomizationAppliedAt.GetHashCode();
      if (Level != 0) hash ^= Level.GetHashCode();
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
      if (CityEntityId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(CityEntityId);
      }
      if (X != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(X);
      }
      if (Y != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(Y);
      }
      productions_.WriteTo(output, _repeated_productions_codec);
      if (IsRotated != false) {
        output.WriteRawTag(72);
        output.WriteBool(IsRotated);
      }
      if (CustomizationEntityId.Length != 0) {
        output.WriteRawTag(122);
        output.WriteString(CustomizationEntityId);
      }
      if (CustomizationAppliedAt != 0) {
        output.WriteRawTag(128, 1);
        output.WriteInt32(CustomizationAppliedAt);
      }
      if (Level != 0) {
        output.WriteRawTag(144, 1);
        output.WriteInt32(Level);
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
      if (CityEntityId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(CityEntityId);
      }
      if (X != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(X);
      }
      if (Y != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(Y);
      }
      productions_.WriteTo(ref output, _repeated_productions_codec);
      if (IsRotated != false) {
        output.WriteRawTag(72);
        output.WriteBool(IsRotated);
      }
      if (CustomizationEntityId.Length != 0) {
        output.WriteRawTag(122);
        output.WriteString(CustomizationEntityId);
      }
      if (CustomizationAppliedAt != 0) {
        output.WriteRawTag(128, 1);
        output.WriteInt32(CustomizationAppliedAt);
      }
      if (Level != 0) {
        output.WriteRawTag(144, 1);
        output.WriteInt32(Level);
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
      if (CityEntityId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CityEntityId);
      }
      if (X != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(X);
      }
      if (Y != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Y);
      }
      size += productions_.CalculateSize(_repeated_productions_codec);
      if (IsRotated != false) {
        size += 1 + 1;
      }
      if (CustomizationEntityId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(CustomizationEntityId);
      }
      if (CustomizationAppliedAt != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(CustomizationAppliedAt);
      }
      if (Level != 0) {
        size += 2 + pb::CodedOutputStream.ComputeInt32Size(Level);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(CityMapEntityDto other) {
      if (other == null) {
        return;
      }
      if (other.CityEntityId.Length != 0) {
        CityEntityId = other.CityEntityId;
      }
      if (other.X != 0) {
        X = other.X;
      }
      if (other.Y != 0) {
        Y = other.Y;
      }
      productions_.Add(other.productions_);
      if (other.IsRotated != false) {
        IsRotated = other.IsRotated;
      }
      if (other.CustomizationEntityId.Length != 0) {
        CustomizationEntityId = other.CustomizationEntityId;
      }
      if (other.CustomizationAppliedAt != 0) {
        CustomizationAppliedAt = other.CustomizationAppliedAt;
      }
      if (other.Level != 0) {
        Level = other.Level;
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
          case 18: {
            CityEntityId = input.ReadString();
            break;
          }
          case 32: {
            X = input.ReadInt32();
            break;
          }
          case 40: {
            Y = input.ReadInt32();
            break;
          }
          case 50: {
            productions_.AddEntriesFrom(input, _repeated_productions_codec);
            break;
          }
          case 72: {
            IsRotated = input.ReadBool();
            break;
          }
          case 122: {
            CustomizationEntityId = input.ReadString();
            break;
          }
          case 128: {
            CustomizationAppliedAt = input.ReadInt32();
            break;
          }
          case 144: {
            Level = input.ReadInt32();
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
            CityEntityId = input.ReadString();
            break;
          }
          case 32: {
            X = input.ReadInt32();
            break;
          }
          case 40: {
            Y = input.ReadInt32();
            break;
          }
          case 50: {
            productions_.AddEntriesFrom(ref input, _repeated_productions_codec);
            break;
          }
          case 72: {
            IsRotated = input.ReadBool();
            break;
          }
          case 122: {
            CustomizationEntityId = input.ReadString();
            break;
          }
          case 128: {
            CustomizationAppliedAt = input.ReadInt32();
            break;
          }
          case 144: {
            Level = input.ReadInt32();
            break;
          }
        }
      }
    }
    #endif

  }

  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class CityMapEntityProductionDto : pb::IMessage<CityMapEntityProductionDto>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CityMapEntityProductionDto> _parser = new pb::MessageParser<CityMapEntityProductionDto>(() => new CityMapEntityProductionDto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<CityMapEntityProductionDto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.CityMapEntityDtoReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CityMapEntityProductionDto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CityMapEntityProductionDto(CityMapEntityProductionDto other) : this() {
      definitionId_ = other.definitionId_;
      isStarted_ = other.isStarted_;
      source_ = other.source_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public CityMapEntityProductionDto Clone() {
      return new CityMapEntityProductionDto(this);
    }

    /// <summary>Field number for the "definition_id" field.</summary>
    public const int DefinitionIdFieldNumber = 2;
    private string definitionId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string DefinitionId {
      get { return definitionId_; }
      set {
        definitionId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "is_started" field.</summary>
    public const int IsStartedFieldNumber = 8;
    private bool isStarted_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool IsStarted {
      get { return isStarted_; }
      set {
        isStarted_ = value;
      }
    }

    /// <summary>Field number for the "source" field.</summary>
    public const int SourceFieldNumber = 13;
    private global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant source_ = global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant.Main;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant Source {
      get { return source_; }
      set {
        source_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as CityMapEntityProductionDto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(CityMapEntityProductionDto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (DefinitionId != other.DefinitionId) return false;
      if (IsStarted != other.IsStarted) return false;
      if (Source != other.Source) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (DefinitionId.Length != 0) hash ^= DefinitionId.GetHashCode();
      if (IsStarted != false) hash ^= IsStarted.GetHashCode();
      if (Source != global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant.Main) hash ^= Source.GetHashCode();
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
      if (DefinitionId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(DefinitionId);
      }
      if (IsStarted != false) {
        output.WriteRawTag(64);
        output.WriteBool(IsStarted);
      }
      if (Source != global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant.Main) {
        output.WriteRawTag(104);
        output.WriteEnum((int) Source);
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
      if (DefinitionId.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(DefinitionId);
      }
      if (IsStarted != false) {
        output.WriteRawTag(64);
        output.WriteBool(IsStarted);
      }
      if (Source != global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant.Main) {
        output.WriteRawTag(104);
        output.WriteEnum((int) Source);
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
      if (DefinitionId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DefinitionId);
      }
      if (IsStarted != false) {
        size += 1 + 1;
      }
      if (Source != global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant.Main) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Source);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(CityMapEntityProductionDto other) {
      if (other == null) {
        return;
      }
      if (other.DefinitionId.Length != 0) {
        DefinitionId = other.DefinitionId;
      }
      if (other.IsStarted != false) {
        IsStarted = other.IsStarted;
      }
      if (other.Source != global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant.Main) {
        Source = other.Source;
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
          case 18: {
            DefinitionId = input.ReadString();
            break;
          }
          case 64: {
            IsStarted = input.ReadBool();
            break;
          }
          case 104: {
            Source = (global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant) input.ReadEnum();
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
            DefinitionId = input.ReadString();
            break;
          }
          case 64: {
            IsStarted = input.ReadBool();
            break;
          }
          case 104: {
            Source = (global::Ingweland.Fog.Inn.Models.Hoh.ProductionSourceConstant) input.ReadEnum();
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
