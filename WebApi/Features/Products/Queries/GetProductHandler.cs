using MediatR;
using WebApi.Infrastructure.Database;

namespace WebApi.Features.Products.Queries
{
    public class GetProductRequest : IRequest<GetProductResponse>
    {
        public int Id { get; set; }
    }

    public class GetProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class GetProductHandlerHandler : IRequestHandler<GetProductRequest, GetProductResponse>
    {
        private readonly ProjectDbContext _projectDbContext;

        public GetProductHandlerHandler(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<GetProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
        {
            //query
            var product = _projectDbContext.Products.Single(x => x.Id == request.Id);

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
