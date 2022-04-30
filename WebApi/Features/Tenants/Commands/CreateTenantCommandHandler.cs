using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Infrastructure.Database;
using WebApi.Services;

namespace WebApi.Features.Tenants.Commands;

public class CreateTenantRequest : IRequest<CreateTenantResponse>
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string Domain { get; set; }
    public List<string> UserIds { get; set; }
}

public class CreateTenantResponse
{
    public Guid Id { get; set; }
}

public class CreateTenantQueryHandler : IRequestHandler<CreateTenantRequest, CreateTenantResponse>
{
    private readonly ProjectDbContext _projectDbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateTenantQueryHandler(ProjectDbContext projectDbContext, ICurrentUserService currentUserService)
    {
        _projectDbContext = projectDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<CreateTenantResponse> Handle(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        //mapping
        var tenant = new Tenant()
        {
            Name = request.Name,
            Country = request.Country,
            Domain = request.Domain,
            Users =  await _projectDbContext.Users.Where(x => request.UserIds.Contains(x.Id)).ToListAsync()
        };
        
        //query
        await _projectDbContext.Tenants.AddAsync(tenant);
        
        await _projectDbContext.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
        
        //mapping
        return new CreateTenantResponse()
        {
            Id = tenant.Id
        };
    }
}