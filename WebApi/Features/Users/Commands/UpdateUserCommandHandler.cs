using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;

namespace WebApi.Features.Users.Commands;

public class UpdateUserRequest : IRequest<string>
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserRequest, string>
{
    private readonly ProjectDbContext _projectDbContext;

    public UpdateUserCommandHandler(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<string> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {

        var user = await _projectDbContext.Users.SingleAsync(x => x.Id == request.Id);
        
        //update user here
        
        
        
        _projectDbContext.Users.Update(user);

        await _projectDbContext.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}