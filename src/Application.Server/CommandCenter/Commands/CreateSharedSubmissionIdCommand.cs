using Ingweland.Fog.Application.Core.Constants;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Models.Fog.Entities;
using MediatR;

namespace Ingweland.Fog.Application.Server.CommandCenter.Commands;

public record CreateSharedSubmissionIdCommand(Guid SubmissionId) : IRequest<Guid>;

public class CreateSharedSubmissionIdCommandHandler(IFogDbContext context)
    : IRequestHandler<CreateSharedSubmissionIdCommand, Guid>
{
    public async Task<Guid> Handle(CreateSharedSubmissionIdCommand request, CancellationToken cancellationToken)
    {
        var sharedSubmissionId = new SharedSubmissionIdEntity
        {
            SubmissionId = request.SubmissionId,
            SharedId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddDays(FogConstants.SHARED_SUBMISSION_ID_VALIDITY_DAYS),
        };

        context.SharedSubmissionIds.Add(sharedSubmissionId);
        await context.SaveChangesAsync(cancellationToken);
        return sharedSubmissionId.SharedId;
    }
}
