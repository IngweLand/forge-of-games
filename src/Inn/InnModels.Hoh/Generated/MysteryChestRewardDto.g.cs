// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: mystery_chest_reward_dto.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Ingweland.Fog.Inn.Models.Hoh {

  /// <summary>Holder for reflection information generated from mystery_chest_reward_dto.proto</summary>
  public static partial class MysteryChestRewardDtoReflection {

    #region Descriptor
    /// <summary>File descriptor for mystery_chest_reward_dto.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MysteryChestRewardDtoReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch5teXN0ZXJ5X2NoZXN0X3Jld2FyZF9kdG8ucHJvdG8aDGlkX2R0by5wcm90",
            "bxoZZ29vZ2xlL3Byb3RvYnVmL2FueS5wcm90byJ3ChVNeXN0ZXJ5Q2hlc3RS",
            "ZXdhcmREVE8SEgoCaWQYASABKAsyBi5JZER0bxIsCg5wYWNrZWRfcmV3YXJk",
            "cxgCIAMoCzIULmdvb2dsZS5wcm90b2J1Zi5BbnkSHAoUcmV3YXJkX3Byb2Jh",
            "YmlsaXRpZXMYAyABKAxCH6oCHEluZ3dlbGFuZC5Gb2cuSW5uLk1vZGVscy5I",
            "b2hiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Ingweland.Fog.Inn.Models.Hoh.IdDtoReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.AnyReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Ingweland.Fog.Inn.Models.Hoh.MysteryChestRewardDTO), global::Ingweland.Fog.Inn.Models.Hoh.MysteryChestRewardDTO.Parser, new[]{ "Id", "PackedRewards", "RewardProbabilities" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  public sealed partial class MysteryChestRewardDTO : pb::IMessage<MysteryChestRewardDTO>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<MysteryChestRewardDTO> _parser = new pb::MessageParser<MysteryChestRewardDTO>(() => new MysteryChestRewardDTO());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<MysteryChestRewardDTO> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Ingweland.Fog.Inn.Models.Hoh.MysteryChestRewardDtoReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public MysteryChestRewardDTO() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public MysteryChestRewardDTO(MysteryChestRewardDTO other) : this() {
      id_ = other.id_ != null ? other.id_.Clone() : null;
      packedRewards_ = other.packedRewards_.Clone();
      rewardProbabilities_ = other.rewardProbabilities_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public MysteryChestRewardDTO Clone() {
      return new MysteryChestRewardDTO(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private global::Ingweland.Fog.Inn.Models.Hoh.IdDto id_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::Ingweland.Fog.Inn.Models.Hoh.IdDto Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "packed_rewards" field.</summary>
    public const int PackedRewardsFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Google.Protobuf.WellKnownTypes.Any> _repeated_packedRewards_codec
        = pb::FieldCodec.ForMessage(18, global::Google.Protobuf.WellKnownTypes.Any.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.WellKnownTypes.Any> packedRewards_ = new pbc::RepeatedField<global::Google.Protobuf.WellKnownTypes.Any>();
    /// <summary>
    /// ResourceRewardDTO, HeroRewardDTO, RewardDefinitionDTO, DynamicActionChangeRewardDTO
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pbc::RepeatedField<global::Google.Protobuf.WellKnownTypes.Any> PackedRewards {
      get { return packedRewards_; }
    }

    /// <summary>Field number for the "reward_probabilities" field.</summary>
    public const int RewardProbabilitiesFieldNumber = 3;
    private pb::ByteString rewardProbabilities_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pb::ByteString RewardProbabilities {
      get { return rewardProbabilities_; }
      set {
        rewardProbabilities_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as MysteryChestRewardDTO);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(MysteryChestRewardDTO other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Id, other.Id)) return false;
      if(!packedRewards_.Equals(other.packedRewards_)) return false;
      if (RewardProbabilities != other.RewardProbabilities) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (id_ != null) hash ^= Id.GetHashCode();
      hash ^= packedRewards_.GetHashCode();
      if (RewardProbabilities.Length != 0) hash ^= RewardProbabilities.GetHashCode();
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
      if (id_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Id);
      }
      packedRewards_.WriteTo(output, _repeated_packedRewards_codec);
      if (RewardProbabilities.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(RewardProbabilities);
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
      if (id_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Id);
      }
      packedRewards_.WriteTo(ref output, _repeated_packedRewards_codec);
      if (RewardProbabilities.Length != 0) {
        output.WriteRawTag(26);
        output.WriteBytes(RewardProbabilities);
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
      if (id_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Id);
      }
      size += packedRewards_.CalculateSize(_repeated_packedRewards_codec);
      if (RewardProbabilities.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(RewardProbabilities);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(MysteryChestRewardDTO other) {
      if (other == null) {
        return;
      }
      if (other.id_ != null) {
        if (id_ == null) {
          Id = new global::Ingweland.Fog.Inn.Models.Hoh.IdDto();
        }
        Id.MergeFrom(other.Id);
      }
      packedRewards_.Add(other.packedRewards_);
      if (other.RewardProbabilities.Length != 0) {
        RewardProbabilities = other.RewardProbabilities;
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
            if (id_ == null) {
              Id = new global::Ingweland.Fog.Inn.Models.Hoh.IdDto();
            }
            input.ReadMessage(Id);
            break;
          }
          case 18: {
            packedRewards_.AddEntriesFrom(input, _repeated_packedRewards_codec);
            break;
          }
          case 26: {
            RewardProbabilities = input.ReadBytes();
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
            if (id_ == null) {
              Id = new global::Ingweland.Fog.Inn.Models.Hoh.IdDto();
            }
            input.ReadMessage(Id);
            break;
          }
          case 18: {
            packedRewards_.AddEntriesFrom(ref input, _repeated_packedRewards_codec);
            break;
          }
          case 26: {
            RewardProbabilities = input.ReadBytes();
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
