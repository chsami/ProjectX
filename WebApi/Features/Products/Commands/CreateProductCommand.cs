using MediatR;
using WebApi.Infrastructure.Database;

namespace WebApi.Features.Products.Commands
{
    public class CreateProductRequest : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductRequest, int>
    {
        private readonly ProjectDbContext _projectDbContext;


        public CreateProductCommandHandler(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<int> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productToAdd = new Domain.Product()
            {
                Name = request.Name
            };

            _projectDbContext.Products.Add(productToAdd);

            await _projectDbContext.SaveChangesAsync("TODO userId", cancellationToken);

            return productToAdd.Id;
        }
    }
}
