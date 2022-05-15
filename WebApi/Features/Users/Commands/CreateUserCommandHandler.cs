using MediatR;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;

namespace WebApi.Features.Users.Commands;

public class CreateUserRequest : IRequest<string>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserRequest, string>
{
    private readonly ProjectDbContext _projectDbContext;
    private readonly IFireBaseService _firebaseService;

    public CreateUserCommandHandler(ProjectDbContext projectDbContext, IFireBaseService firebaseService)
    {
        _projectDbContext = projectDbContext;
        _firebaseService = firebaseService;
    }

    public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        //send information to firebase

        var firebaseUser = await _firebaseService.CreateUser(request.Email, request.Password);

        _projectDbContext.Users.Add(new Domain.Entities.User()
        {
            Id = firebaseUser.Uid,
            Email = firebaseUser.Email
        });

        await _projectDbContext.SaveChangesAsync(cancellationToken);

        return firebaseUser.Uid;
    }
}