using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;

namespace WebApi.Features.Users.Commands;

public class DeleteUserRequest : IRequest<string>
{
    public string UserId { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserRequest, string>
{
    private readonly ProjectDbContext _projectDbContext;

    public DeleteUserCommandHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<string> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _projectDbContext.Users.SingleAsync(x => x.Id == request.UserId);

        user.IsDeleted = true;

        await _projectDbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}