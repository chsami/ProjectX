using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Features.Tenants.Queries;
using WebApi.Infrastructure.Database;

namespace WebApi.Features.Users.Queries;

public class GetUserByEmailRequest : IRequest<GetUserByEmailResponse>
{
    public string Email { get; set; }
}

public class GetUserByEmailResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
}

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailRequest, GetUserByEmailResponse>
{
    private readonly ProjectDbContext _projectDbContext;

    public GetUserByEmailQueryHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<GetUserByEmailResponse> Handle(GetUserByEmailRequest request, CancellationToken cancellationToken)
    {
        //query
        var user = await _projectDbContext.Users.SingleAsync(x => x.Email == request.Email);
        //mapping
        return new GetUserByEmailResponse()
        {
            Id = user.Id,
            Email = user.Email
        };
    }
}