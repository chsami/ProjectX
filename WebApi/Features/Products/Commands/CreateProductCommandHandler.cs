using MediatR;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;
using WebApi.Infrastructure.Services.Firebase.Models;
using WebApi.Services;

namespace WebApi.Features.Products.Commands
{
    public class CreateProductRequest : IRequest<Guid>
    {
        public string Name { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductRequest, Guid>
    {
        private readonly ProjectDbContext _projectDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFireBaseService _fireBaseService;

        public CreateProductCommandHandler(ProjectDbContext projectDbContext, ICurrentUserService currentUserService, IFireBaseService fireBaseService)
        {
            _projectDbContext = projectDbContext;
            _currentUserService = currentUserService;
            _fireBaseService = fireBaseService;
        }

        public async Task<Guid> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            var productToAdd = new Domain.Product()
            {
                Name = request.Name
            };

            _projectDbContext.Products.Add(productToAdd);

            await _projectDbContext.SaveChangesAsync(_currentUserService.UserId, cancellationToken);

            await _fireBaseService.SaveDocument(new ProductDocument()
            {
                Name = productToAdd.Name
            });

            return productToAdd.Id;
        }
    }
}
