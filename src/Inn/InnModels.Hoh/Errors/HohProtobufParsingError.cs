using FluentResults;

namespace Ingweland.Fog.Inn.Models.Hoh.Errors;

public class HohProtobufParsingError:Error
{
    public HohProtobufParsingError(ProtobufParsingStage stage, string messageType, Exception? exception = null) 
    {
        Message = $"Protobuf parsing error in stage {stage} for message type {messageType}";
        Stage = stage;
        MessageType = messageType;

        WithMetadata("Stage", stage);
        WithMetadata("MessageType", messageType);

        if (exception != null)
        {
            CausedBy(exception);
        }
    }

    public string MessageType { get; }
    public ProtobufParsingStage Stage { get; }
}
