using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Entities;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;
using WebApi.Services;

namespace WebApi.Features.Tenants.Queries
{
    public class GetTenantRequest : IRequest<GetTenantResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetTenantResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Domain { get; set; }
        public List<GetTenantUserResponse> Users { get; set; }
    }

    public class GetTenantUserResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }

    public class GetTenantQueryHandler : IRequestHandler<GetTenantRequest, GetTenantResponse>
    {
        private readonly ProjectDbContext _projectDbContext;

        public GetTenantQueryHandler(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<GetTenantResponse> Handle(GetTenantRequest request, CancellationToken cancellationToken)
        {
            //query
            var tenant = await _projectDbContext.Tenants.Include(x => x.Users).SingleAsync(x => x.Id == request.Id);
            //mapping
            return new GetTenantResponse()
            {
                Id = tenant.Id,
                Country = tenant.Country,
                Domain = tenant.Domain,
                Name = tenant.Name,
                Users = tenant.Users.Select(x => new GetTenantUserResponse()
                {
                    Id = x.Id,
                    Email = x.Email
                }).ToList()
            };
        }
    }
}