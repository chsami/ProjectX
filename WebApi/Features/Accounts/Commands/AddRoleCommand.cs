using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;
using WebApi.Services;

namespace WebApi.Features.Accounts.Commands;
public class AddRoleRequest : IRequest
{
    public string UserId { get; set; }
    public List<string> Roles { get; set; }
}

public class AddRoleCommandHandler : IRequestHandler<AddRoleRequest>
{
    private readonly ProjectDbContext _projectDbContext;
    private readonly ICurrentUserService _currentUserService;

    public AddRoleCommandHandler(ProjectDbContext projectDbContext,  ICurrentUserService currentUserService)
    {
        _projectDbContext = projectDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(AddRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await _projectDbContext.Users.SingleAsync(x => x.Id == request.UserId);

        var roles = await _projectDbContext.Roles.Where(x => request.Roles.Contains(x.Name)).ToListAsync();

        if (!roles.Any()) throw new ArgumentException("Roles not found.");
        
        user.Roles.AddRange(roles);
        
        await _projectDbContext.SaveChangesAsync(_currentUserService.UserId, cancellationToken);

        return Unit.Value;
    }
}