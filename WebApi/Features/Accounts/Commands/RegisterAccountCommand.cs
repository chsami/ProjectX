using MediatR;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;
using WebApi.Services;

namespace WebApi.Features.Accounts.Commands;

public class RegisterAccountRequest : IRequest<int>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountRequest, int>
{
    private readonly ProjectDbContext _projectDbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFireBaseService _firebaseService;

    public RegisterAccountCommandHandler(ProjectDbContext projectDbContext, ICurrentUserService currentUserService, IFireBaseService firebaseService)
    {
        _projectDbContext = projectDbContext;
        _currentUserService = currentUserService;
        _firebaseService = firebaseService;
    }

    public async Task<int> Handle(RegisterAccountRequest request, CancellationToken cancellationToken)
    {
        //send information to firebase

        var firebaseUser = await _firebaseService.CreateUser(request.Email, request.Password);

        _projectDbContext.Users.Add(new Domain.Entities.User()
        {
            Id = firebaseUser.Uid,
            Email = firebaseUser.Email
        });

        await _projectDbContext.SaveChangesAsync(firebaseUser.Uid, cancellationToken);

        return 0;
    }
}