using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Extensions;
using WebApi.Domain.Entities;
using WebApi.Features.Tenants.Queries;
using WebApi.Infrastructure.Database;

namespace WebApi.Features.Users.Queries;

public class GetUsersRequest : IRequest<GetUsersResponse>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}

public class GetUsersResponse
{
    public List<User> Users { get; set; }

}

public class GetUsersQueryHandler : IRequestHandler<GetUsersRequest, GetUsersResponse>
{
    private readonly ProjectDbContext _projectDbContext;

    public GetUsersQueryHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<GetUsersResponse> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        //query
        var users = await _projectDbContext.Users.Include(x => x.Roles).Include(x => x.Tenants).Paginate(request.PageSize, request.PageNumber).ToListAsync();
        //mapping
        return new GetUsersResponse()
        {
            Users = users
        };
    }
}