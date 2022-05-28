using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;

namespace WebApi.Features.Users.Commands;
public class AddUserRolesRequest : IRequest<List<string>>
{
    public string UserId { get; set; }
    public List<Guid> Roles { get; set; }
}

public class AddUserRolesCommandHandler : IRequestHandler<AddUserRolesRequest, List<string>>
{
    private readonly ProjectDbContext _projectDbContext;
    private readonly IFireBaseService _fireBaseService;

    public AddUserRolesCommandHandler(ProjectDbContext projectDbContext, IFireBaseService fireBaseService)
    {
        _projectDbContext = projectDbContext;
        _fireBaseService = fireBaseService;
    }

    public async Task<List<string>> Handle(AddUserRolesRequest request, CancellationToken cancellationToken)
    {
        var user = await _projectDbContext.Users.Include(x => x.Roles).SingleAsync(x => x.Id == request.UserId);

        if (!request.Roles.Any())
        {
            user.Roles.Clear();
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return new List<string>();
        }

        var roles = await _projectDbContext.Roles.Where(x => request.Roles.Contains(x.Id)).ToListAsync();

        if (!roles.Any()) throw new ArgumentException("Roles not found.");

        user.Roles.Clear();
        
        user.Roles.AddRange(roles);
        
        await _projectDbContext.SaveChangesAsync(cancellationToken);
        
        await _fireBaseService.SetCustomUserClaimsAsync(user.Id, roles.Select(role => role.Name).ToList());

        return user.Roles.Select(x => x.Name).ToList();
    }
}