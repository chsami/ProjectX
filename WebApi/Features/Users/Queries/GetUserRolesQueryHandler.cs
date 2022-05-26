using Microsoft.EntityFrameworkCore;
using WebApi.Features.Products.Queries;
using WebApi.Infrastructure.Database;
using MediatR;

namespace WebApi.Features.Users.Queries;

public class GetUserRolesRequest : IRequest<List<GetUserRolesResponse>>
{
    public string UserId { get; set; }
}

public class GetUserRolesResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Selected { get; set; }
}

public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesRequest, List<GetUserRolesResponse>>
{
    private readonly ProjectDbContext _projectDbContext;

    public GetUserRolesQueryHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<List<GetUserRolesResponse>> Handle(GetUserRolesRequest request, CancellationToken cancellationToken)
    {
        //query
        var user = await _projectDbContext.Users.Include(x => x.Roles).SingleAsync(x => x.Id == request.UserId);

        var roles = await _projectDbContext.Roles.ToListAsync();

        return roles.Select(x => new GetUserRolesResponse()
        {
            Id = x.Id,
            Name = x.Name,
            Selected = user.Roles.Select(c => c.Id).Contains(x.Id)
        }).ToList();
    }
}