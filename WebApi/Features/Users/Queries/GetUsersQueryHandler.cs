using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Extensions;
using WebApi.Domain.Entities;
using WebApi.Features.Tenants.Queries;
using WebApi.Infrastructure.Database;

namespace WebApi.Features.Users.Queries;

public class GetUsersRequest : IRequest<List<GetUsersResponse>>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}

public class GetUsersResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
    public DateTime CreatedOn { get; set; }
    public List<string> Roles { get; set; }
    public List<string> Tenants { get; set; }
    public int TotalCount { get; set; }
}


public class GetUsersQueryHandler : IRequestHandler<GetUsersRequest, List<GetUsersResponse>>
{
    private readonly ProjectDbContext _projectDbContext;

    public GetUsersQueryHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<List<GetUsersResponse>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
    {
        //query
        var users = await _projectDbContext.Users
            .Include(x => x.Roles)
            .Include(x => x.Tenants)
            .Paginate(request.PageSize, request.PageNumber).ToListAsync();

        var totalCount = await _projectDbContext.Users.CountAsync();
        
        //mapping
        return users.Select(x => new GetUsersResponse()
        {
            Id = x.Id,
            Email = x.Email,
            CreatedOn = x.CreatedOn,
            Roles = x.Roles.Select(r => r.Name).ToList(),
            Tenants = x.Tenants.Select(t => t.Name).ToList(),
            TotalCount = totalCount
        }).ToList();
    }
}
