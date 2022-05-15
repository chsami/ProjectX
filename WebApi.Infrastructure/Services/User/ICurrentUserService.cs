namespace WebApi.Infrastructure;

public interface ICurrentUserService
{
    string UserId { get; }
}
