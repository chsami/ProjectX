using Microsoft.EntityFrameworkCore;
using WebApi.Features.Products.Queries;
using WebApi.Infrastructure.Database;
using MediatR;

namespace WebApi.Features.Accounts.Queries;

public class GetRolesRequest : IRequest<List<GetRolesResponse>>
{
    public string UserId { get; set; }
}

public class GetRolesResponse
{
    public string Name { get; set; }
}

public class GetProductHandlerHandler : IRequestHandler<GetRolesRequest, List<GetRolesResponse>>
{
    private readonly ProjectDbContext _projectDbContext;

    public GetProductHandlerHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<List<GetRolesResponse>> Handle(GetRolesRequest request, CancellationToken cancellationToken)
    {
        //query
        var user = await _projectDbContext.Users.Include(x => x.Roles).SingleAsync(x => x.Id == request.UserId);

        return user.Roles.Select(x => new GetRolesResponse()
        {
            Name = x.Name
        }).ToList();
    }
}