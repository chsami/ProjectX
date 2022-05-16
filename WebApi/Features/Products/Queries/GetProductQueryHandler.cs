using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;

namespace WebApi.Features.Products.Queries
{
    public class GetProductRequest : IRequest<GetProductResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class GetProductQueryHandler : IRequestHandler<GetProductRequest, GetProductResponse>
    {
        private readonly ProjectDbContext _projectDbContext;

        public GetProductQueryHandler(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<GetProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            //query
            var product = await _projectDbContext.Products.SingleAsync(x => x.Id == request.Id);

            //mapping
            var response = new GetProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
            };

            return response;
        }
    }
}
