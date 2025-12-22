using FluentResults;

namespace Ingweland.Fog.Application.Server.Errors;

public class DataTooLargeError : Error
{
    public DataTooLargeError(int size) : base("The provided data exceeds the maximum allowed size.")
    {
        Metadata.Add("Size", size);
    }
}
