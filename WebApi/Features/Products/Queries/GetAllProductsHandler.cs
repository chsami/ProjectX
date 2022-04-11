using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;
using WebApi.Services;

namespace WebApi.Features.Products.Queries
{
    public class GetAllProductsRequest : IRequest<List<GetAllProductsResponse>>
    {
    }

    public class GetAllProductsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GetAllProductsHandlerHandler : IRequestHandler<GetAllProductsRequest, List<GetAllProductsResponse>>
    {
        private readonly ProjectDbContext _projectDbContext;

        public GetAllProductsHandlerHandler(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<List<GetAllProductsResponse>> Handle(GetAllProductsRequest request, CancellationToken cancellationToken)
        {
            //query
            var products = _projectDbContext.Products;

            //mapping
            var response = await products.Select(x => new GetAllProductsResponse() {Id = x.Id, Name = x.Name})
                .ToListAsync(cancellationToken: cancellationToken);
            
            return response;
        }
    }
}