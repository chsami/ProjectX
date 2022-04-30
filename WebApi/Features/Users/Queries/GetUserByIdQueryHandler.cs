using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Features.Tenants.Queries;
using WebApi.Infrastructure.Database;

namespace WebApi.Features.Users.Queries;

public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
{
    public string Id { get; set; }
}

public class GetUserByIdResponse
{
    public string Id { get; set; }
    public string Email { get; set; }
}

public class GetUserQueryHandler : IRequestHandler<GetUserByIdRequest, GetUserByIdResponse>
{
    private readonly ProjectDbContext _projectDbContext;

    public GetUserQueryHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<GetUserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        //query
        var user = await _projectDbContext.Users.SingleAsync(x => x.Id == request.Id);
        //mapping
        return new GetUserByIdResponse()
        {
            Id = user.Id,
            Email = user.Email
        };
    }
}