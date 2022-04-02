using MediatR;

namespace WebApi.Features.Products.Commands
{
    public class CreateProductRequest : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductRequest, int>
    {

        public CreateProductCommandHandler()
        {
        }

        public async Task<int> Handle(CreateProductRequest request, CancellationToken cancellationToken)
        {
            return 0;
        }
    }
}
