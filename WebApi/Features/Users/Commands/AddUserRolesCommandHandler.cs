﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Services.Firebase;
using WebApi.Services;

namespace WebApi.Features.Users.Commands;
public class AddUserRolesRequest : IRequest<List<string>>
{
    public string UserId { get; set; }
    public List<string> Roles { get; set; }
}

public class AddUserRolesCommandHandler : IRequestHandler<AddUserRolesRequest, List<string>>
{
    private readonly ProjectDbContext _projectDbContext;
    private readonly ICurrentUserService _currentUserService;

    public AddUserRolesCommandHandler(ProjectDbContext projectDbContext,  ICurrentUserService currentUserService)
    {
        _projectDbContext = projectDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<List<string>> Handle(AddUserRolesRequest request, CancellationToken cancellationToken)
    {
        var user = await _projectDbContext.Users.Include(x => x.Roles).SingleAsync(x => x.Id == request.UserId);

        var roles = await _projectDbContext.Roles.Where(x => request.Roles.Contains(x.Name)).ToListAsync();

        if (!roles.Any()) throw new ArgumentException("Roles not found.");

        user.Roles.Clear();
        
        user.Roles.AddRange(roles);
        
        await _projectDbContext.SaveChangesAsync(_currentUserService.UserId, cancellationToken);

        return user.Roles.Select(x => x.Name).ToList();
    }
}