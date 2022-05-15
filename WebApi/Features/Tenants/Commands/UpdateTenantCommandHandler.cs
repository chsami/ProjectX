using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Infrastructure.Database;

namespace WebApi.Features.Tenants.Commands;

public class UpdateTenantRequest : IRequest<UpdateTenantResponse>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
    public string Domain { get; set; }
    public List<string> UserIds { get; set; }
}

public class UpdateTenantResponse
{
    public Guid Id { get; set; }
}

public class UpdateTenantQueryHandler : IRequestHandler<UpdateTenantRequest, UpdateTenantResponse>
{
    private readonly ProjectDbContext _projectDbContext;

    public UpdateTenantQueryHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<UpdateTenantResponse> Handle(UpdateTenantRequest request, CancellationToken cancellationToken)
    {
        var tenant = await _projectDbContext.Tenants.Include(x => x.Users).SingleAsync(x => x.Id == request.Id);
        tenant.Users.Clear();

        tenant.Name = request.Name;
        tenant.Country = request.Country;
        tenant.Domain = request.Domain;
        tenant.Users =  await _projectDbContext.Users.Where(x => request.UserIds.Contains(x.Id)).ToListAsync();

        _projectDbContext.Tenants.Update(tenant);

        await _projectDbContext.SaveChangesAsync(cancellationToken);

        return new UpdateTenantResponse()
        {
            Id = tenant.Id
        };
    }
}